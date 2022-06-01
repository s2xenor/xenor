using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="Item", menuName="inventaire/Items")]
public class Items : ScriptableObject
{
    public GameObject image;
    public Sprite ImageItem; 
    public int id;
    public ItemType itemType;
    public int amount;

    public enum ItemType
    {
        PotionStrength,
        PotionDamage,
        Health
    }

    


}
