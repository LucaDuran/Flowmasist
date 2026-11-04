using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI cardMoveCostText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeAmount = 12f;
    [SerializeField] private RectTransform visual;
    [SerializeField]private float hoverLift = 20f;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float hoverSpeed = 5f;
    [SerializeField] private float selectedLift = 45f;
    private bool isSelected;
    private float selectedAmount;
    private bool isHovered;
    private float hoverAmount;
    private float shakeTime;
    private Vector2 visualBase;
    private CardPlacementManager cardPlacementManager;
    private Card cardShowed;
    public Card Card => cardShowed;
    private DiscardController discardController;

    public void ShowCard(Card card, CardPlacementManager manager, DiscardController discard)
    {
        cardShowed = card;
        cardPlacementManager = manager;
        discardController = discard;
        cardIconImage.sprite = cardShowed.CardIcon;
        cardMoveCostText.text = "E: " + cardShowed.MoveCost;
    }
    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
    public void OnPointerClick(PointerEventData eventData) 
    {
        if (discardController.IsDiscarding)
        {
            discardController.ToggleCard(this);
        }
        else
        {
            cardPlacementManager.OnCardSelected(cardShowed, this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
    private void Awake()
    {
        visualBase = visual.anchoredPosition;
    }
    public void Shake()
    {
        shakeTime = shakeDuration;
    }
    private void Update()
    {
        float shakeOffset = 0f;
        if (shakeTime > 0f)
        {
            shakeTime -= Time.deltaTime;
            float progress = Mathf.Max(shakeTime, 0f) / shakeDuration;
            shakeOffset = Mathf.Sin(progress * 40f) * shakeAmount * progress;
        }
        float smoothing = 1f - Mathf.Exp(-hoverSpeed * Time.deltaTime);
        hoverAmount = Mathf.Lerp(hoverAmount, isHovered ? 1f : 0f, smoothing);
        selectedAmount = Mathf.Lerp(selectedAmount, isSelected ? 1f : 0f, smoothing);
        float lift = hoverLift * hoverAmount + selectedLift * selectedAmount;
        visual.anchoredPosition = visualBase + new Vector2(shakeOffset, lift);
        visual.localScale = Vector3.one * Mathf.Lerp(1f, hoverScale, Mathf.Max(hoverAmount, selectedAmount));
    }
}
