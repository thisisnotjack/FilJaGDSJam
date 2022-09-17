﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] TransitionCraneController _transitionCraneController;

    protected void Start()
    {
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {
        if(gameState == GameStateManager.GameState.Transition)
        {
            StartTransition();
        }
    }

    public void StartTransition()
    {
        _transitionCraneController.StartCraneMovement();
        _transitionCraneController.transitionDidFinish += HandleCraneMovementFinished;
    }

    private void HandleCraneMovementFinished()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.LaunchPrepearation);
    }
}
