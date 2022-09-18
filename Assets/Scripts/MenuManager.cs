using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Transform _cameraFollow;
    public Transform _buildingModeCameraOrigin;
    public Transform _menuModeCameraOrigin;

    public Button _playBtn;
    public Button _instructionsBtn;
    public Button _optionsBtn;
    public Button _quitBtn;
    void Start()
    {
        GameStateManager.instance.gameStateChanged += GameStateChanged;
        _playBtn.onClick.AddListener(() =>
        {
            GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Building);
        }); 
        _instructionsBtn.onClick.AddListener(() =>
        {
            print("No, this obviously doesn't work Filip, don't be ridiculous");
        });
        _optionsBtn.onClick.AddListener(() =>
        {
            print("No, this obviously doesn't work Filip, don't be ridiculous");
        });
        _quitBtn.onClick.AddListener(() =>
        {
           Application.Quit();
        });
    }

    private void GameStateChanged(GameStateManager.GameState state)
    {
        switch (state)
        {
            case GameStateManager.GameState.Menu:
                _cameraFollow.DOLocalMove(_menuModeCameraOrigin.transform.localPosition, 3f);
                break;
            case GameStateManager.GameState.Building:
                
                _cameraFollow.DOLocalMove(_buildingModeCameraOrigin.transform.localPosition,3f);
                break;
            case GameStateManager.GameState.Transition:
                break;
            case GameStateManager.GameState.LaunchPrepearation:
                break;
            case GameStateManager.GameState.Flying:
                break;
            case GameStateManager.GameState.GameEnd:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
