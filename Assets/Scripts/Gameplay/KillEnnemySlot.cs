using CardData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillEnnemySlot : MonoBehaviour
{
    public List<GameObject> cardObjectsInSlot = new List<GameObject>();
   
    public GameObject cooldownObject;
    public GameObject icon;
    public GameObject nbCardInSlotObject;

    public Image cooldownBar;
    public TMP_Text nbCardInSlotText;

    private bool isInCooldown = false;
    private int countCardInslot = 0;
    public float currentCoolDown = 0f;
    private float totalCooldownTime = 0f; // Durée totale du cooldown


    private void Update()
    {
        if (isInCooldown)
        {
            currentCoolDown -= Time.deltaTime;
            UpdateCooldownBar();

            if (currentCoolDown <= 0)
            {
                isInCooldown = false;
                currentCoolDown = 0f;
                cooldownObject.SetActive(false);

                // Détruire la carte Ennemy et respawn la carte Human
                HandleCooldownCompletion();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInCooldown) return;

        CardMovement cardMovement = collision.GetComponent<CardMovement>();
        CardDisplay cardDisplay = collision.GetComponent<CardDisplay>();

        if (cardMovement != null && cardDisplay != null && !cardMovement.isDragging)
        {
            // Vérifier si une carte Ennemy est déjà présente
            bool enemyCardPresent = cardObjectsInSlot.Exists(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.Ennemy);
            bool humanCardPresent = cardObjectsInSlot.Exists(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.Human);

            // Conditions pour ajouter une carte Ennemy
            if (cardDisplay.cardData.cardType == CardType.Ennemy && !enemyCardPresent && cardObjectsInSlot.Count < 2)
            {
                AddCardToSlot(collision.gameObject);
                collision.transform.position = new Vector3(-1000, -1000, 0);
                enemyCardPresent = true;
                Debug.Log("Enemy card added to slot.");

                // Sert a compter le nombre de carte dans le slot
                countCardInslot++;
                nbCardInSlotText.text = countCardInslot.ToString();
                icon.SetActive(false);
                nbCardInSlotObject.SetActive(true);
            }
            // Conditions pour ajouter une carte Human
            else if (cardDisplay.cardData.cardType == CardType.Human && enemyCardPresent && !humanCardPresent && cardObjectsInSlot.Count < 2)
            {
                AddCardToSlot(collision.gameObject);
                collision.transform.position = new Vector3(-1000, -1000, 0);
                humanCardPresent = true;
                Debug.Log("Human card added to slot.");

                countCardInslot++;
                nbCardInSlotText.text = countCardInslot.ToString();
            }

            // Si les deux cartes sont présentes, démarrer le cooldown
            if (enemyCardPresent && humanCardPresent && !isInCooldown)
            {
                StartCooldown(6f); 
            }
        }
    }

    private void AddCardToSlot(GameObject cardObject)
    {
        cardObjectsInSlot.Add(cardObject);
        


    }

    private void StartCooldown(float cooldown)
    {
        isInCooldown = true;
        totalCooldownTime = cooldown;
        currentCoolDown = cooldown;
        cooldownObject.SetActive(true);
        UpdateCooldownBar();
    }

    private void HandleCooldownCompletion()
    {

        RemoveDestroyedObjects();
        // Trouver les cartes Ennemy et Human
        GameObject enemyCard = cardObjectsInSlot.Find(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.Ennemy);
        GameObject humanCard = cardObjectsInSlot.Find(card => card.GetComponent<CardDisplay>().cardData.cardType == CardType.Human);



        // Respawn la carte Human
        if (humanCard != null)
        {
            cardObjectsInSlot.Remove(enemyCard);
            Destroy(enemyCard);
            Debug.Log("Enemy card destroyed.");

            humanCard.transform.localPosition = new Vector3(0, 0, 0);
            Debug.Log("Human card respawned.");

            nbCardInSlotObject.SetActive(false);
            icon.SetActive(true);
            countCardInslot = 0;
        }
        else
        {
            
            if (enemyCard != null)
            {
                enemyCard.transform.localPosition = new Vector3(0, 0, 0); // Position par défaut ou originale
                Debug.Log("Enemy card respawned.");
            }
        }

        // Nettoyer le slot
        ClearSlot();
    }

    private void ClearSlot()
    {
        cardObjectsInSlot.Clear();
    }

    private void UpdateCooldownBar()
    {
        if (cooldownBar != null && totalCooldownTime > 0)
        {
            cooldownBar.fillAmount = currentCoolDown / totalCooldownTime;
        }
    }

    private void RemoveDestroyedObjects()
    {
        cardObjectsInSlot.RemoveAll(card => card == null);
    }
}
