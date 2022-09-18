using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] Button _backToBuildingButton;
    [SerializeField] Button _backToLaunch;
    [SerializeField] FlyingManager _flyingManager;
    [SerializeField] private TextMeshProUGUI _bestDistText;
    [SerializeField] private TextMeshProUGUI _bestHeightText;
    [SerializeField] private ItemUnlockingManager _itemUnlockingManager;
    [SerializeField] private TextMeshProUGUI _nextItemDistText;
    [SerializeField] private TextMeshProUGUI _nextItemHeightText;
    [SerializeField] private Transform _distanceItemCameraParent;
    [SerializeField] private Transform _heightItemCameraParent;
    [SerializeField] private Transform _distItemUItransformParent;
    [SerializeField] private Transform _heightItemUItransformParent;

    [SerializeField] private Transform _unlockDistItemCameraParent;
    [SerializeField] private Transform _unlockHeightCameraParent;
    [SerializeField] private Transform _unlockDistUItransformParent;
    [SerializeField] private Transform _unlockHeightUItransformParent;
    protected void Start()
    {
        if (_flyingManager == null)
            _flyingManager = FindObjectOfType<FlyingManager>();
        _backToBuildingButton.onClick.AddListener(HandleBackToBuildingButtonPressed);
        _backToLaunch.onClick.AddListener(HandleBackToLaunchButtonPressed);
        GameStateManager.instance.gameStateChanged += OnGameStateChanged;
        if (GameStateManager.instance.currentGameState == GameStateManager.GameState.GameEnd)
            PopulateUI();
    }

    private void OnGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.GameEnd)
        {
            PopulateUI();
        }
    }

    private void PopulateUI()
    {
        print("Populate UI");
        var bestDist = _flyingManager.bestDistanceInRun.ToString("F1") ;
        var bestHeight = _flyingManager.bestHeightInRun.ToString("F1") ;
        _bestDistText.text = bestDist + "M";
        _bestHeightText.text = bestHeight + "M";
        _itemUnlockingManager.RefreshUnlocks();
        var nextHeightItemInfo = _itemUnlockingManager.GetNextHeightItem();
        PopulateItemPreview(nextHeightItemInfo.itemPrefab,_heightItemUItransformParent,_heightItemCameraParent, _nextItemHeightText, nextHeightItemInfo.metersNeeded); 
        var nextDistItemInfo =_itemUnlockingManager.GetNextDistanceItem();
        PopulateItemPreview(nextDistItemInfo.itemPrefab,_distItemUItransformParent,_distanceItemCameraParent, _nextItemDistText,nextDistItemInfo.metersNeeded);
        
        var unlockedHeightItem = _itemUnlockingManager.GetUnlockedHeightItem();
        PopulateItemPreview(unlockedHeightItem,_unlockDistUItransformParent,_unlockDistItemCameraParent); 
        var unlockedDistItem = _itemUnlockingManager.GetUnlockedDistItem();
        PopulateItemPreview(unlockedDistItem,_unlockHeightUItransformParent,_unlockHeightCameraParent); 
    }
    void PopulateItemPreview(GameObject prefab, Transform uiParent, Transform camParent, TextMeshProUGUI metersText = null, float metersNeeded =0f)
    {
        if (prefab == null)
        {
            uiParent.gameObject.SetActive(false);
        }
        else
        {
            uiParent.gameObject.SetActive(true);
            if(metersText != null)
                metersText.text = metersNeeded + "M"; 
            for (int i = camParent.childCount - 1; i > 0; i--)
            {
                Destroy(camParent.GetChild(i).gameObject);
            }
            GameObject g = Instantiate(prefab, camParent);
            Transform[] transforms = g.GetComponentsInChildren<Transform>(true); 
            foreach (Transform t in transforms)
            {
                t.gameObject.layer =  camParent.gameObject.layer;
            }
            g.transform.localPosition = Vector3.zero;
            g.transform.localEulerAngles = Vector3.zero;
            g.transform.localScale = Vector3.one;
        }
    }

    private void HandleBackToBuildingButtonPressed()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.Building);
    }

    private void HandleBackToLaunchButtonPressed()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.LaunchPrepearation);
    }

}
