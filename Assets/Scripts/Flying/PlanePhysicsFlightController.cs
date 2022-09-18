using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePhysicsFlightController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _underwaterDrag = 2.0f;
    [SerializeField] float _underwaterAngularDrag = 5.0f;
    [SerializeField] float _amountOfGravityToShiftToCenter = 0.0f;
    [SerializeField] float _amountOfResistanceToShiftToCenter = 0.0f;
    [SerializeField] float _amountOfEnginesToShiftToCenter = 0.0f;

    private PhysicsAffector[] _physicsAffectors;
    private EngineAffector[] _engineAffectors;
    public PhysicsAffector[] physicsAffectors => _physicsAffectors;
    private PlanePhysicsData _planePhysicsData;
    private float _forcesMultiplier = 1;
    private float _maxVelocity = 120f;

    protected void Start()
    {
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    public void ProcessFlight(bool applyEngineForce)
    {
        ApplyForcesForAffectors();
        if (applyEngineForce)
        {
            ApplyForcesForEngines();
        }
        LimitMaxVelocity();
    }

    private void LimitMaxVelocity()
    {
        if(_rigidbody.velocity.magnitude > _maxVelocity)
        {
            _rigidbody.velocity = _maxVelocity * (_rigidbody.velocity / _rigidbody.velocity.magnitude);
        }
    }

    private void ApplyForcesForAffectors()
    {
        foreach (var affector in _physicsAffectors)
        {
            //Add gravity
            var gravityForceToAdd = _forcesMultiplier * affector.weight * Physics.gravity;
            _rigidbody.AddForceAtPosition((1 - _amountOfGravityToShiftToCenter) * gravityForceToAdd, affector.position);
            _rigidbody.AddForce((_amountOfGravityToShiftToCenter) * gravityForceToAdd);
            //Add resistance
            
            var positionVector = transform.position - affector.position;
            var angle = Vector3.Angle(positionVector, Vector3.up);
            var multiplier = Mathf.Sin(Mathf.Deg2Rad * angle);
            var resistanceForceToAdd = multiplier * (_forcesMultiplier * affector.resistance * Vector3.up);
            _rigidbody.AddForceAtPosition((1 - _amountOfResistanceToShiftToCenter) * ((resistanceForceToAdd)), affector.position);
            _rigidbody.AddForce((_amountOfResistanceToShiftToCenter) * ((resistanceForceToAdd)));
        }
    }

    private void ApplyForcesForEngines()
    {
        foreach (var engineAffector in _engineAffectors)
        {
            var engineForceToAdd = - ((_forcesMultiplier * engineAffector.engineSrength * engineAffector.engineThrustDirection));
            _rigidbody.AddForceAtPosition((1 - _amountOfEnginesToShiftToCenter) * engineForceToAdd, engineAffector.centerTransform.position);
            _rigidbody.AddForce((_amountOfEnginesToShiftToCenter) * engineForceToAdd);
        }
    }

    public void SetWaterMultipliers()
    {
        _forcesMultiplier = 1f;
        _rigidbody.drag = _underwaterDrag;
        _rigidbody.angularDrag = _underwaterAngularDrag;
    }

    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {
        if(gameState == GameStateManager.GameState.Transition)
        {
            _physicsAffectors = GetComponentsInChildren<PhysicsAffector>();
            _engineAffectors = GetComponentsInChildren<EngineAffector>();
        }
    }
}
