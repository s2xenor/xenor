using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventaire
{
    private List<Items> ItemList;
    public void inventory()
        {
            ItemList = new List<Items>();
            AddItem(new Items { itemType = Items.ItemType.Health, amount=1 });
            AddItem(new Items { itemType = Items.ItemType.PotionSpeed, amount = 1 });
            AddItem(new Items { itemType = Items.ItemType.PotionStreng, amount = 1 });
            AddItem(new Items { itemType = Items.ItemType.PotionEndurance, amount = 1 });
            AddItem(new Items { itemType = Items.ItemType.PotionDamage, amount = 1 });
            AddItem(new Items { itemType = Items.ItemType.HealthKO, amount = 1 });
            AddItem(new Items { itemType = Items.ItemType.PotionAttack, amount = 1 });
            Debug.Log(ItemList.Count);    
        }
    public void AddItem(Items item)
    {
        ItemList.Add(item);

    }
    public List<Items> GetItemsList()
    {
        return ItemList;
    }

    
}
