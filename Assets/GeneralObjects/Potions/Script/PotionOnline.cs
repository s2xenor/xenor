using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionOnline : MonoBehaviour
{

    /*
     * Potion class
     */

    public enum Type
    {
        Heal
    }

    Type type;

    float damage = 0;
    float heal = 0;

    public PotionOnline(Type type, float x)
    {
        this.type = type;

        switch (type)
        {
            case Type.Heal:
                heal = x;
                break;
            default:
                break;
        }
    }

    // Make effect of potion when used
    public void Effect(playerOnline joueur, life vie)
    {
        switch (type)
        {  
            case Type.Heal:
                vie.HealMax();//heal player
                break;

            default:
                break;
        }
    }
}
