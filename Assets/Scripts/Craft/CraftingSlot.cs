using CardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    public List<GameObject> cardObjectsInSlot = new List<GameObject>();
    public bool isInCooldown = false;
    public GameObject cooldownObject;
    public Image cooldownBar;
    public float currentCoolDown = 0f;
    private float totalCooldownTime = 0f; // Durée totale du cooldown

    private void Update()
    {
        if (isInCooldown)
        {
            currentCoolDown -= Time.deltaTime;
            UpdateCooldownBar();
            if (currentCoolDown  <= 0)
            {
                isInCooldown = false;
                currentCoolDown = 0f;
                cooldownObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInCooldown) return;

        CardMovement cardMovement = collision.GetComponent<CardMovement>();
        CardDisplay cardDisplay = collision.GetComponent<CardDisplay>();

        if (cardMovement != null && cardDisplay != null && !cardMovement.isDragging && !cardMovement.hasBeenCounted)
        {
            bool humanCardPresent = cardObjectsInSlot.Exists(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.Human);


            if ( cardDisplay.cardData.cardType != CardType.Ennemy && 
                cardDisplay.cardData.cardType != CardType.QuestCard &&
                cardDisplay.cardData.cardType != CardType.Idea &&
                cardDisplay.cardData.cardType != CardType.Food &&
                (!humanCardPresent || cardDisplay.cardData.cardType != CardType.Human)) 
            {

                Debug.Log("Card added to slot: " + cardDisplay.cardData.cardName);
                cardObjectsInSlot.Add(collision.gameObject);
                cardMovement.hasBeenCounted = true;
                

                if (cardDisplay.cardData.cardType == CardType.Human)
                {
                    cardMovement.hasBeenCounted = false;

                    // Déplacer la carte humaine hors de la vue
                    collision.transform.position = new Vector3(-1000, -1000, 0);
                }
                else
                {
                    collision.gameObject.SetActive(false);
                }
            }

        }
    }

    public void Craft() {
        if (cardObjectsInSlot != null)
        {
            CraftManager.Instance.Craft(this);
        }
        else return;
        
    }

    public void StartCooldown(float cooldown)
    {
        isInCooldown = true;
        totalCooldownTime = cooldown;  // On stocke la durée totale
        currentCoolDown = cooldown;     // On initialise le cooldown actuel    
        cooldownObject.SetActive(true);
        UpdateCooldownBar();
    }

    public void ClearSlot()
    {
        foreach(GameObject cardObject in cardObjectsInSlot)
        {
            Destroy(cardObject);
        }
        cardObjectsInSlot.Clear();
    }


    void UpdateCooldownBar()
    {
        if (cooldownBar != null && totalCooldownTime > 0)
        {
            // Remplissage en proportion de currentCoolDown par rapport à la durée totale
            cooldownBar.fillAmount = currentCoolDown / totalCooldownTime;
        }
    }
}
