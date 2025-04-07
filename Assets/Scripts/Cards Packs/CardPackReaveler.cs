using CardPackData;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using CardData;
using Unity.VisualScripting;
using UnityEngine.EventSystems;


public class CardPackReaveler : MonoBehaviour, IPointerDownHandler
{
  
    private CardPack cardPack;
    private int revealedCount = 0;
    private bool isRevealing = false;
    private Vector3[] spawnOffsets = new Vector3[5]
    {
        new Vector3(100f, 0f, 0f),   // Position 1 (haut)
        new Vector3(100*2f, 0f, 0f),   // Position 2 (droite)
        new Vector3(-100f, 0f, 0f),  // Position 3 (bas)
        new Vector3(-100*2f, 0f, 0f), // Position 4 (gauche)
        new Vector3(0f,0f,0f) // Position 5 (milieu)
    };
   

    [Header("Settings")]
    private Transform parentTransform; // Référence à l'objet parent
    public GameObject cardPrefab; // La prefab à utiliser pour les cartes
    

    private void Start()
    {
        
        
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        cardPack = cardDisplay.cardData as CardPack;
     
         
        if (cardPack == null || cardDisplay.cardData == null)
            Debug.LogError("CardDisplay ou cardData non assigné !");
        else
            Debug.Log("CardPack récupéré avec " + cardPack.containedCard.Count + " cartes, et cardDisplay non null.");

        if (cardPrefab == null)
            Debug.LogError("Assdignez une prefab de carte dans l'inspecteur!");
          
        // Trouver dynamiquement l'objet parent dans la scène
        GameObject parentObject = GameObject.Find("CardManager");
        if (parentObject != null)
        {
            parentTransform = parentObject.transform;
        }
        else
        {
            Debug.LogError("Objet parent non trouvé dans la scène!");
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown déclenché");
        if (!isRevealing && revealedCount < cardPack.toReveal)
        {
            StartCoroutine(RevealNextCard());
           
            //Debug.Log(revealedCount);
            //Debug.Log(cardPack.toReveal);

        }
    }

    private IEnumerator RevealNextCard()
    {
        
        isRevealing = true;
        if (cardPack == null || revealedCount > cardPack.containedCard.Count)
        {
            Debug.LogError("Erreur lors de la révélation de la carte.");
            isRevealing = false;
            yield break;
        }

        // Création de la carte
        GameObject newCard = Instantiate(
                cardPrefab,
                transform.position + spawnOffsets[revealedCount],
                Quaternion.identity,
                parentTransform // Utilisez "transform" si vous voulez parent à CardPack
            );
           
            
           
            CardDisplay newCardDisplay = newCard.GetComponent<CardDisplay>();
            if (newCardDisplay != null)
            {
                CardTager tagger = newCard.AddComponent<CardTager>();
                tagger.SetCardData(cardPack.containedCard[revealedCount]);

            
            newCardDisplay.Initialize(cardPack.containedCard[revealedCount]);

            }

            newCard.SetActive(true);
            yield return new WaitForEndOfFrame();
            revealedCount++;

            if (revealedCount >= cardPack.toReveal)
            {
                cardPack.isOpened = true;
                Destroy(gameObject);
                
            }

            yield return new WaitForEndOfFrame();
            cardPack.isOpened = false;
            isRevealing =false;
        
    }


}
