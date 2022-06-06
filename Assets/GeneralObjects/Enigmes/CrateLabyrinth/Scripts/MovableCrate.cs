using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovableCrate : PressurePlate
{
    /*
     * Vraiables Priv�es
     */

    private bool linkedToPlayer = false;

    private GameObject playerLinked;

    GameObject canvasPopUp;

    /*
     * Fonctions
     */

    private void Start()
    {
        MessageOnScreenCanvas = GameObject.FindGameObjectWithTag("CanvasText");
        canvasPopUp = GameObject.FindGameObjectWithTag("CanvasText");
    }

    //Appel�e a chaque frame
    private void Update()
    {
        //check si on appuie sur E ssi en contact avec la boite est pr�ss�e
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool mycoll = false; // Check if my player is next to the box

            for (int i = 0; i < player.Count && !mycoll; i++)
            {
                mycoll = player[i].GetComponent<PhotonView>().IsMine;

                if (i == 1)
                    (player[0], player[1]) = (player[1], player[0]); // Change order as crates get linked to the first one
            }

            //si qqn entre en contact avec la boite ET que la boite n'est pas d�j� li�e � qqn
            if (mycoll && pressed && !linkedToPlayer)
            {
                // on la lie au perso
                photonView.RPC("UpdateLink", RpcTarget.All, player[0].gameObject.GetPhotonView().ViewID);
            }
            //sinon, si la boite est d�j� li�e � ce qqn, on lui fais l�cher la boite
            else if (linkedToPlayer && playerLinked.GetPhotonView().IsMine)
            {
             //   UnLink();
                photonView.RPC("UpdateLink", RpcTarget.All, 0);
            }
        }
    }

    /**
    * <summary>Affiche le message pour proposer � l'utilisateur d'int�ragir</summary>
    * 
    * <param name="other">objet avec qui on a collision� (ici le joueur)</param>
    * 
    * <returns>Return nothing</returns>
    */
    protected override void OnPressure(Collider2D other)
    {
        //On affiche le message qui indique au joueur comment int�ragir avec la porte.
        //Grace � fonctionnalit� lock le message ne s'affichera que si c pas lock
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
            MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to start pulling the box");
    }

    /**
    * <summary>Lier l'objet avec le premier player rentr� en collision avec l'objet</summary>
    * 
    * <returns>Return nothing</returns>
    */
    private void Link(int id)
    {
        //le premier joueur qui est entr� en collision avec la boite stock� dans PressurePlate.cs
        playerLinked = PhotonView.Find(id).gameObject;

        //on r�cupere le component qui permet de faire le lien
        FixedJoint2D lienActuel = playerLinked.GetComponent<FixedJoint2D>();
        //On stocke la position de la boite et du joueur
        //Cela permet d'�viter que le point d'ancrage fasse bouger la boite est le joueur au moment de l'attache
        Vector2 posBox = this.transform.position;
        Vector2 posPlayer = playerLinked.transform.position;
        //On active le component
        lienActuel.enabled = true;
        lienActuel.anchor = posBox;
        lienActuel.connectedAnchor = posPlayer;
        //On lie au component le rigidbody de la boite
        lienActuel.connectedBody = this.GetComponent<Rigidbody2D>();

        //on indique que le lien a �t� �tabli avec succ�s
        linkedToPlayer = true;
    }

    /**
    * <summary>D�lier l'objet du player avec lequel il �tait li�</summary>
    * 
    * <returns>Return nothing</returns>
    */
    private void UnLink()
    {
        //on r�cupere le component qui permet de faire le lien
        FixedJoint2D lienActuel = playerLinked.GetComponent<FixedJoint2D>();
        //On le desactive
        lienActuel.enabled = false;
        //On r�initialise le lien
        lienActuel.connectedBody = null;

        //On r�initialise les varaibles
        linkedToPlayer = false;
        playerLinked = null;
    }

    /**
    * <summary>Move crate where it is supposed to be</summary>
    * 
    * <returns>Return nothing</returns>
    */

    /**
    * <summary>Update linekedToPlayer</summary>
    * 
    * <returns>Return nothing</returns>
    */
    [PunRPC]
    void UpdateLink(int id)
    {
        if (linkedToPlayer)
            UnLink();
        else
            Link(id);
    }
}