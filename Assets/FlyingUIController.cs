using UnityEngine;
using TMPro;

public class FlyingUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentDistanceText;
    [SerializeField] TextMeshProUGUI _currentBestDistanceText;
    [SerializeField] TextMeshProUGUI _currentHeightText;
    [SerializeField] TextMeshProUGUI _currentBestHeightText;

    void Update()
    {
        _currentHeightText.text = FlyingManager.instance.GetCurrentHeight().ToString("F1") + "m";
        _currentDistanceText.text = FlyingManager.instance.GetCurrentDistance().ToString("F1") + "m";
        _currentBestDistanceText.text = FlyingManager.instance.bestDistance.ToString("F1") + "m";
        _currentBestHeightText.text = FlyingManager.instance.bestHeight.ToString("F1") + "m";
    }
}
