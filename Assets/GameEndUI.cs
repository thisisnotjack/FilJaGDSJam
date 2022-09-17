using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] Button _backToBuildingButton;
    [SerializeField] Button _backToLaunch;

    protected void Start()
    {
        _backToBuildingButton.onClick.AddListener(HandleBackToBuildingButtonPressed);
        _backToLaunch.onClick.AddListener(HandleBackToLaunchButtonPressed);
    }

    private void HandleBackToBuildingButtonPressed()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Building);
    }

    private void HandleBackToLaunchButtonPressed()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Launch);
    }

}
