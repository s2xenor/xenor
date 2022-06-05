using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script est � placer sur les bennes � boite (dans l'�nigmes des boites)
// Permet : si l'objet qui entre dans la zon de trigger est une boite : de d�truire la boite
// NE PAS OUBLIER : le dumpster doit posseder un box collider 2D avec trigger d'activ�
public class Dumpster : MonoBehaviour
{
    // Si qqc rentre en contact evc le dumpster
    /**
    * <summary>D�truit l'objet qui entre en collision si c'est une boite (appell� auto par unity)</summary>
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
            //On d�truit l'objet boite.
            Destroy(other.gameObject);
        }
    }
}
