using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAffector : MonoBehaviour
{
    [SerializeField] float _engineStrength;
    [SerializeField] Vector3 _engineThrustDirection;
    [SerializeField] Transform _engineBasePositionTransform;
    public float engineSrength => _engineStrength;
    public Vector3 engineThrustDirection => _engineThrustDirection;
    public Transform centerTransform => _engineBasePositionTransform;
}
