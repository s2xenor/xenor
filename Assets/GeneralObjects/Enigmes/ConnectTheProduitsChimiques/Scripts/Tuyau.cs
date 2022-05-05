using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuyau : PressurePlate
{
    public PCTile TileData = new PCTile();

    /**
     * Sprites pour tuyaux
     */
    //Sources
    public Sprite Source_Empty;
    public Sprite Source_Rose;
    public Sprite Source_Blue;
    public Sprite Source_Green;

    //Strait
    public Sprite Strait_Empty;
    public Sprite Strait_Rose;
    public Sprite Strait_Blue;
    public Sprite Strait_Green;


    //Corner
    public Sprite Corner_Empty;
    public Sprite Corner_Rose;
    public Sprite Corner_Blue;
    public Sprite Corner_Green;

    //Cross
    public Sprite Cross_Empty;
    public Sprite Cross_RN;
    public Sprite Cross_BN;
    public Sprite Cross_GN;
    public Sprite Cross_NR;
    public Sprite Cross_NB;
    public Sprite Cross_NG;
    public Sprite Cross_BR;
    public Sprite Cross_RG;
    public Sprite Cross_GB;

    /*
     * Fonctions
     */

    //Appelée a chaque frame
    private void Update()
    {
        //check si on appuie sur E ssi la plaque est préssée
        if (pressed && Input.GetKeyDown(KeyCode.R))
        {
            //teléporte les joueurs
            Rotate();
        }
    }

    //Fonction OnPressure() appellée si un player marche sur le tuyaux, elle affiche le message pour demander une intéraction
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
        MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press R to rotate the pipe");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //SI on fait pas ça le message part dès qu'il quitte une autre boite donc obligé
        //On affiche le message qui indique au joueur comment intéragir avec la porte.
        MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press R to rotate the pipe");
    }

    private void Rotate()
    {
        TileData.Rotation++;
        if (TileData.Rotation == 4)
        {
            TileData.Rotation = 0;
        }
        this.GetComponent<Transform>().Rotate(new Vector3(0, 0, 90));
        //update l'image (si connectée a fluid)
    }

    public void AffichageUpdate()
    {
        switch (TileData.TileType)
        {
            case PCTile.PCTileType.None:
                throw new System.Exception("y a un pb");
                break;
            case PCTile.PCTileType.Strait:
                this.GetComponent<SpriteRenderer>().sprite = Strait_Empty;
                break;
            case PCTile.PCTileType.Corner:
                this.GetComponent<SpriteRenderer>().sprite = Corner_Empty;
                break;
            case PCTile.PCTileType.Cross:
                this.GetComponent<SpriteRenderer>().sprite = Cross_Empty;
                break;
            case PCTile.PCTileType.Source:
                this.GetComponent<SpriteRenderer>().sprite = Source_Empty;
                break;
            default:
                throw new System.Exception("y a un pb");
                break;
        }
    }
}
