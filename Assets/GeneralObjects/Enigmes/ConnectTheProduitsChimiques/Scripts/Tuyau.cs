using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuyau : PressurePlate
{
    public PCTile TileData = new PCTile();

    public PCMap Map;
    public int CoordX;
    public int CoordY;

    //public Tuyau Suivant;
    //public Tuyau Precedent; peut être pas besoin (si on appelle le suivant juste si faut mettre la couleur ? avec les direction d'où elle arrive

    public bool Colored = false;
    public bool Colored2 = false;

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
        int numeroSource = 0;
        foreach ((int, int) coords in Map.StartsAndEnds)
        {
            //(y,x)
            if (coords.Item1 == -1)
            {
                Map.TuyauxMaze[coords.Item2][0].GetComponent<Tuyau>().ColorUpdate(PCTile.PCFluidDirection.None, (PCTile.PCFluidColor)numeroSource);
                numeroSource++;
            }
        }
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
        }
    }

    public void ColorUpdate(PCTile.PCFluidDirection commingFrom, PCTile.PCFluidColor color)
    {
        //S'il faut mettre de la couleur
        switch (TileData.TileType)
        {
            case PCTile.PCTileType.None:
                throw new System.Exception("y a un pb");
            case PCTile.PCTileType.Strait:
                //Si on est sur un tiuyaux droit
                //Si le fluid viens de la bonne direction
                if (commingFrom == TileData.FluidCommingDirection)
                {
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Strait_Blue;
                            break;
                        case PCTile.PCFluidColor.pink:
                            this.GetComponent<SpriteRenderer>().sprite = Strait_Rose;
                            break;
                        case PCTile.PCFluidColor.green:
                            this.GetComponent<SpriteRenderer>().sprite = Strait_Green;
                            break;
                        default:
                            break;
                    }
                    Colored = true;
                    PCTile.PCFluidDirection pCFluidDirection = TileData.FluidDirection;
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            case PCTile.PCTileType.Corner:
                if (commingFrom == TileData.FluidCommingDirection)
                {
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Corner_Blue;
                            break;
                        case PCTile.PCFluidColor.pink:
                            this.GetComponent<SpriteRenderer>().sprite = Corner_Rose;
                            break;
                        case PCTile.PCFluidColor.green:
                            this.GetComponent<SpriteRenderer>().sprite = Corner_Green;
                            break;
                        default:
                            break;
                    }
                    Colored = true;
                    PCTile.PCFluidDirection pCFluidDirection = TileData.FluidDirection;
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            case PCTile.PCTileType.Cross:
                this.GetComponent<SpriteRenderer>().sprite = Cross_Empty;
                break;
            case PCTile.PCTileType.Source:
                Debug.Log(TileData.FluidCommingDirection);
                Debug.Log(commingFrom);
                if (commingFrom == TileData.FluidCommingDirection)
                {
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Source_Blue;
                            break;
                        case PCTile.PCFluidColor.pink:
                            this.GetComponent<SpriteRenderer>().sprite = Source_Rose;
                            break;
                        case PCTile.PCFluidColor.green:
                            this.GetComponent<SpriteRenderer>().sprite = Source_Green;
                            break;
                        default:
                            break;
                    }
                    Colored = true;
                    PCTile.PCFluidDirection pCFluidDirection = TileData.FluidDirection;
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            default:
                throw new System.Exception("y a un pb");
        }
    }

    private void NextTuyauxColor(PCTile.PCFluidDirection pCFluidDirection, PCTile.PCFluidColor color)
    {
        switch (pCFluidDirection)
        {
            case PCTile.PCFluidDirection.Down:
                if (CoordY + 1 < Map.TuyauxMaze[0].Length && Map.TuyauxMaze[CoordX][CoordY + 1] != null)
                {
                    Map.TuyauxMaze[CoordX][CoordY + 1].ColorUpdate(pCFluidDirection, color);
                }
                break;
            case PCTile.PCFluidDirection.Left:
                if (CoordX - 1 >= 0 && Map.TuyauxMaze[CoordX - 1][CoordY] != null)
                {
                    Map.TuyauxMaze[CoordX - 1][CoordY].ColorUpdate(pCFluidDirection, color);
                }
                break;
            case PCTile.PCFluidDirection.Up:
                if (CoordY - 1 >= 0 && Map.TuyauxMaze[CoordX][CoordY - 1] != null)
                {
                    Map.TuyauxMaze[CoordX][CoordY - 1].ColorUpdate(pCFluidDirection, color);
                }
                break;
            case PCTile.PCFluidDirection.Right:
                if (CoordX + 1 < Map.TuyauxMaze[0].Length && Map.TuyauxMaze[CoordX + 1][CoordY] != null)
                {
                    Map.TuyauxMaze[CoordX + 1][CoordY].ColorUpdate(pCFluidDirection, color);
                }
                break;
            case PCTile.PCFluidDirection.End:
                //TODO : dire que c bon pour 1
                break;
            default:
                break;
        }
    }
}
