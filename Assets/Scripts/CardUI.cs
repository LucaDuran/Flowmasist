using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI cardMoveCostText;
    [SerializeField] private Image cardIconImage;
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
        cardMoveCostText.text = "MoveCost: " + cardShowed.MoveCost;
    }
    public void SetSelected(bool selected)
    {
        cardIconImage.color = selected ? Color.red : Color.white;
    }
    public void OnPointerClick(PointerEventData eventData) 
    {
        if (discardController.IsDiscarding)
        {
            discardController.ToggleCard(this);
        }
        else
        {
            cardPlacementManager.OnCardSelected(cardShowed);
        }
    }
}
