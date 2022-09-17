using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneStartPushController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Vector3 _startPushDirection;
    [SerializeField] float _startPushStrength = 10f;
    private float _forceMultiplier;


    public void ProcessFlight()
    {
        _rigidbody.AddForce(_startPushStrength * _startPushDirection);
    }
}
