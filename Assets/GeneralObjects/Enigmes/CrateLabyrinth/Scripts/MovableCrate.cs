using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCrate : PressurePlate
{
    /*
     * Vraiables Privées
     */

    private bool linkedToPlayer = false;

    private GameObject playerLinked;

    /*
     * Fonctions
     */


    //Appelée a chaque frame
    private void Update()
    {
        //check si on appuie sur E ssi en contact avec la boite est préssée
        if (Input.GetKeyDown(KeyCode.E))
        {
            //si qqn entre en contact avec la boite ET que la boite n'est pas déjà liée à qqn
            if (pressed && !linkedToPlayer)
            {
                // on la lie au perso
                Link();
            }
            //sinon, si la boite est déjà liée à ce qqn, on lui fais lâcher la boite
            else if (linkedToPlayer)
            {
                UnLink();
            }
        }
    }

    /**
    * <summary>Affiche le message pour proposer à l'utilisateur d'intéragir</summary>
    * 
    * <param name="other">objet avec qui on a collisioné (ici le joueur)</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected override void OnPressure(Collider2D other)
    {
        //On affiche le message qui indique au joueur comment intéragir avec la porte.
        //Grace à fonctionnalité lock le message ne s'affichera que si c pas lock
        MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to start pulling the box");
    }

    /**
    * <summary>Lier l'objet avec le premier player rentré en collision avec l'objet</summary>
    * 
    * <returns>Return nothing</returns>
    */
    private void Link()
    {
        //le premier joueur qui est entré en collision avec la boite stocké dans PressurePlate.cs
        playerLinked = player[0].gameObject; 

        //on récupere le component qui permet de faire le lien
        FixedJoint2D lienActuel = playerLinked.GetComponent<FixedJoint2D>();
        //On stocke la position de la boite et du joueur
        //Cela permet d'éviter que le point d'ancrage fasse bouger la boite est le joueur au moment de l'attache
        Vector2 posBox = this.transform.position;
        Vector2 posPlayer = playerLinked.transform.position;
        //On active le component
        lienActuel.enabled = true;
        lienActuel.anchor = posBox;
        lienActuel.connectedAnchor = posPlayer;
        //On lie au component le rigidbody de la boite
        lienActuel.connectedBody = this.GetComponent<Rigidbody2D>();

        //on indique que le lien a été établi avec succés
        linkedToPlayer = true;
    }

    /**
    * <summary>Délier l'objet du player avec lequel il était lié</summary>
    * 
    * <returns>Return nothing</returns>
    */
    private void UnLink()
    {
        //on récupere le component qui permet de faire le lien
        FixedJoint2D lienActuel = playerLinked.GetComponent<FixedJoint2D>();
        //On le desactive
        lienActuel.enabled = false;
        //On réinitialise le lien
        lienActuel.connectedBody = null;

        //On réinitialise les varaibles
        linkedToPlayer = false;
        playerLinked = null;
    }
}