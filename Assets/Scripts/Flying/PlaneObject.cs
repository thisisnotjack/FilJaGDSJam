using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _underwaterDrag = 2.0f;
    [SerializeField] float _underwaterAngularDrag = 5.0f;
    private PhysicsAffector[] _physicsAffectors;
    private EngineAffector[] _engineAffectors;
    public PhysicsAffector[] physicsAffectors => _physicsAffectors;
    private PlanePhysicsData _planePhysicsData;

    private float _forcesMultiplier = 1;

    void Start()
    {
        _physicsAffectors = GetComponentsInChildren<PhysicsAffector>();
        _engineAffectors = GetComponentsInChildren<EngineAffector>();
    }

    void Update()
    {
        ProcessFlight();
    }

    private void ProcessFlight()
    {
        ApplyForcesForAffectors();
        ApplyForcesForEngines();
    }

    private void ApplyForcesForAffectors()
    {
        foreach(var affector in _physicsAffectors)
        {
            //Add gravity
            _rigidbody.AddForceAtPosition(_forcesMultiplier * affector.weight * Physics.gravity, affector.position);
            //Add resistance
            _rigidbody.AddForceAtPosition(affector.transform.rotation * (_forcesMultiplier * affector.resistance * Vector3.up), affector.position);
        }
    }

    private void ApplyForcesForEngines()
    {
        foreach(var engineAffector in _engineAffectors)
        {
            _rigidbody.AddForceAtPosition(engineAffector.centerTransform.rotation * (_forcesMultiplier * engineAffector.engineSrength * engineAffector.engineThrustDirection), engineAffector.centerTransform.position);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        var waterLayerMask = LayerMask.GetMask("Water");
        if (waterLayerMask == (waterLayerMask | (1 << other.gameObject.layer)))
        {
            _forcesMultiplier = 1f;
            _rigidbody.drag = _underwaterDrag;
            _rigidbody.angularDrag = _underwaterAngularDrag;
        }
    }
}
