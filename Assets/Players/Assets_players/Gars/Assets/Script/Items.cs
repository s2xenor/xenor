using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items 
{
    public enum ItemType 
    {
        PotionSpeed,
        PotionStreng,
        PotionEndurance,
        PotionDamage,
        Health,//pour se soigner 
        HealthKO,//pour revivre 
        PotionAttack,//pour combattre les monstres 
    }

    public ItemType itemType;
    public int amount;


}
