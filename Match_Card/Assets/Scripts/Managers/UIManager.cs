using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

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
    }

    private void UpdateScore(int score)
    {
        this.score += score;
        if (this.score < 0)
            this.score = 0; // Prevent negative score
        ScoreText.text = $"Score : {this.score}";
    }

    private void UpdateAttempts(int attempts)
    {
        this.attempts += attempts;
        if (attempts < 0)
            attempts = 0; // Prevent negative attempts
        AttemptsText.text = $"Attempts : {this.attempts}";
    }

    private void UpdateTimer(float time)
    {
        timer = time;
        if (timer < 0)
            timer = 0; // Prevent negative time
        TimerText.text = $"Time : {time:F2}";
    }

    void ShowGameOverPanel(bool isGameOver)
    {
        GameOverPanel.SetActive(isGameOver);
        if (isGameOver)
        {
            GameOverInformationText.text =
                $"Game Over! Final Score: {score}\nTotal Attempts: {attempts}\nTime Taken: {timer:F2} seconds\nHighest Combo Streak: {highestComboStreak}";
        }
        else
        {
            GameOverInformationText.text = string.Empty;
        }
    }

    void ShowComboStreakNumber(int streak)
    {
        if (streak > highestComboStreak)
        {
            highestComboStreak = streak;
        }
        ComboStreakText.text = $"Combo x{streak}";
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
}
