using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAffector : MonoBehaviour
{
    [SerializeField] float _engineStrength;
    [SerializeField] Transform _engineThrustDirectionPoint;
    [SerializeField] Transform _engineBasePositionTransform;
    [SerializeField] Animator _engineAnimator;
    [SerializeField] ParticleSystem _engineParticleSystem;

    public float engineSrength => _engineStrength;
    public Vector3 engineThrustDirection => (_engineThrustDirectionPoint.position - transform.position).normalized;
    public Transform centerTransform => _engineBasePositionTransform;
    
    public void StartEngine()
    {
        _engineParticleSystem.Play();
    }

    public void StopEngine()
    {
        _engineParticleSystem.Stop();
    }
}
