using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Classe qui gère les collision avec le tabouret
// Permet que seul les palyers puissant marcher sur les tabourets
public class Stool : MonoBehaviour
{
    //Fonction OnCollisionEnter2D() appelé à chaque collision
    /**
    * <summary>Appelé a chaque collision : permet que le joueur ignore les collisons avec cet objet, mais pas les autres objets</summary>
    * 
    * <param name="collision">objet qui est entré en collision avec l'objet qui porte le script</param>
    * 
    * <returns>Return nothing</returns>
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }
}
