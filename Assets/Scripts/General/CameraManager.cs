using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera _cameraController;
    public Transform _buildingModeCameraOrigin;
    public Transform _transitionModeCameraOrigin;
    public Transform _launchModeCameraOrigin;
    public Transform _menuModeCameraOrigin;
    public Transform _originalAxisOrigin;
    public Transform _rotationAxis;
    public Transform _plane;
    public float _camRotateSpeed;
    void Start()
    {
        GameStateManager.instance.gameStateChanged += GameStateChanged;
    }

    void Update()
    {
        //_rotationAxis.transform.position = _plane.transform.position;
        if (GameStateManager.instance.currentGameState != GameStateManager.GameState.Building)
            return;
        float rotationDir = Input.GetAxis("Horizontal");
        if (rotationDir != 0)
        {
            _rotationAxis.Rotate(rotationDir * Vector3.up * _camRotateSpeed * Time.deltaTime);
        }
    }
    private void GameStateChanged(GameStateManager.GameState state)
    {
        print("menu game state changes to " + state);
        switch (state)
        {
            case GameStateManager.GameState.Menu:
                _cameraController.Follow = null;
                _rotationAxis.DOLocalMove(_originalAxisOrigin.localPosition, 1f);
                _cameraController.transform.DOLocalMove(_menuModeCameraOrigin.localPosition, 3f);
                break;
            case GameStateManager.GameState.Building:
                _cameraController.Follow = null;
                _rotationAxis.DOLocalMove(_originalAxisOrigin.localPosition, 1f);
                _rotationAxis.DOLocalRotate(Vector3.zero, 2f);
                _cameraController.transform.DOLocalMove(_buildingModeCameraOrigin.localPosition,3f);
                break;
            case GameStateManager.GameState.Transition:
                _cameraController.Follow = null;
                _rotationAxis.DOLocalRotate(Vector3.zero, 2f);
                _rotationAxis.DOLocalMove(_transitionModeCameraOrigin.localPosition, 1.5f).SetDelay(2f);
                _cameraController.transform.DOLocalMove(_launchModeCameraOrigin.localPosition, 3f);
                break;
            case GameStateManager.GameState.LaunchPrepearation:
                _cameraController.Follow = _plane;
                break;
            case GameStateManager.GameState.Flying:
                break;
            case GameStateManager.GameState.GameEnd:
                _cameraController.Follow = null;
                break;
        }
    }
}
