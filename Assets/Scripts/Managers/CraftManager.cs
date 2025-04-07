using CardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }
    public List<CraftingRecipe> recipes;
    public CardSpawner cardSpawner;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Craft(CraftingSlot craftingSlot)
    {


        List<GameObject> cardObjectsInSlot = craftingSlot.cardObjectsInSlot;
        Dictionary<Card, int> cardsInSlot = new Dictionary<Card, int>();
        Card humanCard = null;

        foreach (GameObject cardObject in cardObjectsInSlot)
        {
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                Card cardData = cardDisplay.cardData;
                if (cardsInSlot.ContainsKey(cardData))
                {
                    cardsInSlot[cardData]++;
                }
                else
                {
                    cardsInSlot[cardData] = 1;
                }

                if (cardData.cardType == CardType.Human)
                {
                    humanCard = cardData;
                }
            }
        }
        if (humanCard == null)
        {
            Debug.Log("Aucune carte humaine trouvée dans le slot de crafting.");
            RespawnCards(craftingSlot);
            return;
        }

        foreach (CraftingRecipe recipe in recipes)
        {
            if (ContainsIngredients(cardsInSlot, recipe.GetIngredientsDictionary()))
            {
                StartCoroutine(CraftWithCooldown(craftingSlot, recipe, humanCard));
                return;
            }
        }

        // Si aucun crafting n'a réussi, respawn les cartes
        RespawnCards(craftingSlot);
    }

    private IEnumerator CraftWithCooldown(CraftingSlot craftingSlot, CraftingRecipe recipe, Card humanCard)
    {
        craftingSlot.StartCooldown(recipe.cooldown);
        yield return new WaitForSeconds(recipe.cooldown);

        // Crafting réussi après le cooldown
        cardSpawner.SpawnCard(recipe.result);
        cardSpawner.SpawnCard(humanCard);
        DestroyIngredients(craftingSlot, recipe.GetIngredientsDictionary());
        craftingSlot.ClearSlot();
    }

    private bool ContainsIngredients(Dictionary<Card, int> cardsInSlot, Dictionary<Card, int> ingredients)
    {
        foreach (KeyValuePair<Card, int> ingredient in ingredients)
        {
            if (!cardsInSlot.ContainsKey(ingredient.Key) || cardsInSlot[ingredient.Key] < ingredient.Value)
            {
                return false;
            }
        }
        return true;
    }

    private void DestroyIngredients(CraftingSlot craftingSlot, Dictionary<Card, int> ingredients)
    {
         foreach (GameObject cardObject in craftingSlot.cardObjectsInSlot)
        {
            Debug.Log("Destroying card: " + cardObject.GetComponent<CardDisplay>().cardData.cardName);
            Destroy(cardObject);
        }
    }

    private void RespawnCards(CraftingSlot craftingSlot)
    {
        foreach (GameObject cardObject in craftingSlot.cardObjectsInSlot)
        {
            cardSpawner.SpawnCard(cardObject.GetComponent<CardDisplay>().cardData);
        }
        craftingSlot.ClearSlot();
    }
}


