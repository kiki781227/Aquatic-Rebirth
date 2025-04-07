using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using CardData;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public Image traitImage;
    public GameObject priceTrait;
    public GameObject healthTrait;
    public TMP_Text priceText;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public int nbQuestDone;
    public bool excludeFromCounting;

    private Color[] cardColors =
    {
        new Color(245f / 255f, 245f / 255f, 220f / 255f, 255f / 255f), // Human Card Color
        new Color(142f / 255f, 108f / 255f, 70f / 255f), // Food Card Color
        new Color (243f / 255f , 230f / 255f, 47f / 255f), // Coin Card Color
        new Color (70f / 255f, 73f / 255f,76f / 255f, 232f / 255f), // Raw Card Material
        new Color (137f / 255f, 139f / 255f, 151f / 255f), // Primary Card Material
        new Color (141f / 255f,70f / 255f,72f / 255f), // Ennemy Card 
        new Color (30f / 255f, 122f / 255f, 118f / 255f), // Pack Card
        new Color(117f / 255f, 184f/ 255f, 255f / 255f, 255f / 255f ),// Idee Card Color
        new Color(0,0,0,0),
        new Color(160f/255f, 244f/255f, 183f/255f, 255f/255f), // Quest Card color 
        new Color(45f/255f, 45f/255f, 45f/255f, 192f/255f),
    };


    private void Start()
    {
        UpdateCardDisplay();   
    }

    public void Initialize(Card cardData)
    {
        if (!excludeFromCounting && DeckManager.Instance != null)
        {
            this.cardData = cardData;
            UpdateCardDisplay();
            DeckManager.Instance.RegisterCard(this);
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
            case 0:
                healthText.text = cardData.health.ToString();
                traitImage.sprite = cardData.cardSprite;
                priceTrait.SetActive(false);
                break;

            case 1:
                
                healthText.text = "+" + cardData.value.ToString();
                priceText.text = cardData.sellingPrice.ToString();
                traitImage.sprite = cardData.cardSprite;
                break;

            case 2:
            case 5:
                priceTrait.SetActive(false);
                healthTrait.SetActive(false);
                traitImage.sprite = cardData.cardSprite;
                break;

          

            case 6:
                    // Display du pack sur la partie achat
                    string pricePack = cardData.value.ToString();
                    priceText.text = pricePack;

                    break;


                case 9:
                    healthTrait.SetActive(false);
                    traitImage.sprite = cardData.cardSprite;
                    priceText.text = cardData.sellingPrice.ToString();
                    break;

                case 10:
                    cardData.value = Random.Range(1, 3);
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
        if (!excludeFromCounting && DeckManager.Instance != null)
        {
            //Debug.Log($"OnEnable appelé sur {gameObject.name} à {Time.time}");
            //Debug.Log($"Désenregistrement de la carte : {cardData.cardName}");
            DeckManager.Instance.UnregisterCard(this);
        }
        
    }
}
