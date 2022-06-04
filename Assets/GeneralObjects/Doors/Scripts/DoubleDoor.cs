using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Permet de changer de scene avec une plaque de pression lorsque qu'il y a deux portes et que les joueur doivent donc emprunter 2 portes différentes
//ATTENTION : Respecter les conventions établie dans PressurePlate
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

    //Appelée a chaque frame
    private void Update()
    {
        //check si on appuie sur E ssi la plaque est préssée et ssi l'autre plaque est aussi activé
        if (pressed && autrePressurePlate.pressed && Input.GetKeyDown(KeyCode.E))
        {
            //teléporte les joueurs
            Teleport();
        }
    }

    //Fonction OnPressure() appellée si un player marche sur la plaque de pression,
    //  SI (y a aussi un player sur autrePressurePlate) : elle affiche le message qui demande l'interaction
    //  SINON : elle dit qu'il faut que les deux joueurs soit sur une plaque différente pour pouvoir changer de salle
    /**
    * <summary>Détermine ce s'il faut faire qqc (et quoi) avec le player qui est sur la plaque</summary>
    * 
    * <param name="other">objet avec qui on a collisioné</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected override void OnPressure(Collider2D other)
    {
        // affiche le bon message suivant s'il y a deja qqn sur l'autre plaque de pression
        if (autrePressurePlate.pressed == true)
        {
            //On affiche le message qui indique au joueur comment intéragir avec la porte.
            MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to interact with the door");
        }
        else
        {
            //SINON
            //TODO : Changer la couleur de la lupiotte

            //On affiche le message qui indique au joueur comment intéragir avec la porte.
            MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Both players need to be on a different pressure plate");
        }

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
