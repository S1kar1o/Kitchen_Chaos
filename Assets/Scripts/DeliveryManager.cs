using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitinRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private int deliveredAmount = 0;
    private void Awake()
    {
        Instance = this;
        waitinRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (GameManager.Instance.IsGamePlaying() && waitinRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitinRecipeSO = recipeListSO.Recipes[UnityEngine.Random.Range(0, recipeListSO.Recipes.Count)];

                waitinRecipeSOList.Add(waitinRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeMax; i++)
        {
            RecipeSO waitingRecipeSo = waitinRecipeSOList[i];
            if (waitingRecipeSo.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContetntMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSo.KitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContetntMatchesRecipe = false;
                    }
                }
                if (plateContetntMatchesRecipe)
                {
                    waitinRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    deliveredAmount++;
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitinRecipeSOList;
    }
    public int GetDeliveredAmount()
    {
        return deliveredAmount;
    }
}
