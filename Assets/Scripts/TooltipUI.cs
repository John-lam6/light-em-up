using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Settings")]
    [SerializeField] private string tooltipMessage = "Your tooltip text here";
    [SerializeField] private Vector2 offset = new Vector2(15f, -15f);

    [Header("Tooltip References")]
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private TextMeshProUGUI tooltipText;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 originalScale;

    private Canvas canvas;
    private Tween scaleTween;
    private bool isHovering;
    private Button button;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        originalScale = transform.localScale;
        button = GetComponent<Button>();
        Hide();
    }

    private void Update()
    {
        if (!isHovering) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out pos
        );
        tooltipRect.anchoredPosition = pos + offset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;
        if (button != null && !button.interactable) return;

        if (scaleTween != null && scaleTween.IsActive())
            scaleTween.Kill();

        scaleTween = transform.DOScale(originalScale * 1.1f, 0.15f).SetUpdate(true);

        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        if (scaleTween != null && scaleTween.IsActive())
            scaleTween.Kill();

        scaleTween = transform.DOScale(originalScale, 0.15f).SetUpdate(true);

        Hide();
    }

    private void Show()
    {
        tooltipText.text = tooltipMessage;
        tooltipRect.gameObject.SetActive(true);
        tooltipRect.SetAsLastSibling();
        isHovering = true;
    }

    private void Hide()
    {
        tooltipRect.gameObject.SetActive(false);
        isHovering = false;
    }

    private void OnDestroy()
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
            scaleTween = null;
        }
    }
}