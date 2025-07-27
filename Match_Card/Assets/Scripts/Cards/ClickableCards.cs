using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableCards : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    bool CardFlipped = true;

    [SerializeField]
    RectTransform frontFace;

    [SerializeField]
    RectTransform backFace;

    [SerializeField]
    Image UniqueImage;

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
