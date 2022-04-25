using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script est à placer sur les bennes à boite (dans l'énigmes des boites)
// Permet : si l'objet qui entre dans la zon de trigger est une boite : de détruire la boite
// NE PAS OUBLIER : le dumpster doit posseder un box collider 2D avec trigger d'activé
public class Dumpster : MonoBehaviour
{
    // Si qqc rentre en contact evc le dumpster
    /**
    * <summary>Détruit l'objet qui entre en collision si c'est une boite (appellé auto par unity)</summary>
    * 
    * <param name="other">objet entrer en collision avec</param>
    * 
    * <returns>Return nothing</returns>
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        //si le qqc est une boite
        if (other.tag == "Box")
        {
            //On détruit l'objet boite.
            Destroy(other.gameObject);
        }
    }
}
