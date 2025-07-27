using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioSource ComboSoundSource;

    [SerializeField]
    AudioClip cardFlipSound;

    [SerializeField]
    AudioClip ComboStreakSound;

    [SerializeField]
    AudioClip cardMatchSound;

    [SerializeField]
    AudioClip gameOverSound;

    [SerializeField]
    AudioClip WrongMatchSound;

    void Start()
    {
        GameplayManager.Instance.OnScoreChanged += PlayCardMatchSound;
        GameplayManager.Instance.OnAttemptChanged += PlayWrongMatchSound;
        GameplayManager.Instance.GameOver += PlayGameOverSound;
        GameplayManager.Instance.StreakChanged += PlayCardComboSound;
        GameplayManager.Instance.OnCardFlipped += PlayCardFlipSound;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OnScoreChanged -= PlayCardMatchSound;
        GameplayManager.Instance.OnAttemptChanged -= PlayWrongMatchSound;
        GameplayManager.Instance.GameOver -= PlayGameOverSound;
        GameplayManager.Instance.StreakChanged -= PlayCardComboSound;
        GameplayManager.Instance.OnCardFlipped -= PlayCardFlipSound;
    }

    void PlayCardFlipSound()
    {
        audioSource.PlayOneShot(cardFlipSound);
    }

    void PlayCardMatchSound(int score)
    {
        audioSource.PlayOneShot(cardMatchSound);
    }

    void PlayWrongMatchSound(int attempts)
    {
        audioSource.PlayOneShot(WrongMatchSound);
    }

    void PlayGameOverSound(bool isGameOver)
    {
        audioSource.PlayOneShot(gameOverSound);
    }

    void PlayCardComboSound(int streakCount)
    {
        float normalizedStreak = Mathf.Clamp01((streakCount - 1) / 5f); // 5 = streak at which volume reaches 1

        float volume = Mathf.Lerp(0.5f, 1f, normalizedStreak);

        ComboSoundSource.volume = volume;
        ComboSoundSource.PlayOneShot(ComboStreakSound);
    }

    void StopAllSounds()
    {
        audioSource.Stop();
    }
}
