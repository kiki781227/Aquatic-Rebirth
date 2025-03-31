using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class CardDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardDisplay cardDisplay;

    // Informations de la carte (par exemple, récupérées depuis ton ScriptableObject)
    private string carteNom;
    private string carteDescription;

    // Référence au manager UI (à assigner dans l'inspecteur ou chercher via tag)
    private CarteDescriptionUIManager uiManager;


    private void Start()
    {
        //Debug.Log($"{Time.time} Start: {gameObject.name}");

        cardDisplay = GetComponent<CardDisplay>();  
        if (cardDisplay != null)
        {
            carteNom = cardDisplay.cardData.cardName;
            carteDescription = cardDisplay.cardData.cardDescription;    
        }
        else
        {
            //Debug.Log("Erreur1");
        }

       

        if (uiManager == null) uiManager = FindObjectOfType<CarteDescriptionUIManager>(); ;
    }

   

    // Durée d'attente avant d'afficher la description (en secondes)
    
    

    // Quand le pointeur entre sur la carte
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("PointerEnter activer");

       
        ShowDescription();
    }

    // Quand le pointeur sort de la carte
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("PointerExit activer");

        // Effacer la description affichée
        if (uiManager != null)
            uiManager.ClearDescription();
    }

    // Coroutine qui attend le délai avant d'afficher la description
    private void ShowDescription()
    {
        
        if (uiManager != null)
        {
            uiManager.UpdateDescription(carteNom,  carteDescription);
        }
    }

}
