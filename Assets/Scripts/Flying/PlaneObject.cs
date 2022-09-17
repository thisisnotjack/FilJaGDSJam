using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    [SerializeField] PlanePhysicsFlightController _planePhysicsFlightController;
    [SerializeField] PlaneStartPushController _planeStartPushController;
    [SerializeField] float _startPushDuration;

    private bool _startingPushInProgress = false;
    private float _pushStartTime;

    protected void Start()
    {
        StartPush();
    }

    protected void Update()
    {
        if (_startingPushInProgress)
        {
            _planeStartPushController.ProcessFlight();
            if ((Time.time - _pushStartTime) >= _startPushDuration)
            {
                _startingPushInProgress = false;
                Debug.Log("Initial push ended");
            }
        }
        else
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

}
