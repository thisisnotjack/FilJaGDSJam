using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : Singleton<PlaneManager>
{
    [SerializeField] PlaneObject _planeObject;

    public PlaneObject planeObject => _planeObject;

    protected void Start()
    {
        _planeObject.hitWater += HandlePlaneHitWater;
        GameStateManager.instance.gameStateChanged += SetPlaneToState;
    }

    private void HandlePlaneHitWater()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameEnd);
    }

    public void SetPlaneToState(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.Building:
                _planeObject.EnableAllColliders();
                _planeObject.DisableMovement();
                _planeObject.ResetInternalState();
                break;
            case GameStateManager.GameState.LaunchPrepearation:
                _planeObject.DisableColliders();
                _planeObject.DisableMovement();
                _planeObject.ResetInternalState();
                break;
            case GameStateManager.GameState.Flying:
                _planeObject.EnablePlaneCollider();
                _planeObject.EnableMovement();
                break;
        }
    }

    public void MovePlaneToTransform(Transform targetTransform)
    {
        _planeObject.transform.position = targetTransform.position;
        _planeObject.transform.rotation = targetTransform.rotation;
    }
}
