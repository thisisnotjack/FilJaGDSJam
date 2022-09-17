using UnityEngine;
using UnityEngine.UI;

public class BuildingUIController : MonoBehaviour
{
    [SerializeField] Button _launchButton;

    void Start()
    {
        _launchButton.onClick.AddListener(HandleLaunchButtonWasclicked);
    }
    
    private void HandleLaunchButtonWasclicked()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Transition);
    }
}
