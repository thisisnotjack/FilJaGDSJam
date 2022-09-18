using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource _musicSource;
    [SerializeField] float _menuVolume = 1.0f;
    [SerializeField] float _inGameVolume = 0.5f;
    private float _musicFadeDuration = 0.5f;
    private float _musicFadeStart;

    protected void Start()
    {
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {

        switch (gameState)
        {
            case GameStateManager.GameState.Menu:
                FadeMusicVolume(_menuVolume);
                break;
            case GameStateManager.GameState.Building:
                FadeMusicVolume(_menuVolume);
                break;
            default:
                FadeMusicVolume(_inGameVolume);
                break;
        }
    }

    private void FadeMusicVolume(float targetVolume)
    {
        StopAllCoroutines();
        _musicFadeStart = Time.time;
        StartCoroutine(FadeMusic(targetVolume, _musicFadeDuration));
    }

    private IEnumerator FadeMusic(float targetVolume, float duration)
    {
        float alpha = 0f;
        float startingVolume = _musicSource.volume;
        while(alpha < 1)
        {
            alpha = (Time.time - _musicFadeStart) / duration;
            _musicSource.volume = (1 - alpha) * startingVolume + alpha * targetVolume;
            yield return null;
        }
    }
}
