using CardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<Card> ingredients;
    //public Card humanCard; // Carte humaine obligatoire
    public Card result;
    public float cooldown; // Cooldown spécifique à chaque recette

    public Dictionary<Card, int> GetIngredientsDictionary()
    {
        Dictionary<Card, int> ingredientsDict = new Dictionary<Card, int>();
        foreach (Card ingredient in ingredients)
        {
            if (ingredientsDict.ContainsKey(ingredient))
            {
                ingredientsDict[ingredient]++;
            }
            else
            {
                ingredientsDict[ingredient] = 1;
            }
        }
        return ingredientsDict;
    }
}
