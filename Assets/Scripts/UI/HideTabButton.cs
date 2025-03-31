using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHide : MonoBehaviour
{
    public GameObject hideButton;
    public GameObject showButton;
    public HideTabOceanQuest hideTabOceanQuest;
    public GameObject tabDescri;



    public void Hide()
    {
        hideTabOceanQuest.Hidetab();

        tabDescri.SetActive(false);
        hideButton.SetActive(false);
        showButton.SetActive(true);
    }

    public void Show()
    {
        hideTabOceanQuest.Showtab();
        tabDescri.SetActive(true);
        hideButton.SetActive(true);
        showButton.SetActive(false);
    }
}
