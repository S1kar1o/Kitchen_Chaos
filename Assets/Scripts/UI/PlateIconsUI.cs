using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject kitchenObject;
    [SerializeField] private Transform iconTemplate;
    private void Start()
    {
        kitchenObject.OnIngredientAdded += KitchenObject_OnIngredientAdded;
    }
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if (child == iconTemplate)
                continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in kitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform=Instantiate(iconTemplate,transform);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
            iconTransform.gameObject.SetActive(true);

        }
    }
    private void KitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedArgs e)
    {
        UpdateVisual();
    }
}
