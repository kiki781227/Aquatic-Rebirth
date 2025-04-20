using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellDropZone : MonoBehaviour
{
    private CardSpawner cardSpawner;
   
    
    // Start is called before the first frame update
    void Awake()
    {
        cardSpawner = FindAnyObjectByType<CardSpawner>();
       
          
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CardMovement cardMovement = collision.GetComponent<CardMovement>();
        CardDisplay cardDisplay = collision.GetComponent<CardDisplay>();
        if (cardDisplay.cardData.isSellable)
        {
            
            
            int nbCoin = cardDisplay.cardData.sellingPrice;

            //Debug.Log(collision.gameObject);
            // Si la pi�ce est rel�ch�e ET n'a pas d�j� �t� compt�e
            if (!cardMovement.isDragging && !cardMovement.hasBeenCounted && cardDisplay.cardData.isSellable)
            {
                cardMovement.hasBeenCounted = true; // Marquer comme compt�e

               
                Destroy(collision.gameObject);
                

                for (int i = 0; i < nbCoin; i++) {
                    cardSpawner.SpawnCard(DeckManager.Instance.coinCardData);
                } 
            }
        }
     }

    }
