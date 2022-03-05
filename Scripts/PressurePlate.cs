﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PressurePlate sert à detecter la présence d'un individu sur l'objet sur lequel le script est
//NE PAS OUBLIER : ajouter un composant BoxCollider2D
//NE PAS OUBLIER : les joueur doivent avoir un composant BoxCollider2D
//NE PAS OUBLIER : les joueur doivent avoir un composant RigidBody2D !!! (mettre la variable "Gravity Scale" à 0 car on est en TOP-DOWN (donc pas de gravitée))
//NE PAS OUBLIER : cocher le isTrigger sur le composant BoxCollider2D !!!
//ATTENTION : joueurs doivent avoir le tag "Player"
public class PressurePlate : MonoBehaviour
{
    /*
     * Variables Protégées
     * Accessible par les classes filles
     */

    //Bolléens qui sait si la plaque est acctuellement préssée ou non
    protected bool pressed = false;

    //Stocke le Player en contact avec la plaque
    protected List<Collider2D> player;


    /*
     * Fonctions
     */

    /**
    * <summary>Appelée lorsque qqc entre en contact avec la plaque.
    *   Seulement si c un joueur : elle indique que l'on est en contat avec la plaque + call OnPressure()</summary>
    * 
    * <param name="other">objet avec qui est sur la plaque</param>
    * 
    * <returns>Return nothing</returns>
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Si ce qui est entrée en collision est un joueur
        if (other.tag == "Player")
        {
            //On indique que la plaque est préssée
            pressed = true;
            //On stocke le player (au cas où qu'on en ai besoin plus tard pour un ajout de features (mais pour l'instant ça sert à rien)
            player.Add(other);
            //On réalise l'action
            OnPressure(other);
        }
    }

    /**
    * <summary>Appelée lorsque l'on sort du contact avec la plaque.
    *   Seulement si c un joueur : elle indique que l'on n'est plus en contat avec la plaque</summary>
    * 
    * <param name="other">objet avec qui est sur la plaque</param>
    * 
    * <returns>Return nothing</returns>
    */
    private void OnTriggerExit2D(Collider2D other)
    {
        //Si ce qui sort est un joueur
        if (other.tag == "Player")
        {
            //On indique que la plaque n'est plus préssée
            pressed = false;
            //On supprime le player stocker (il n'y a plus de player sur la plaque)
            player.Remove(other);
        }
    }

    //Fonction OnPressure() appellée si un player marche sur la plaque de pression
    //IMPORTANT : cette fonction à pour vocation à être modifier dans les classe fille (avec override)
    /**
    * <summary>Détermine ce s'il faut faire qqc (et quoi) avec le player qui est sur la plaque</summary>
    * 
    * <param name="other">objet avec qui on a collisioné</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected virtual void OnPressure(Collider2D other)
    {
        return;
    }
}
