using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Permet de changer de scene avec une plaque de pression lorsque qu'il y a deux portes et que les joueur doivent donc emprunter 2 portes différentes
//ATTENTION : Respecter les conventions établie dans PressurePlate
//NE PAS OUBLIER : de relier manuellement le deux portes lors de la crétion de scene les objets entre eux !
//NE PAS OUBLIER : Entrer le nom de la scene suivante
public class DoubleDoor : PressurePlate
{
    /*
     * Variables Publiques
     */

    //Nom de la scene suivante qu'il faudra charger.
    public string nextSceneName;

    //L'autre plaque de pression (de l'autre porte qui lui est liée)
    //NE PAS OUBLIER : de relier manuellemant lors de la crétion de scene les objets entre eux !
    public DoubleDoor autrePressurePlate;


    /*
     * Fonctions
     */

    //Fonction OnPressure() appellée si un player marche sur la plaque de pression,
    //  SI (y a aussi un player sur autrePressurePlate) : elle téléporte les joueurs (elle charge la nouvelle scène)
    //  SINON : Rien ne se passe
    /**
    * <summary>Détermine ce s'il faut faire qqc (et quoi) avec le player qui est sur la plaque</summary>
    * 
    * <param name="other">objet avec qui on a collisioné</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected override void OnPressure(Collider2D other)
    {
        // SI (y a aussi un player sur autrePressurePlate) : elle téléporte les joueurs (elle charge la nouvelle scène)
        //Check si le booléen de autrePressurePlate de la classe componente PressurePlate est true
        if (autrePressurePlate.pressed == true)
        {
            //TODO : Enregistrer l'avancement
            //GameManager.instance.SaveState();

            //Charger la scene suivante
            SceneManager.LoadScene(nextSceneName);
        }
        //SINON
        //TODO : Changer la couleur de la lupiotte
    }
}
