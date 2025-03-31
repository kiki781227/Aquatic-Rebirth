using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CarteDescriptionUIManager : MonoBehaviour
{
    
    // Références aux composants Text de la section
    public TMP_Text nomText;
    [SerializeField] private CanvasGroup descriCanvasGroup;
    public TMP_Text descriptionText;

    
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

    public void HideDescri()
    {
        descriCanvasGroup.alpha = 0f; // Rendre invisible
        descriCanvasGroup.blocksRaycasts = false; // Désactiver les interactions
    }

    // Appeler cette méthode pour afficher le HUD
    public void ShowDescri()
    {
        descriCanvasGroup.alpha = 1f; // Rendre visible
        descriCanvasGroup.blocksRaycasts = true; // Activer les interactions
    }
}
