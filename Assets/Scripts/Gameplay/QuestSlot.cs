using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    

    // Indique si ce slot a d�j� re�u une carte
    public bool isFilled = false;
    private int numberOfCardDroped = 0;
    [HideInInspector] public CardDisplay cardDisplay;
    // M�thode appel�e pour remplir le slot avec une carte

    private void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
    }

    public void FillSlot(GameObject card)
    {
        numberOfCardDroped++;

        //Update l'affichage du nombre de cartes dans le slot
        cardDisplay.UpdateQuestProgress(numberOfCardDroped);
        
        if (!isFilled)
        {
            if (numberOfCardDroped == cardDisplay.cardData.value)
            {
                isFilled = true;
                GameManager.Instance.UpdateQuestProgress();
                numberOfCardDroped = 0;
                
            }
            
            //Debug.Log($"{gameObject.name} rempli par {card.name}");
            
        }
    }

    // D�tection des collisions avec les cartes (les slots sont des triggers)
    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Le trigger marche");
        // V�rifier si l'objet entrant est bien une carte (par exemple, on utilise le tag "Card")
        
        if (!isFilled)
        {
            CardMovement cardMovement = other.GetComponent<CardMovement>();
            CardDisplay otherCardDisplay = other.GetComponent<CardDisplay>();
            //Debug.Log(cardMovement.hasBeenCounted);
            if (!cardMovement.isDragging && !cardMovement.hasBeenCounted && otherCardDisplay.cardData.cardName == cardDisplay.cardData.cardName)
            {
                
                // On v�rifie la compl�tude de la phase dans le QuestManage         
                    cardMovement.hasBeenCounted = true;
                    //Debug.Log(cardMovement.hasBeenCounted);
                    FillSlot(other.gameObject);
                    Destroy(other.gameObject);

            }
        }

    }


}
