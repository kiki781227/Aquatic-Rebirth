using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject panelQuest;
    public GameObject panelCraft;
    public GameObject panelKill;

    public Button tabQuestBtn;
    public Button tabCraftBtn;
    public Button tabKillBtn;

    private Color originalColor;
    private Button currentSelectedButton;

    private void Start()
    {
        // Stocker la couleur d'origine des boutons
        originalColor = tabQuestBtn.colors.normalColor;

    }

    public void ShowTab(string tabName)
    {
        // Désactiver tous les panneaux
        panelQuest.SetActive(false);
        panelCraft.SetActive(false);
        panelKill.SetActive(false);

        // Réinitialiser la couleur du bouton actuellement sélectionné
        if (currentSelectedButton != null)
        {
            ResetButtonColor(currentSelectedButton);
        }

        // Activer le panneau correspondant et définir la couleur du bouton sélectionné
        switch (tabName)
        {
            case "Quest":
                panelQuest.SetActive(true);
                SetSelectedColor(tabQuestBtn);
                currentSelectedButton = tabQuestBtn;
                break;
            case "Craft":
                panelCraft.SetActive(true);
                SetSelectedColor(tabCraftBtn);
                currentSelectedButton = tabCraftBtn;
                break;
            case "Kill":
                panelKill.SetActive(true);
                SetSelectedColor(tabKillBtn);
                currentSelectedButton = tabKillBtn;
                break;
        }
    }

    private void SetSelectedColor(Button btn)
    {
        var colors = btn.colors;
        colors.normalColor = colors.selectedColor; // Appliquer la couleur sélectionnée
        btn.colors = colors;
    }

    private void ResetButtonColor(Button btn)
    {
        var colors = btn.colors;
        colors.normalColor = originalColor; // Remettre la couleur d'origine
        btn.colors = colors;
    }
}

