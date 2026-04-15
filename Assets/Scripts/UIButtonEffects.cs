using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Tween scaleTween;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        if (scaleTween != null && scaleTween.IsActive())
            scaleTween.Kill();

        scaleTween = transform.DOScale(originalScale * 1.1f, 0.15f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        if (scaleTween != null && scaleTween.IsActive())
            scaleTween.Kill();

        scaleTween = transform.DOScale(originalScale, 0.15f).SetUpdate(true);
    }

    void OnDisable()
    {
        if (scaleTween != null && scaleTween.IsActive())
            scaleTween.Kill();
    }

    void OnDestroy()
    {
        if (scaleTween != null && scaleTween.IsActive())
            scaleTween.Kill();
    }
}