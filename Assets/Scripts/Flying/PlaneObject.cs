using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    [SerializeField] PlanePhysicsFlightController _planePhysicsFlightController;
    [SerializeField] PlaneStartPushController _planeStartPushController;
    [SerializeField] float _startPushDuration;
    [SerializeField] Transform _craneAttachPoint;
    [SerializeField] Rigidbody _rigidbody;
   
    public Transform craneAttachPoint => _craneAttachPoint;
    public event System.Action hitWater;

    private bool _startingPushInProgress = false;
    private bool _flightInProgress = false;
    private float _pushStartTime;
    private bool _underwater = false;

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
        if (!_underwater && waterLayerMask == (waterLayerMask | (1 << other.gameObject.layer)))
        {
            _underwater = true;
            _planePhysicsFlightController.SetWaterMultipliers();
            hitWater?.Invoke();
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

    public void EnableCollider()
    {
        var collider = GetComponent<Collider>();
        collider.enabled = true;
    }
}
