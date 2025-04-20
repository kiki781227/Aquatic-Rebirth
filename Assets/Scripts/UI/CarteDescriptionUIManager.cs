using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CarteDescriptionUIManager : MonoBehaviour
{
    
    // Références aux composants Text de la section
    public TMP_Text nomText; // nom de la carte
    public TMP_Text descriptionText; // description de la carte



    private void Start()
    {
       
        ClearDescription();
    }

    // Met à jour le contenu de la section de description
    public void UpdateDescription(string nom,  string description)
    {

        

        if (nomText != null)
            nomText.text = nom;

        if (descriptionText != null)
            descriptionText.text =  description;
    }

    // Efface le contenu de la section
    public void ClearDescription()
    {

        

        if (nomText != null)
            nomText.text = "";

        if (descriptionText != null)
            descriptionText.text = "";
    }

    public void UpdateTimerDisplay(float timer)
    {
        if (nomText != null)
            nomText.text = "Timer";

        if (descriptionText != null)
        {
            descriptionText.text = "Temps restant: " + Mathf.Ceil(timer).ToString() + "s";
        }
    }

}
