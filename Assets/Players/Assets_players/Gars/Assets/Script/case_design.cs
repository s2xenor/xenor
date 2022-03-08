using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class case_design : MonoBehaviour
{
    private inventaire inventory;
    public GameObject objectitemContainer;
    public GameObject objectitemTemplate;
    private Transform ObjectitemContainer;
    private Transform ObjectitemTemplate;


    private void Awake()
    {
        ObjectitemContainer = objectitemContainer.transform;//transform.Find("objectitemContainer")
        ObjectitemTemplate = objectitemTemplate.transform;//transform.Find("objectitemTemplate")
    }

    public void SetInventory(inventaire inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        int x= 100;
        int y= 100;
        float itemCellSize = 100f;
        List<Items> list1 = inventory.GetItemsList();
        foreach (Items item in list1)
        {

            RectTransform itemRectTransform = Instantiate(objectitemTemplate, ObjectitemContainer).GetComponent<RectTransform>();
            itemRectTransform.gameObject.SetActive(true);
            itemRectTransform.anchoredPosition = new Vector2(x * itemCellSize, y * itemCellSize);
            x++;
            if(x>2)
            {
                x = 0;
                y++;
            }
        }
    }

}
