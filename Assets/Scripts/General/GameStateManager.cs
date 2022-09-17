using System.Collections;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public enum GameState
    {
        Building,
        Transition,
        LaunchPrepearation,
        Flying,
        GameEnd
    }
    private GameState _currentGameState;
    public GameState currentGameState => _currentGameState;

    public event System.Action<GameState> gameStateChanged;

    protected void Start()
    {
        ChangeGameState(GameState.Building);
    }

    public void ChangeGameState(GameState newGameState)
    {
        StartCoroutine(ChangeStateAfterFrame(newGameState));
    }

    private void ChangeGameStateInternal(GameState newGameState)
    {
        _currentGameState = newGameState;
        print("Game state changes to " + newGameState);
        gameStateChanged?.Invoke(newGameState);
    }

    private IEnumerator ChangeStateAfterFrame(GameState state)
    {
        yield return new WaitForEndOfFrame();
        ChangeGameStateInternal(state);
    }

}
