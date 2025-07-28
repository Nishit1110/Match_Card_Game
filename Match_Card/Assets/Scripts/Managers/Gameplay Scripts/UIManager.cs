using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Nishit.Class;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ScoreText;

    [SerializeField]
    TextMeshProUGUI TimerText;

    [SerializeField]
    TextMeshProUGUI AttemptsText;

    [SerializeField]
    TextMeshProUGUI GameOverInformationText;

    [SerializeField]
    TextMeshProUGUI ComboStreakText;

    [SerializeField]
    GameObject GameOverPanel;

    [SerializeField]
    Button PauseButton;

    [SerializeField]
    Button ResumeButton;

    [SerializeField]
    Button HomeButtonPauseMenu;

    [SerializeField]
    Button HomeButtonGameOverMenu;

    [SerializeField]
    GameObject PausePanel;

    int score = 0;
    int attempts = 0;
    float timer = 0f;
    int highestComboStreak = 0;

    private void Start()
    {
        GameplayManager.Instance.OnScoreChanged += UpdateScore;
        GameplayManager.Instance.OnAttemptChanged += UpdateAttempts;
        GameplayManager.Instance.OnTimeChanged += UpdateTimer;
        GameplayManager.Instance.GameOver += ShowGameOverPanel;
        GameplayManager.Instance.StreakChanged += ShowComboStreakNumber;

        HomeButtonPauseMenu.onClick.AddListener(BackToHome);
        HomeButtonGameOverMenu.onClick.AddListener(BackToHome);
        PauseButton.onClick.AddListener(PauseGame);
        ResumeButton.onClick.AddListener(ResumeGame);

        UpdateScore(0);
        UpdateAttempts(0);
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OnScoreChanged -= UpdateScore;
        GameplayManager.Instance.OnAttemptChanged -= UpdateAttempts;
        GameplayManager.Instance.OnTimeChanged -= UpdateTimer;
        GameplayManager.Instance.GameOver -= ShowGameOverPanel;
        GameplayManager.Instance.StreakChanged -= ShowComboStreakNumber;

        HomeButtonPauseMenu.onClick.RemoveListener(BackToHome);
        HomeButtonGameOverMenu.onClick.RemoveListener(BackToHome);
        PauseButton.onClick.RemoveListener(PauseGame);
        ResumeButton.onClick.RemoveListener(ResumeGame);
    }

    private void UpdateScore(int score)
    {
        this.score += score;
        if (this.score < 0)
            this.score = 0;

        var sb = StringBuilderPool.Get();
        sb.Append("Score : ").Append(this.score);
        ScoreText.text = StringBuilderPool.Release();
    }

    private void UpdateAttempts(int attempts)
    {
        this.attempts += attempts;
        if (this.attempts < 0)
            this.attempts = 0;

        var sb = StringBuilderPool.Get();
        sb.Append("Attempts : ").Append(this.attempts);
        AttemptsText.text = StringBuilderPool.Release();
    }

    private void UpdateTimer(float time)
    {
        timer = time;
        if (timer < 0)
            timer = 0;

        var sb = StringBuilderPool.Get();
        sb.Append("Time : ").AppendFormat("{0:F2}", timer);
        TimerText.text = StringBuilderPool.Release();
    }

    void ShowGameOverPanel(bool isGameOver)
    {
        GameOverPanel.SetActive(isGameOver);
        if (isGameOver)
        {
            var sb = StringBuilderPool.Get();
            sb.Append("Game Over! Final Score: ")
                .Append(score)
                .Append("\nTotal Attempts: ")
                .Append(attempts)
                .Append("\nTime Taken: ")
                .AppendFormat("{0:F2}", timer)
                .Append(" seconds")
                .Append("\nHighest Combo Streak: ")
                .Append(highestComboStreak);
            GameOverInformationText.text = StringBuilderPool.Release();
        }
        else
        {
            GameOverInformationText.text = string.Empty;
        }
    }

    void ShowComboStreakNumber(int streak)
    {
        if (streak > highestComboStreak)
            highestComboStreak = streak;

        var sb = StringBuilderPool.Get();
        sb.Append("Combo x").Append(streak);
        ComboStreakText.text = StringBuilderPool.Release();

        ComboStreakText.transform.localScale = Vector3.zero;

        Color startColor = ComboStreakText.color;
        startColor.a = 0f;
        ComboStreakText.color = startColor;

        Sequence comboSequence = DOTween.Sequence();

        comboSequence
            .Append(
                ComboStreakText.DOColor(
                    new Color(startColor.r, startColor.g, startColor.b, 1f),
                    0.2f
                )
            )
            .Join(ComboStreakText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack))
            .AppendInterval(1f)
            .Append(
                ComboStreakText.DOColor(
                    new Color(startColor.r, startColor.g, startColor.b, 0f),
                    0.3f
                )
            )
            .Join(ComboStreakText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        GameplayManager.Instance.CanFlipCard = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        GameplayManager.Instance.CanFlipCard = true;
    }

    void BackToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu_Scene");
    }
}
