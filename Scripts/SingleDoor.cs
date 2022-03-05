using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Permet de changer de scene avec une plaque de pression lorsque qu'il n'y a qu'une seul est unique porte (les deux joueurs empruntent la même porte)
//ATTENTION : Respecter les conventions établie dans PressurePlate
//NE PAS OUBLIER : Entrer le nom de la scene suivante
public class SingleDoor : PressurePlate
{
    /*
     * Variables Publiques
     */

    //Nom de la scene suivante qu'il faudra charger.
    public string nextSceneName;


    /*
     * Fonctions
     */

    //Fonction OnPressure() appellée si un player marche sur la plaque de pression, elle téléporte les joueurs (elle charge la nouvelle scène)
    /**
    * <summary>Détermine ce s'il faut faire qqc (et quoi) avec le player qui est sur la plaque</summary>
    * 
    * <param name="other">objet avec qui on a collisioné</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected override void OnPressure(Collider2D other)
    {
        //TODO : Enregistrer l'avancement
        //GameManager.instance.SaveState();

        //Charger la scene suivante
        SceneManager.LoadScene(nextSceneName);
    }
}
