using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventaire : MonoBehaviour
{
    public int amount;
    public Image ItemUIImage;
    public List<Items> ItemList = new List<Items>();
    public life vie;
    public static inventaire instance;
    public int index = 0;

    public inventaire()
    {

         //AddItem(new Items { itemType = Items.ItemType.Health, amount=1 });
         //AddItem(new Items { itemType = Items.ItemType.PotionStreng, amount = 1 });
         //AddItem(new Items { itemType = Items.ItemType.PotionDamage, amount = 1 });


    }
     /*public void AddItem(Items item)
     {
         ItemList.Add(item);

     }*/
     public List<Items> GetItemsList()
     {
         return ItemList;
     }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de l'inventaire dans la scène");
            return;
        }
        instance = this;
    }


    public void UpdateInventory()
    {
        ItemUIImage.sprite = ItemList[index].ImageItem;
    }

    public void AddItem(int amount)
    {
        this.amount += amount;
        //UpdateTextUI();
        
    }

    public void ConsumeItem()
    {
        Items current_item = ItemList[2];
        vie.HealMax();
        ItemList.Remove(current_item);
    }

    public void RemoveItem(int amount)
    {
        this.amount -= amount;
        //UpdateTextUI();

    }



}
