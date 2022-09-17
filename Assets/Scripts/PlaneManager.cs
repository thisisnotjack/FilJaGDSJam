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
    }

    private void HandlePlaneHitWater()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameEnd);
    }

    public void MovePlanetoTransform(Transform targetTransform)
    {
        _planeObject.transform.position = targetTransform.position;
        _planeObject.transform.rotation = targetTransform.rotation;
    }
}
