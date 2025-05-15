using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CardData;

public class HeadUpDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text foodText;
    private int nbfoodRequired = 0;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text cardOnTableTxt;
    private int maxCardOnTable = 12;
    [SerializeField] private TMP_Text oceanHealth;

    private int oceanMaxHealth;
    private int oceanActualHealth;
    private bool isHudUpdatePending = false;
   
    [SerializeField] private CanvasGroup hudCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        DeckManager.Instance.OnCardsUpdate += UpdateHUD;
        if (GameManager.Instance != null)
        {
            oceanMaxHealth = GameManager.Instance.oceanLifeMax;
            oceanActualHealth = GameManager.Instance.oceanActualLife;
            GameManager.Instance.OnOceanLifeChanged += UpdateOceanHealth;
        }
        else
        {
            Debug.LogError("GameManager.Instance est NULL !");
        }
    }



    // Update is called once per frame
    void UpdateHUD()
    {
        if (!isActiveAndEnabled) return;

        if (!isHudUpdatePending)
        {
            StartCoroutine(UpdateHUDCoroutine());
        }

    }

    private IEnumerator UpdateHUDCoroutine()
    {
        isHudUpdatePending = true;
        yield return new WaitForEndOfFrame();

        if (!isActiveAndEnabled)
        {
            isHudUpdatePending = false;
            yield break;
        }

        
        int totalFoodValue = CalculateTotalFoodValue();
        Debug.Log($"totalFoodValue{totalFoodValue}");
        int humansCards = DeckManager.Instance.CountCardsOfType(CardData.CardType.Human);
        nbfoodRequired = 2 + humansCards - 1;
        //Debug.Log("Human cards count : " + humansCards);
        foodText.text = $"{totalFoodValue}/{nbfoodRequired}";
        foodText.color = totalFoodValue < nbfoodRequired ? Color.red : Color.black;

        int coinCards = DeckManager.Instance.CountCardsOfType(CardData.CardType.Coin);
        //Debug.Log("Coin card count : " + coinCards);
        coinText.text = coinCards.ToString();
        coinText.color = coinCards < 1 ? Color.red : Color.black;

        int nbCardsOnTable = DeckManager.Instance.CountCardsExcludingTypes(CardType.Ennemy, CardType.Human);
        //Debug.Log("Total cards on table: " + nbCardsOnTable);
        cardOnTableTxt.text = $"{nbCardsOnTable}/{maxCardOnTable}";
        cardOnTableTxt.color = nbCardsOnTable > maxCardOnTable ? Color.red : Color.black;   


        oceanHealth.text = $"{oceanActualHealth}/{oceanMaxHealth}";
        oceanHealth.color = oceanActualHealth < oceanMaxHealth / 2 ? Color.red : Color.black;

        isHudUpdatePending = false;
    }

    private void UpdateOceanHealth(int newLife)
    {
        oceanActualHealth = newLife;
        oceanHealth.text = $"{oceanActualHealth}/{oceanMaxHealth}";
        oceanHealth.color = oceanActualHealth < oceanMaxHealth / 2 ? Color.red : Color.black;
    }

    private int CalculateTotalFoodValue()
    {
        int totalFoodValue = 0;
        int count = 0;
        CardDisplay[] foodCards = DeckManager.Instance.GetCardsOfType(CardData.CardType.Food);
        foreach (CardDisplay foodCard in foodCards)
        {
            if (foodCard != null && foodCard.cardData != null && foodCard.gameObject.activeInHierarchy)
            {
                count++;
                //Debug.Log("nbCarteFood sur table: " + count);
                Debug.Log("foodValue apres update: " +foodCard.cardData.value); 
                totalFoodValue += foodCard.cardData.value;
            }
        }
        return totalFoodValue;
    }

    public void HideHUD()
    {
        hudCanvasGroup.alpha = 0f; // Rendre invisible
        hudCanvasGroup.blocksRaycasts = false; // Désactiver les interactions
    }

    // Appeler cette méthode pour afficher le HUD
    public void ShowHUD()
    {
        hudCanvasGroup.alpha = 1f; // Rendre visible
        hudCanvasGroup.blocksRaycasts = true; // Activer les interactions
    }
}
