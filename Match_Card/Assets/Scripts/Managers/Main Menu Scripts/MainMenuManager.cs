using Nishit.Class;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Difficulty Settings")]
    [SerializeField]
    Slider DifficultySlider;

    [SerializeField]
    TextMeshProUGUI SliderText;

    [Space(10)]
    [Header("Play Button")]
    [SerializeField]
    Button StartGameButton;

    [Space(10)]
    [Header("Play Button")]
    [SerializeField]
    Button SoundButton;

    [SerializeField]
    Sprite MuteSprite;

    [SerializeField]
    Sprite UnmuteSprite;

    [Space(10)]
    [Header("Stored Data Display")]
    [SerializeField]
    TextMeshProUGUI HighestComboStreakText;

    [SerializeField]
    TextMeshProUGUI HighestScoreText;

    int difficultyLevel = 2;

    void Start()
    {
        AssignButtons();
        LoadDataScores();
    }

    void AssignButtons()
    {
        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", difficultyLevel);
        DifficultySlider.value = difficultyLevel;
        DifficultySlider.onValueChanged.AddListener(OnDifficultySliderChanged);
        OnDifficultySliderChanged(difficultyLevel);

        StartGameButton.onClick.AddListener(OnStartGameButtonClicked);

        SoundButton.onClick.AddListener(ToggleSound);
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        ChangeVolumeSprite(isMuted);
    }

    void LoadDataScores()
    {
        var StringBuffer = StringBuilderPool.Get();

        if (HighestComboStreakText)
        {
            StringBuffer.Append("Highest Combo : ");
            StringBuffer.Append(PlayerPrefs.GetInt("HighestComboStreak", 0));
            HighestComboStreakText.text = StringBuilderPool.Release();
        }
        if (HighestScoreText)
        {
            StringBuffer.Clear();
            StringBuffer.Append("Highest Score : ");
            StringBuffer.Append(PlayerPrefs.GetInt("HighestScore", 0));
            HighestScoreText.text = StringBuilderPool.Release();
        }
    }

    void OnDisable()
    {
        DifficultySlider.onValueChanged.RemoveAllListeners();
        StartGameButton.onClick.RemoveAllListeners();
        SoundButton.onClick.RemoveAllListeners();
    }

    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("Game_Scene");
    }

    public void OnDifficultySliderChanged(float value = 0f)
    {
        PlayerPrefs.SetInt("DifficultyLevel", (int)value);
        SliderText.text = $"Pairs : {value}";
    }

    public void ToggleSound()
    {
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        isMuted = !isMuted;
        ChangeVolumeSprite(isMuted);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }

    void ChangeVolumeSprite(bool isMuted)
    {
        SoundButton.image.sprite = isMuted ? MuteSprite : UnmuteSprite;
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
