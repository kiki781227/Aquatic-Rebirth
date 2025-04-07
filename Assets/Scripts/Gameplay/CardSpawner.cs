using CardData;
using CardPackData;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardSpawner : MonoBehaviour
{
 
    public GameObject cardPackPrefab; // Le prefab de la carte à instancier
    public GameObject cardPrefab;
    public Transform cardParent;  // Le parent où les cartes seront affichées
    public int columns = 4;
    public float spacing = 10f;

    private float cardWidth; // Largeur d'une carte
    private float cardHeight;
   
    private int cardCount = 0;

    

    private void Start()
    {

   
        RectTransform rt = cardPackPrefab.GetComponent<RectTransform>();
        cardWidth = rt.sizeDelta.x * 50;
        cardHeight = rt.sizeDelta.y * 50;
    }
    // En fonction du type de Card passé, on appelle la fonction correspondante
    public void SpawnCard(Card cardData)
    {
        if (cardData is CardPack cardPackData)
        {
            CreateCardPack(cardPackData);
        }
        else
        {
            CreateCard(cardData);
        }
    }

    // Crée une carte normale
    public void CreateCard(Card cardData)
    {
         
        GameObject newCard = Instantiate(cardPrefab, cardParent, false);
        newCard.SetActive(false);

        // Positionnement de la carte
        RectTransform cardTransform = newCard.GetComponent<RectTransform>();
        cardTransform.anchoredPosition = GetNextCardPosition();


        CardTager tagger = newCard.AddComponent<CardTager>();
        tagger.SetCardData(cardData);

        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.Initialize(cardData);
            // Assigne les données de la carte
            //cardDisplay.cardData = cardData;
            //cardDisplay.UpdateCardDisplay();

        }
        newCard.SetActive(true);
    }

    //Creer un carte Pack
    public void CreateCardPack(CardPack cardPackData)
    {
        GameObject newCard = Instantiate(cardPackPrefab, cardParent, false);
        newCard.SetActive(false);

        // Positionnement de la carte
        RectTransform cardTransform = newCard.GetComponent<RectTransform>();
        cardTransform.anchoredPosition = GetNextCardPosition();


        CardTager tagger = newCard.AddComponent<CardTager>();
        tagger.SetCardData(cardPackData);

        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.Initialize(cardPackData);
            //cardDisplay.cardData = cardPackData; // Assigne les données de la carte
            cardDisplay.priceTrait.SetActive(false);
            //cardDisplay.UpdateCardDisplay();
        }
        newCard.SetActive(true);
    }

    private Vector2 GetNextCardPosition()
    {
        int row = cardCount / columns;
        int column = cardCount % columns;

        float xPosition = (column - (columns - 1) / 2f) * (cardWidth + spacing);
        float yPosition = -(row - (columns - 1) / 2f) * (cardHeight + spacing);

        cardCount++;
        return new Vector2(xPosition, yPosition);
    }

    public void ResetCardPosition()
    {
        cardCount = 0;
    }

}



