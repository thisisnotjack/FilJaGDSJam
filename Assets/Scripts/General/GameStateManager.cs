using System.Collections;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public enum GameState
    {
        Menu,
        Building,
        Transition,
        LaunchPrepearation,
        Flying,
        GameEnd
    }

    public GameState _initialGameState = GameState.Menu;
    private GameState _currentGameState;
    protected IEnumerator Start()
    {
        yield return null;// just in case as this should be called after other objects subscribe to this
        ChangeGameState(_initialGameState); 
    }
    public GameState currentGameState => _currentGameState;

    public event System.Action<GameState> gameStateChanged;

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
