using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public enum GameState
    {
        Building,
        Transition,
        Launch,
        Flying,
        GameEnd
    }
    private GameState _currentGameState;
    public GameState currentGameState => _currentGameState;

    public event System.Action<GameState> gameStateChanged;

    public void ChangeGameState(GameState newGameState)
    {
        _currentGameState = newGameState;
        print("Game state changes to " + newGameState);
        gameStateChanged?.Invoke(newGameState);
    }
}
