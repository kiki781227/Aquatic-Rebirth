using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using CardData;

public class CardDisplay : MonoBehaviour
{
    [Header("Carte donnees")]
    public Card cardData;
    public Image cardImage;
    public Image traitImage;
    public GameObject priceTrait;
    public GameObject healthTrait;
    public TMP_Text priceText;
    public TMP_Text nameText;
    public TMP_Text healthText;

    [Header("Quest donnees")]
    public int nbQuestDone;
    //public bool excludeFromCounting;

    // Liste des couleurs de carte
    private Color[] cardColors =
    {
        new Color(245f / 255f, 245f / 255f, 220f / 255f, 180f / 255f), // Human Card Color
        new Color(142f / 255f, 108f / 255f, 70f / 255f), // Food Card Color
        new Color (243f / 255f , 230f / 255f, 47f / 255f, 180f / 255f), // Coin Card Color
        new Color (70f / 255f, 73f / 255f,76f / 255f, 232f / 255f), // Raw Card Material
        new Color (180f / 255f, 177f / 255f, 177f / 255f, 252f / 255f), // Primary Card Material
        new Color (141f / 255f,70f / 255f,72f / 255f), // Ennemy Card 
        new Color (47f / 255f, 93f / 255f, 107f / 255f), // Pack Card
        new Color(117f / 255f, 184f/ 255f, 255f / 255f, 255f / 255f ),// Idee Card Color
        new Color(241f / 255f, 220f / 255f ,245f / 255f, 255f/255f), // ToolCrafted Card Color
        new Color(160f / 255f, 244f / 255f, 183f / 255f, 255f / 255f), // Quest Card color 
        new Color(45f / 255f, 45f / 255f, 45f / 255f, 192f / 255f), // Quest Slot color
    };


    private void Start()
    {
        UpdateCardDisplay();   
    }

    public void Initialize(Card cardData)
    {
        if (/*!excludeFromCounting &&*/ DeckManager.Instance != null)
        {
            this.cardData = cardData;
            UpdateCardDisplay();
            DeckManager.Instance.RegisterCard(this);
            //cardData.value = cardData.originalValue;
        }

    }

    public void UpdateCardDisplay()
    {
        if (cardData == null)
        {
            Debug.LogWarning("CardData manquant sur " + gameObject.name);
            return;
        }

        int indexCardType = (int)cardData.cardType; // Retourne l'indexe d'enumeration de l'element [0] dans la liste cardType en entier
        cardImage.color = cardColors[indexCardType]; 
        nameText.text = cardData.cardName;

        switch (indexCardType) { 
            case 0: // Human
                healthText.text = cardData.health.ToString();
                traitImage.sprite = cardData.cardSprite;
                priceTrait.SetActive(false);
                break;

            case 1: // Food
                
                healthText.text = "+" + cardData.value.ToString();
                priceText.text = cardData.sellingPrice.ToString();
                traitImage.sprite = cardData.cardSprite;
                break;

            case 2: // Coin
            case 5: // Ennemy
                priceTrait.SetActive(false);
                healthTrait.SetActive(false);
                traitImage.sprite = cardData.cardSprite;
                break;      

            case 6: // CardPack
                 string pricePack = cardData.value.ToString();
                 priceText.text = pricePack;
                 break;


            case 9: // QuestCard
                 healthTrait.SetActive(false);
                 traitImage.sprite = cardData.cardSprite;
                 priceText.text = cardData.sellingPrice.ToString();
                  break;

            case 10: // QuestSlot
                 cardData.value = 1;
                 priceText.text = $"{nbQuestDone}/{cardData.value}";
                 break;

            default: // Cas: ToolCarafted , Idea, RawMaterial, PrimaryMaterial 
                priceText.text = cardData.sellingPrice.ToString();
                healthText.text = cardData.health.ToString();
                traitImage.sprite = cardData.cardSprite;
                healthTrait.SetActive(false);
                break;
                }
    }

    public void UpdateQuestProgress(int progress)
    {
        nbQuestDone = progress;
        if (cardData.cardType == CardType.QuestSlot)
        {
            priceText.text = $"{nbQuestDone}/{cardData.value}";
        }
    }

    public void ResetQuestProgress()
    {
        nbQuestDone = 0;
        if (cardData.cardType == CardType.QuestCard)
        {
            priceText.text = $"{nbQuestDone}/{cardData.value}";
        }
    }



    void OnDisable()
    {
        if (/*!excludeFromCounting &&*/ DeckManager.Instance != null)
        {
            DeckManager.Instance.UnregisterCard(this);
        }
        
    }
}
