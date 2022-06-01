using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{

    /*
     * Add Object hitbox in children of Potion
     */

    public enum Type
    {
        Damage,
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
            case Type.Damage:
                damage = x;
                break;
            case Type.Heal:
                heal = x;
                break;
            default:
                break;
        }
    }

    // Make effect of potion when used
    public void Effect(player joueur, Monsters monstre, life vie)
    {
        switch (type)
        {
            case Type.Damage:
                if (joueur != null)
                {
                    vie.Reduce4(1);
                }
                else
                {
                    monstre.GetDamage(20);
                }
                break;
            case Type.Heal:
                if (joueur!= null)
                {
                    vie.HealMax();
                }
                else
                {
                    monstre.Heal();
                }
                break;
            default:
                break;
        }
    }
}
