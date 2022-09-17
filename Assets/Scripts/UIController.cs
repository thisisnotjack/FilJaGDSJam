using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _menuUI;
    [SerializeField] GameObject _buildingUI;
    [SerializeField] GameObject _flyingUI;
    [SerializeField] GameObject _launchUI;
    [SerializeField] GameObject _gameEndUI;

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.Menu:
                DisableAllUI();
                _menuUI.SetActive(true);
                break;
            case GameStateManager.GameState.Building:
                DisableAllUI();
                _buildingUI.SetActive(true);
                break;
            case GameStateManager.GameState.LaunchPrepearation:
                DisableAllUI();
                _launchUI.SetActive(true);
                break;
            case GameStateManager.GameState.Flying:
                DisableAllUI();
                _flyingUI.SetActive(true);
                break;
            case GameStateManager.GameState.Transition:
                DisableAllUI();
                break;
            case GameStateManager.GameState.GameEnd:
                DisableAllUI();
                _gameEndUI.SetActive(true);
                break;
        }
    }

    private void DisableAllUI()
    {
        _buildingUI.SetActive(false);
        _launchUI.SetActive(false);
        _flyingUI.SetActive(false);
        _gameEndUI.SetActive(false);
    }
}
