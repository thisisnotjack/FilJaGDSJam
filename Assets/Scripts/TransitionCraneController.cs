using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCraneController : MonoBehaviour
{

    [SerializeField] float _firstPhaseDuration;
    [SerializeField] float _secondPhaseDuration;
    [SerializeField] float _waitingDuration;
    [SerializeField] float _rotationDuration = 0.3f;
    [Space]
    [SerializeField] AudioSource _craneAudioSource;
    [SerializeField] AudioClip _plopSound;
    [SerializeField] AudioClip _movementSound;

    public event System.Action transitionDidFinish;

    private Vector3 _currentMovementTarget;
    private Vector3 _startPosition;
    private FollowComponent _planeFollowComponent;

    protected void Start()
    {
        _startPosition = transform.position;
    }

    public void StartCraneMovement()
    {
        gameObject.SetActive(true);
        StartCoroutine(ApproachCoroutine());
    }

    public IEnumerator ApproachCoroutine()
    {
        PlayCraneMovementSound();
        _currentMovementTarget = PlaneManager.instance.planeObject.craneAttachPoint.position;
        yield return MoveTowardsGoal(_firstPhaseDuration);
        StickPlane();
        Invoke("PlayPlopSound", 0.12f);
        yield return new WaitForSeconds(_waitingDuration);
        PlayCraneMovementSound();
        _currentMovementTarget = FlyingManager.instance.platformPlanePositionTransform.position;
        yield return MoveTowardsGoal(_secondPhaseDuration);
        yield return RotatePlane();
        Invoke("PlayPlopSound", 0.2f);
        UnstickPlane();
        yield return new WaitForSeconds(_waitingDuration);
        yield return DissapearCoroutine();

    }

    public IEnumerator RotatePlane()
    {
        var planeTransform = PlaneManager.instance.planeObject.transform;
        Quaternion startRotation = planeTransform.rotation;
        Quaternion targetRotation = FlyingManager.instance.platformPlanePositionTransform.rotation;
        var alpha = 0.0f;
        var movementStartTime = Time.time;
        while (alpha < 1f)
        {
            alpha = (Time.time - movementStartTime) / _rotationDuration;
            planeTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, alpha);
            yield return null;
        }
    }

    public IEnumerator MoveTowardsGoal(float duration)
    {
        Vector3 startPosition = transform.position;
        var movementStartTime = Time.time;
        var difference = _currentMovementTarget - startPosition;
        float totalDistance = Mathf.Abs(difference.x) + Mathf.Abs(difference.y) + Mathf.Abs(difference.z);
        //First move on the y axis
        float alpha = 0;
        float currentDuration = duration * (Mathf.Abs(difference.y) / totalDistance);
        Vector3 currentTarget = startPosition;
        currentTarget.y = _currentMovementTarget.y;
        if(difference.y != 0)
        {
            while (alpha < 1f)
            {
                alpha = (Time.time - movementStartTime) / currentDuration;
                transform.position = Vector3.Lerp(startPosition, currentTarget, alpha);
                yield return null;
            }
        }
        //Second move on the x axis
        if (difference.x != 0)
        {
            alpha = 0;
            currentDuration = duration * (Mathf.Abs(difference.x) / totalDistance);
            currentTarget.x = _currentMovementTarget.x;
            movementStartTime = Time.time;
            startPosition = transform.position;
            while (alpha < 1f)
            {
                alpha = (Time.time - movementStartTime) / currentDuration;
                transform.position = Vector3.Lerp(startPosition, currentTarget, alpha);
                yield return null;
            }
        }

        if(difference.z != 0)
        {
            //Last move on the z axis
            alpha = 0;
            currentDuration = duration * (Mathf.Abs(difference.z) / totalDistance);
            currentTarget.z = _currentMovementTarget.z;
            movementStartTime = Time.time;
            startPosition = transform.position;
            while (alpha < 1f)
            {
                alpha = (Time.time - movementStartTime) / currentDuration;
                transform.position = Vector3.Lerp(startPosition, currentTarget, alpha);
                yield return null;
            }
        }
    }

    public IEnumerator DissapearCoroutine()
    {
        _currentMovementTarget = transform.position;
        _currentMovementTarget.y += 50f;
        yield return MoveTowardsGoal(_secondPhaseDuration);
        transform.position = _startPosition;
        transitionDidFinish?.Invoke();
        gameObject.SetActive(false);
    }

    private void StickPlane()
    {
        var planeObject = PlaneManager.instance.planeObject;
        var offset = planeObject.transform.position - transform.position;
        _planeFollowComponent = planeObject.gameObject.AddComponent<FollowComponent>();
        planeObject.DisableMovement();
        planeObject.DisableColliders();
        _planeFollowComponent.FollowTransform(transform, offset);
    }

    private void PlayPlopSound()
    {
        //Play sticking sound
        _craneAudioSource.clip = _plopSound;
        _craneAudioSource.Play();
    }

    private void PlayCraneMovementSound()
    {
        _craneAudioSource.clip = _movementSound;
        _craneAudioSource.Play();
    }


    private void UnstickPlane() 
    {
        Destroy(_planeFollowComponent);
        PlaneManager.instance.planeObject.EnableMovement();
    }
}
