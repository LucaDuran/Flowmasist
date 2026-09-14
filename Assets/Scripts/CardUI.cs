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

    public void ShowCard(Card card, CardPlacementManager manager)
    {
        cardShowed = card;
        cardPlacementManager = manager;
        cardIconImage.sprite = cardShowed.CardIcon;
        cardMoveCostText.text = "MoveCost: " + cardShowed.MoveCost;
    }
    public void OnPointerClick(PointerEventData eventData) 
    {
        cardPlacementManager.OnCardSelected(cardShowed);
    }
}
