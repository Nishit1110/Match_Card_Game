using System.Collections;
using System.Collections.Generic;
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

    int difficultyLevel = 2;

    void Start()
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
}
