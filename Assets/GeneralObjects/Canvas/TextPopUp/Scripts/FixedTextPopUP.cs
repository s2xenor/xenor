using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sert a afficher un message (passer en argument) sur l'�cran du joueur
// A mettre sur le canvas qui contient le texte de message d'interaction.
// ATTENTION : Le Canvas doit contenir un �l�ment Texte 
// ATTENTION : Le script est sur le Canvas
// NE PAS OUBLIER : de d�sactiv� le canva (et seulement le canva) dans unity ! 
public class FixedTextPopUP : MonoBehaviour
{
    /*
     * Varaibles Publiques
     */

    //Variable qui permet de v�rouiller (ne pas supprimer et �v�ntuellement ne pas ajouter) un autre message sur l'�cran du joueur
    public bool textLock = false;

    Canvas canvas;

    /*
     * Fonctions
     */

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        //canvas.enabled = false;
        Debug.Log(canvas);
    }

    //Fonction PressToInteractText() permet d'afficher un message utilitaire sur l'�crand du joueur
    /**
    * <summary>Permet d'afficher un message sur l'�cran du joueur</summary>
    * 
    * <param name="message">message que l'on souhaite afficher</param>
    * 
    * <returns>Return nothing</returns>
    */
    public void PressToInteractText(string message)
    {
        if (!textLock)
        {
            Debug.Log(canvas);
            canvas.enabled = true;
            //Cherche l'�l�ment texte du texte dans le Canvas
            Text txt = gameObject.GetComponentInChildren<Text>();
            txt.enabled = true;
            gameObject.GetComponentInChildren<Text>().text = message;
        }
    }

    //Fonction PressToInteractText() permet de supprimer un �ventuel message pr�sent sur l'�cran du joueur
    /**
    * <summary>Permet de d�sactiver l'affichage du message de l'�cran du joueur</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void SupprPressToInteractText()
    {
        if (!textLock)
        {
            gameObject.GetComponentInChildren<Text>().enabled = false;
            canvas.enabled = false;
        }
    }
}
