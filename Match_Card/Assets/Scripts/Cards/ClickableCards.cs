using DG.Tweening;
using Nishit.Emums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableCards : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    CardValues cardValue;

    [SerializeField]
    bool CardFlipped = true;

    [SerializeField]
    RectTransform frontFace;

    [SerializeField]
    RectTransform backFace;

    [SerializeField]
    Image UniqueImage;

    private GameplayManager gameplayManager;

    public void Init(CardValues cardValue, GameplayManager gameplayManager, Sprite cardSprite)
    {
        this.cardValue = cardValue;
        this.gameplayManager = gameplayManager;

        // Get the sprite from UIContainer

        if (cardSprite != null)
        {
            UniqueImage.sprite = cardSprite;
        }
        else
        {
            Debug.LogWarning($"No sprite found for card value: {this.cardValue}");
        }

        // Set initial state of the card
        frontFace.gameObject.SetActive(false);
        backFace.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CardFlipped)
        {
            ResetCardToDefault();
        }
        else
        {
            FlipCard();
        }
    }

    void FlipCard()
    {
        // Disable front, enable back after scaleX = 0
        Sequence flip = DOTween.Sequence();

        flip.Append(transform.DOScaleX(0f, 0.15f))
            .AppendCallback(() =>
            {
                frontFace.gameObject.SetActive(true);
                backFace.gameObject.SetActive(false);
            })
            .Append(transform.DOScaleX(1f, 0.15f))
            .OnComplete(() =>
            {
                CardFlipped = true;
            });
    }

    public void ResetCardToDefault()
    {
        Sequence flip = DOTween.Sequence();

        flip.Append(transform.DOScaleX(0f, 0.15f))
            .AppendCallback(() =>
            {
                frontFace.gameObject.SetActive(false);
                backFace.gameObject.SetActive(true);
            })
            .Append(transform.DOScaleX(1f, 0.15f))
            .OnComplete(() =>
            {
                CardFlipped = false;
            });
    }
}
