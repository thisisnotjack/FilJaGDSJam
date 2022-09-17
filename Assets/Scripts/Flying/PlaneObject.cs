using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    [SerializeField] PlanePhysicsFlightController _planePhysicsFlightController;
    [SerializeField] PlaneStartPushController _planeStartPushController;
    [SerializeField] float _startPushDuration;
    [SerializeField] Transform _craneAttachPoint;
    [SerializeField] Rigidbody _rigidbody;
    public Transform craneAttachPoint => _craneAttachPoint;

    private bool _startingPushInProgress = false;
    private bool _flightInProgress = false;
    private float _pushStartTime;

    protected void Update()
    {
        if (_startingPushInProgress)
        {
            _planeStartPushController.ProcessFlight();
            if ((Time.time - _pushStartTime) >= _startPushDuration)
            {
                _startingPushInProgress = false;
                _flightInProgress = true;
                Debug.Log("Initial push ended");
            }
        }
        else if(_flightInProgress)
        {
            _planePhysicsFlightController.ProcessFlight();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        var waterLayerMask = LayerMask.GetMask("Water");
        if (waterLayerMask == (waterLayerMask | (1 << other.gameObject.layer)))
        {
            _planePhysicsFlightController.SetWaterMultipliers();
        }
    }

    public void StartPush()
    {
        _pushStartTime = Time.time;
        _startingPushInProgress = true;
    }

    public void DisableMovement()
    {
        //TODO
        _rigidbody.isKinematic = true;
    }

    public void DisableColliders()
    {
        foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void EnableMovement()
    {
        //TODO
        _rigidbody.isKinematic = false;
    }

    public void EnableColliders()
    {
        foreach (var collider in gameObject.GetComponentsInChildren<Collider>(includeInactive: true))
        {
            collider.enabled = true;
        }
    }
}
