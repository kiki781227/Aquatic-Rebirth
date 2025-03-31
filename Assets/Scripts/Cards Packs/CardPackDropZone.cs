using CardData;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CardPackData;

public class CardPackDropZone : MonoBehaviour
{
    private CardDisplay cardDispCardPack;
    private CardSpawner cardSpawner;



    //private int countCoin = 0;
    private int cardPackPrice;
    

    private void Awake()
    {
        cardDispCardPack = gameObject.GetComponent<CardDisplay>();
        cardSpawner = gameObject.GetComponent<CardSpawner>();
  
        cardPackPrice = cardDispCardPack.cardData.value;
       
    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Coin"))
        {
            CardMovement coinMovement = collision.GetComponent<CardMovement>();

            Debug.Log(collision.gameObject);
            // Si la pièce est relâchée ET n'a pas déjà été comptée
            if (!coinMovement.isDragging && !coinMovement.hasBeenCounted)
            {
                coinMovement.hasBeenCounted = true; // Marquer comme comptée
               
                cardPackPrice--; // Prix du cardPack sur la section achat de cardPack (les cartes en vert )
                cardDispCardPack.priceText.text = cardPackPrice.ToString();

                Destroy(collision.gameObject);

                if (cardPackPrice == 0)
                {
                    cardSpawner.SpawnCard(cardDispCardPack.cardData );
                    cardPackPrice = cardDispCardPack.cardData.value;
                    cardDispCardPack.priceText.text = cardPackPrice.ToString();
                     
                }

            }
        }
    }


}
