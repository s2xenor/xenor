using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
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

    public Potion(Type type, float x)
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
    public void Effect(player joueur, life vie)
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
