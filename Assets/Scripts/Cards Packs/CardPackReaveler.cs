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
    private bool humanCardGenerated = false;
    private Vector3[] spawnOffsets = new Vector3[5]
    {
        new Vector3(140f, 0f, 0f),   // Position 1 (haut)
        new Vector3(140*2f, 0f, 0f),   // Position 2 (droite)
        new Vector3(-140f, 0f, 0f),  // Position 3 (bas)
        new Vector3(-140*2f, 0f, 0f), // Position 4 (gauche)
        new Vector3(0f,0f,0f) // Position 5 (milieu)
    };
   

    [Header("Settings")]
    private Transform parentTransform; // R�f�rence � l'objet parent
    public GameObject cardPrefab; // La prefab � utiliser pour les cartes
    

    private void Start()
    {
        
        
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        cardPack = cardDisplay.cardData as CardPack;
     
         
        if (cardPack == null || cardDisplay.cardData == null)
            Debug.LogError("CardDisplay ou cardData non assign� !");

            
          
        // Trouver dynamiquement l'objet parent dans la sc�ne
        GameObject parentObject = GameObject.Find("CardManager");
        if (parentObject != null)
        {
            parentTransform = parentObject.transform;
        }
        else
        {
            Debug.LogError("Objet parent non trouv� dans la sc�ne!");
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown d�clench�");
        if (!isRevealing && revealedCount < cardPack.toReveal)
        {
            RevealNextCard();
           
            //Debug.Log(revealedCount);
            //Debug.Log(cardPack.toReveal);

        }
    }

    private void RevealNextCard()
    {
       
        isRevealing = true;
        if (cardPack == null || revealedCount > cardPack.containedCard.Count)
        {
            Debug.LogError("Erreur lors de la r�v�lation de la carte.");
            isRevealing = false;
            return;
        }

        Card selectedCard;

        // V�rification si c'est un StarterPack
        if (cardPack.isStarterPack)
        {
            // R�v�lation ordonn�e pour les StarterPacks
            selectedCard = cardPack.containedCard[revealedCount];
        }
        else
        {
            
            List<Card> availableCards = new List<Card>(cardPack.containedCard);

            if (humanCardGenerated)
            {
                // Exclure les cartes de type Human si une carte Human a d�j� �t� g�n�r�e
                availableCards.RemoveAll(card => card.cardType == CardType.Human);
            }

            // G�n�rer une carte al�atoire parmi les cartes disponibles
            int cardIndex = Random.Range(0, availableCards.Count);
            selectedCard = availableCards[cardIndex];

            // V�rifier si la carte g�n�r�e est de type Human
            if (selectedCard.cardType == CardType.Human)
            {
                humanCardGenerated = true; // Marquer qu'une carte Human a �t� g�n�r�e
            }
        }

        // Cr�ation de la carte
        GameObject newCard = Instantiate(
                cardPrefab,
                transform.position + spawnOffsets[revealedCount],
                Quaternion.identity,
                parentTransform 
            );
           
            
           
            CardDisplay newCardDisplay = newCard.GetComponent<CardDisplay>();
        if (newCardDisplay != null)
        {
            CardTager tagger = newCard.AddComponent<CardTager>();

            if (selectedCard.cardType == CardType.Food)
            {
                // On clone le ScriptableObject selectedCard
                selectedCard = Instantiate(selectedCard);
            }

            tagger.SetCardData(selectedCard);


            newCardDisplay.Initialize(selectedCard);


        }

        revealedCount++;

        if (revealedCount >= cardPack.toReveal)
        {
            cardPack.isOpened = true;
            Destroy(gameObject);        
        }
            
        isRevealing =false;
        
    }


}
