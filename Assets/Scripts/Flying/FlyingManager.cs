using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingManager : Singleton<FlyingManager>
{
    [SerializeField] Transform _platformPlanePositionTransform;
    [Space]
    [SerializeField] AudioClip _windStartSound;
    [SerializeField] AudioClip _windMiddleSound;
    [SerializeField] AudioClip _windEndSound;

    [Space]
    [SerializeField] AudioSource _ballistaFiringAudioSource;

    public Transform platformPlanePositionTransform => _platformPlanePositionTransform;

    private float _bestHeight = 0;
    private float _bestDistance = 0;

    public float bestHeight => _bestHeight;
    public float bestDistance => _bestDistance;

    protected void Start()
    {
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {
        if(gameState == GameStateManager.GameState.LaunchPrepearation)
        {
            //Set plane position to ballista position
            PlaneManager.instance.MovePlaneToTransform(_platformPlanePositionTransform);
            _ballistaFiringAudioSource.Play();
            GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Flying);
        }
        if(gameState == GameStateManager.GameState.Flying)
        {
            StartFlying();
        }
        if(gameState == GameStateManager.GameState.GameEnd)
        {
            StopWindSound();
        }
    }

    public float GetCurrentDistance()
    {
        var flattenedDifference = (PlaneManager.instance.planeObject.transform.position - _platformPlanePositionTransform.position);
        flattenedDifference.y = 0;
        var currentDistance = flattenedDifference.magnitude;
        if(currentDistance > _bestDistance)
        {
            _bestDistance = currentDistance;
        }
        return currentDistance;
    }

    public float GetCurrentHeight()
    {
        var currentHeight = PlaneManager.instance.planeObject.transform.position.y;
        if(currentHeight > _bestHeight)
        {
            _bestHeight = currentHeight;
        }
        return currentHeight;
    }
    
    public void StartFlying()
    {
        PlaneManager.instance.planeObject.EnablePlaneCollider();
        PlaneManager.instance.planeObject.StartPush();
        StartWindSound();
    }

    private void StopWindSound()
    {
        var audioSource = PlaneManager.instance.planeObject.baseSoundAudioSource;
        StopAllCoroutines();
        audioSource.Stop();
    }

    private void StartWindSound()
    {
        StopAllCoroutines();
        StartCoroutine(WindSoundCoroutine());
    }

    private IEnumerator WindSoundCoroutine()
    {
        var audioSource = PlaneManager.instance.planeObject.baseSoundAudioSource;
        audioSource.clip = _windStartSound;
        audioSource.loop = false;
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.clip = _windMiddleSound;
        audioSource.loop = true;
        audioSource.Play();
    }
}
