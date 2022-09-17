using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingManager : Singleton<FlyingManager>
{
    [SerializeField] Transform _platformPlanePositionTransform;
    public Transform platformPlanePositionTransform => _platformPlanePositionTransform;

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
            GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Flying);
        }
        if(gameState == GameStateManager.GameState.Flying)
        {
            StartFlying();
        }
    }

    public void StartFlying()
    {
        PlaneManager.instance.planeObject.EnablePlaneCollider();
        PlaneManager.instance.planeObject.StartPush();
    }
}
