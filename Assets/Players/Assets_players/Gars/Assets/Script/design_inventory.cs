using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class design_inventory : MonoBehaviour
{
    private inventaire inventory;
    private Transform objectitemContainer;
    private Transform objectitemTemplate;


    private void Awake()
    {
        objectitemContainer = transform.Find("objectitemContainer");
        objectitemTemplate = /*objectitemContainer*/transform.Find("objectitemTemplate");
    }

    public void SetInventory(inventaire inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        int x=0;
        int y=0;
        float itemCellSize = 30f;
        List<Items> list1 = inventory.GetItemsList();
        foreach (var item in list1)
        {
            RectTransform itemRectTransform = Instantiate(objectitemTemplate, objectitemContainer).GetComponent<RectTransform>();
            itemRectTransform.gameObject.SetActive(true);
            itemRectTransform.anchoredPosition = new Vector2(x * itemCellSize, y * itemCellSize);
            x++;
            if(x<4)
            {
                x = 0;
                y++;
            }
        }
    }

}
