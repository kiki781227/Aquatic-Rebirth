using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTabOceanQuest : MonoBehaviour
{
    [SerializeField] private GameObject tabOceanQuest;

    public void Hidetab()
    {
        tabOceanQuest.SetActive(false);
    }

    // Appeler cette méthode pour afficher le HUD
    public void Showtab()
    {
        tabOceanQuest.SetActive(true);
    }
}
