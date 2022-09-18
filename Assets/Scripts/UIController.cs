using DG.Tweening;
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
                DisableAllUI(_menuUI);
                _menuUI.SetActive(true);
                _menuUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                break;
            case GameStateManager.GameState.Building:
                DisableAllUI(_buildingUI);
                _buildingUI.SetActive(true);
                _buildingUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                break;
            case GameStateManager.GameState.LaunchPrepearation:
                DisableAllUI(_launchUI);
                _launchUI.SetActive(true);
                _launchUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                break;
            case GameStateManager.GameState.Flying:
                DisableAllUI(_flyingUI);
                _flyingUI.SetActive(true);
                _flyingUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                break;
            case GameStateManager.GameState.Transition:
                DisableAllUI(null);
                break;
            case GameStateManager.GameState.GameEnd:
                DisableAllUI(_gameEndUI, _flyingUI);
                _gameEndUI.SetActive(true);
                _gameEndUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                if (!_flyingUI.activeSelf)
                {
                    _flyingUI.SetActive(true);
                    _flyingUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                }

                break;
        }
    }

    private void DisableAllUI(GameObject except, GameObject except2 = null)
    {
        if (_menuUI.activeSelf && _menuUI != except)
        {
            _menuUI.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(()=>_menuUI.SetActive(false));
        }
        if (_buildingUI.activeSelf && _buildingUI != except)
        {
            _buildingUI.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(()=>_buildingUI.SetActive(false));
        }
        if (_launchUI.activeSelf && _launchUI != except)
        {
            _launchUI.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(()=>_launchUI.SetActive(false));
        }
        if (_flyingUI.activeSelf && _flyingUI != except && _flyingUI != except2)
        {
            _flyingUI.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(()=>_flyingUI.SetActive(false));
        }
        if (_gameEndUI.activeSelf && _gameEndUI != except)
        {
            _gameEndUI.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(()=>_gameEndUI.SetActive(false));
        }
    }
}
