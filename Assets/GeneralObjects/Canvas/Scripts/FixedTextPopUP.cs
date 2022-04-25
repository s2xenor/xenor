using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sert a afficher un message (passer en argument) sur l'écran du joueur
// A mettre sur le canvas qui contient le texte de message d'interaction.
// ATTENTION : Le Canvas doit contenir un élément Texte 
// ATTENTION : Le script est sur le Canvas
// NE PAS OUBLIER : de désactivé le canva (et seulement le canva) dans unity ! 
public class FixedTextPopUP : MonoBehaviour
{
    /*
     * Varaibles Publiques
     */

    //Variable qui permet de vérouiller (ne pas supprimer et événtuellement ne pas ajouter) un autre message sur l'écran du joueur
    public bool textLock = false;


    /*
     * Fonctions
     */

    //Fonction PressToInteractText() permet d'afficher un message utilitaire sur l'écrand du joueur
    /**
    * <summary>Permet d'afficher un message sur l'écran du joueur</summary>
    * 
    * <param name="message">message que l'on souhaite afficher</param>
    * 
    * <returns>Return nothing</returns>
    */
    public void PressToInteractText(string message)
    {
        if (!textLock)
        {
            //Cherche l'élément texte du texte dans le Canvas
            gameObject.GetComponentInChildren<Text>().text = message;
            gameObject.SetActive(true);
        }
    }

    //Fonction PressToInteractText() permet de supprimer un éventuel message présent sur l'écran du joueur
    /**
    * <summary>Permet de désactiver l'affichage du message de l'écran du joueur</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void SupprPressToInteractText()
    {
        if (!textLock)
        {
            gameObject.SetActive(false);
        }
    }

}
