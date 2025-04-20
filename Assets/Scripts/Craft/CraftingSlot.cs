using CardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CraftingSlot : MonoBehaviour
{
    public List<GameObject> cardObjectsInSlot = new List<GameObject>();

    public bool isInCooldown = false;
    private int countCardInslot = 0;
    public float currentCoolDown = 0f;
    private float totalCooldownTime = 0f; // Durée totale du cooldown
    
    public GameObject cooldownObject;
    public GameObject icon;
    public GameObject nbCardInSlotObject;

    public Image cooldownBar;
    public TMP_Text nbCardInSlotText;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isInCooldown)
        {
            currentCoolDown -= Time.deltaTime;
            UpdateCooldownBar();
            CraftManager.Instance.UpdateCooldwon(currentCoolDown);

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

        RemoveDestroyedObjects();

        CardMovement cardMovement = collision.GetComponent<CardMovement>();
        CardDisplay cardDisplay = collision.GetComponent<CardDisplay>();

        if (cardMovement != null && cardDisplay != null && !cardMovement.isDragging && !cardMovement.hasBeenCounted)
        {
            bool humanCardPresent = cardObjectsInSlot.Exists(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.Human);
            bool toolCardPresent = cardObjectsInSlot.Exists(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.ToolCrafted);

            if ( cardDisplay.cardData.cardType != CardType.Ennemy && 
                cardDisplay.cardData.cardType != CardType.QuestCard &&
                cardDisplay.cardData.cardType != CardType.Idea &&
                cardDisplay.cardData.cardType != CardType.Food &&
                cardDisplay.cardData.cardType != CardType.Coin &&
                (!humanCardPresent || cardDisplay.cardData.cardType != CardType.Human) &&
                (!toolCardPresent || cardDisplay.cardData.cardType != CardType.ToolCrafted)) 
            {

                // Sert a compter le nombre de carte dans le slot
                countCardInslot++;
                nbCardInSlotText.text = countCardInslot.ToString();
                icon.SetActive(false);
                nbCardInSlotObject.SetActive(true);

                Debug.Log("Card added to slot: " + cardDisplay.cardData.cardName);
                cardObjectsInSlot.Add(collision.gameObject);
                
                

                if (cardDisplay.cardData.cardType == CardType.Human)
                {
                    

                    // Déplacer la carte humaine hors de la vue
                    collision.transform.position = new Vector3(-1000, -1000, 0);
                    cardMovement.hasBeenCounted = false;
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
            // Remettre l'etat visuel d'orginie du slot
            nbCardInSlotObject.SetActive(false);
            icon.SetActive(true);
            countCardInslot = 0;
            nbCardInSlotText.text = countCardInslot.ToString();

            audioSource.Play();
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
        // Nettoyer les objets détruits
        RemoveDestroyedObjects();
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

    private void RemoveDestroyedObjects()
    {
        cardObjectsInSlot.RemoveAll(card => card == null);
    }
}
