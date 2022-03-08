using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Permet de changer de scene avec une plaque de pression lorsque qu'il n'y a qu'une seul est unique porte (les deux joueurs empruntent la même porte)
//ATTENTION : Respecter les conventions établie dans PressurePlate
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

    //Appelée a chaque frame
    private void Update()
    {
        //check si on appuie sur E ssi la plaque est préssée
        if (pressed && Input.GetKeyDown(KeyCode.E))
        {
            //teléporte les joueurs
            Teleport();
        }
    }

    //Fonction OnPressure() appellée si un player marche sur la plaque de pression, elle affiche le message pour demander une intéraction
    /**
    * <summary>Détermine ce s'il faut faire qqc (et quoi) avec le player qui est sur la plaque</summary>
    * 
    * <param name="other">objet avec qui on a collisioné</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected override void OnPressure(Collider2D other)
    {
        //On affiche le message qui indique au joueur comment intéragir avec la porte.
        MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to interact with the door");
    }

    //Fonction Teleport() teleporte les joueurs vers la salle suivante
    /**
    * <summary>Charge la nouvelle salle</summary>
    * 
    * <returns>Return nothing</returns>
    */
    private void Teleport()
    {
        //TODO : Enregistrer l'avancement
        //GameManager.instance.SaveState();

        //Charger la scene suivante
        SceneManager.LoadScene(nextSceneName);
    }
}
