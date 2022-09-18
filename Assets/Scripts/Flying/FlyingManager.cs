using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingManager : Singleton<FlyingManager>
{
    [SerializeField] Transform _platformPlanePositionTransform;
    public Transform platformPlanePositionTransform => _platformPlanePositionTransform;

    private float _bestHeight = 0;
    private float _bestDistance = 0;
    private float _bestHeightInRun = 0;
    private float _bestDistanceInRun = 0;

    public float bestHeight => _bestHeight;
    public float bestHeightInRun => _bestHeightInRun;
    public float bestDistance => _bestDistance;
    public float bestDistanceInRun => _bestDistanceInRun;

    protected void Start()
    {
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {
        if(gameState == GameStateManager.GameState.LaunchPrepearation)
        {
            _bestHeightInRun = 0f;
            _bestDistanceInRun = 0f;
            //Set plane position to ballista position
            PlaneManager.instance.MovePlaneToTransform(_platformPlanePositionTransform);
            GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Flying);
        }
        if(gameState == GameStateManager.GameState.Flying)
        {
            StartFlying();
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
        if(currentDistance > _bestDistanceInRun)
        {
            _bestDistanceInRun = currentDistance;
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
        if(currentHeight > _bestHeightInRun)
        {
            _bestHeightInRun = currentHeight;
        }
        return currentHeight;
    }
    
    public void StartFlying()
    {
        PlaneManager.instance.planeObject.EnablePlaneCollider();
        PlaneManager.instance.planeObject.StartPush();
    }
}
