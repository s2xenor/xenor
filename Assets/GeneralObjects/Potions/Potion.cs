using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{

    /*
     * Add Object hitbox in children of Potion
     */

    enum Type
    {
        Damage,
        Heal
    }

    Type type;

    float damage = 0;
    float heal = 0;

    Potion(Type type, float x)
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
    public void Effect(Collider2D obj)
    {
        switch (type)
        {
            case Type.Damage:
                if (obj.tag == "Player")
                {
                    // Damage Player
                }
                else
                {
                    // Damage Monster
                }
                break;
            case Type.Heal:
                if (obj.tag == "Player")
                {
                    // Heal Player
                }
                else
                {
                    // Heal Monster
                }
                break;
            default:
                break;
        }
    }
}
