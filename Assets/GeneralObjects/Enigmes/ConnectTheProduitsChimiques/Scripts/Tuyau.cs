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

    //SI croix, savoir si autre direction est colorié et si oui avec quoi
    private bool Colored = false;
    private bool Colored2 = false;
    private PCTile.PCFluidColor dir1Color;
    private PCTile.PCFluidColor dir1Color2;


    public int Rotation = 0;
    private PCTile.PCFluidDirection fluidDirection;
    private PCTile.PCFluidDirection fluidDirection2;
    private PCTile.PCFluidDirection fluidCommingDirection;
    private PCTile.PCFluidDirection fluidCommingDirection2;

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
    public Sprite Cross_BG;
    public Sprite Cross_RG;
    public Sprite Cross_RB;
    public Sprite Cross_GB;
    public Sprite Cross_GR;

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
        Rotation++;
        Rotation %= 4;
        //Debug.Log("Before");
        //Debug.Log(fluidCommingDirection);
        //Debug.Log(fluidDirection);
        
        //On change les direction d'entrée et de sortie
        if (fluidCommingDirection != PCTile.PCFluidDirection.None)
        {
            if (fluidCommingDirection == PCTile.PCFluidDirection.Down)
            {
                fluidCommingDirection = PCTile.PCFluidDirection.Left;
            }
            else
            {
                fluidCommingDirection--;
            }
        }
        if (fluidDirection != PCTile.PCFluidDirection.End)
        {
            if (fluidDirection == PCTile.PCFluidDirection.Down)
            {
                fluidDirection = PCTile.PCFluidDirection.Left;
            }
            else
            {
                fluidDirection--;
            }
        }

        //SI c'est une croix, alors on fait la même chose dans l'autre direction
        if (TileData.TileType == PCTile.PCTileType.Cross)
        {
            if (fluidCommingDirection2 == PCTile.PCFluidDirection.Down)
            {
                fluidCommingDirection2 = PCTile.PCFluidDirection.Left;
            }
            else
            {
                fluidCommingDirection2--;
            }
            if (fluidDirection2 == PCTile.PCFluidDirection.Down)
            {
                fluidDirection2 = PCTile.PCFluidDirection.Left;
            }
            else
            {
                fluidDirection2--;
            }
        }

        //Debug.Log("After");
        //Debug.Log(fluidCommingDirection);
        //Debug.Log(fluidDirection);
        this.GetComponent<Transform>().Rotate(new Vector3(0, 0, 90));
        //update l'image (si connectée a fluid)
        foreach (Tuyau tuyau in Map.Tuyaux)
        {
            tuyau.AffichageUpdate();
        }
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

    public void InitaliseRotation(int coordX, int coordY)
    {
        CoordX = coordX;
        CoordY = coordY;
        switch (TileData.TileType)
        {
            case PCTile.PCTileType.Strait:
                fluidCommingDirection = PCTile.PCFluidDirection.Left;
                fluidDirection = PCTile.PCFluidDirection.Right;
                break;
            case PCTile.PCTileType.Corner:
                fluidCommingDirection = PCTile.PCFluidDirection.Up;
                fluidDirection = PCTile.PCFluidDirection.Right;
                break;
            case PCTile.PCTileType.Cross:
                fluidCommingDirection = PCTile.PCFluidDirection.Left;
                fluidDirection = PCTile.PCFluidDirection.Right;
                fluidCommingDirection2 = PCTile.PCFluidDirection.Up;
                fluidDirection2 = PCTile.PCFluidDirection.Down;
                break;
            case PCTile.PCTileType.Source:
                if (CoordY == 0)
                {
                    fluidCommingDirection = PCTile.PCFluidDirection.None;
                    fluidDirection = PCTile.PCFluidDirection.Right;
                }
                else
                {
                    fluidCommingDirection = PCTile.PCFluidDirection.Left;
                    fluidDirection = PCTile.PCFluidDirection.End;
                }
                break;
            default:
                break;
        }
    }

    public void AffichageUpdate()
    {
        switch (TileData.TileType)
        {
            case PCTile.PCTileType.Strait:
                this.GetComponent<SpriteRenderer>().sprite = Strait_Empty;
                break;
            case PCTile.PCTileType.Corner:
                this.GetComponent<SpriteRenderer>().sprite = Corner_Empty;
                break;
            case PCTile.PCTileType.Cross:
                this.GetComponent<SpriteRenderer>().sprite = Cross_Empty;
                Colored = false;
                Colored2 = false;
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
        PCTile.PCFluidDirection pCFluidDirection;
        //S'il faut mettre de la couleur
        switch (TileData.TileType)
        {
            case PCTile.PCTileType.Strait:
                //Si on est sur un tiuyaux droit
                //Si le fluid viens de la bonne direction (peut importe le sens)
                if (commingFrom == fluidCommingDirection || commingFrom == fluidDirection)
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
                    //Colored = true;
                    //Suivant le sens de circulation du fluide, le liquide sort d'un coté ou de l'autre
                    if (commingFrom == fluidCommingDirection)
                    {
                        pCFluidDirection = fluidDirection;
                    }
                    else
                    {
                        pCFluidDirection = fluidCommingDirection;
                    }
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            case PCTile.PCTileType.Corner:
                if (commingFrom == fluidCommingDirection || commingFrom == fluidDirection)
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
                    //Colored = true;
                    if (commingFrom == fluidCommingDirection)
                    {
                        pCFluidDirection = fluidDirection;
                    }
                    else
                    {
                        pCFluidDirection = fluidCommingDirection;
                    }
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            case PCTile.PCTileType.Cross:
                //Dans tt les cas le liquide va passer => question est plus en dessous ou au dessus
                //Si c'est au vers bas ou invers => au dessus
                //On recupere d'abord les infos utile pour choisir si on met la couleur en dessous
                bool isTopTube = commingFrom == fluidDirection || commingFrom == fluidCommingDirection;
                if (commingFrom == PCTile.PCFluidDirection.Down || commingFrom == PCTile.PCFluidDirection.Up)
                {
                    //on traite la couleur du dessus
                    PCTile.PCFluidColor? autreCouleur = null;
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_BN;
                            //si y a une deuxième couleur mais la varaible a check dépend de isTopTube
                            if (isTopTube && Colored2)
                            {
                                autreCouleur = dir1Color2;
                            }
                            else if (Colored)
                            {
                                autreCouleur = dir1Color;
                            }
                            if (autreCouleur != null)
                            {
                                switch (autreCouleur)
                                {
                                    case PCTile.PCFluidColor.pink:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_BR;
                                        break;
                                    case PCTile.PCFluidColor.green:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_BG;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case PCTile.PCFluidColor.pink:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_RN;
                            //si y a une deuxième couleur mais la varaible a check dépend de isTopTube
                            if (isTopTube && Colored2)
                            {
                                autreCouleur = dir1Color2;
                            }
                            else if (Colored)
                            {
                                autreCouleur = dir1Color;
                            }
                            if (autreCouleur != null)
                            {
                                switch (dir1Color2)
                                {
                                    case PCTile.PCFluidColor.blue:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_RB;
                                        break;
                                    case PCTile.PCFluidColor.green:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_RG;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case PCTile.PCFluidColor.green:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_GN;
                            //si y a une deuxième couleur mais la varaible a check dépend de isTopTube
                            if (isTopTube && Colored2)
                            {
                                autreCouleur = dir1Color2;
                            }
                            else if (Colored)
                            {
                                autreCouleur = dir1Color;
                            }
                            if (autreCouleur != null)
                            {
                                switch (dir1Color2)
                                {
                                    case PCTile.PCFluidColor.blue:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_GB;
                                        break;
                                    case PCTile.PCFluidColor.pink:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_GR;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (isTopTube)
                    {

                        Colored = true;
                        dir1Color = color;
                    }
                    else
                    {
                        Colored2 = true;
                        dir1Color2 = color;
                    }
                    if (commingFrom == PCTile.PCFluidDirection.Down)
                    {
                        pCFluidDirection = PCTile.PCFluidDirection.Up;
                    }
                    else
                    {
                        pCFluidDirection = PCTile.PCFluidDirection.Down;
                    }
                }
                else
                {
                    //Dans ce cas on est dans le tuyaux du dessous qui va de gauche a droite. on traite la couleur du dessous
                    PCTile.PCFluidColor? autreCouleur = null;
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_NB;
                            //si y a une deuxième couleur mais la varaible a check dépend de isTopTube
                            if (!isTopTube && Colored2)
                            {
                                autreCouleur = dir1Color2;
                            }
                            else if (Colored)
                            {
                                autreCouleur = dir1Color;
                            }
                            if (autreCouleur != null)
                            {
                                switch (autreCouleur)
                                {
                                    case PCTile.PCFluidColor.pink:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_RB;
                                        break;
                                    case PCTile.PCFluidColor.green:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_GB;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case PCTile.PCFluidColor.pink:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_NR;
                            //si y a une deuxième couleur mais la varaible a check dépend de isTopTube
                            if (!isTopTube && Colored2)
                            {
                                autreCouleur = dir1Color2;
                            }
                            else if (Colored)
                            {
                                autreCouleur = dir1Color;
                            }
                            if (autreCouleur != null)
                            {
                                switch (dir1Color2)
                                {
                                    case PCTile.PCFluidColor.blue:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_BR;
                                        break;
                                    case PCTile.PCFluidColor.green:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_GR;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case PCTile.PCFluidColor.green:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_NG;
                            //si y a une deuxième couleur mais la varaible a check dépend de isTopTube
                            if (!isTopTube && Colored2)
                            {
                                autreCouleur = dir1Color2;
                            }
                            else if (true)
                            {
                                autreCouleur = dir1Color;
                            }
                            if (autreCouleur != null)
                            {
                                switch (dir1Color2)
                                {
                                    case PCTile.PCFluidColor.blue:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_BG;
                                        break;
                                    case PCTile.PCFluidColor.pink:
                                        this.GetComponent<SpriteRenderer>().sprite = Cross_RG;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (!isTopTube)
                    {

                        Colored = true;
                        dir1Color = color;
                    }
                    else
                    {
                        Colored2 = true;
                        dir1Color2 = color;
                    }
                    if (commingFrom == PCTile.PCFluidDirection.Left)
                    {
                        pCFluidDirection = PCTile.PCFluidDirection.Right;
                    }
                    else
                    {
                        pCFluidDirection = PCTile.PCFluidDirection.Left;
                    }
                }
                NextTuyauxColor(pCFluidDirection, color);
                break;
            case PCTile.PCTileType.Source:
                if (commingFrom == fluidCommingDirection)
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
                    //Colored = true;
                    pCFluidDirection = fluidDirection;
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
                    //On renvoie une direction différente parce que l'ont veux savoir d'où arrive le liquide sur la case
                    Map.TuyauxMaze[CoordX][CoordY + 1].ColorUpdate(PCTile.PCFluidDirection.Up, color);
                }
                break;
            case PCTile.PCFluidDirection.Left:
                if (CoordX - 1 >= 0 && Map.TuyauxMaze[CoordX - 1][CoordY] != null)
                {
                    Map.TuyauxMaze[CoordX - 1][CoordY].ColorUpdate(PCTile.PCFluidDirection.Right, color);
                }
                break;
            case PCTile.PCFluidDirection.Up:
                if (CoordY - 1 >= 0 && Map.TuyauxMaze[CoordX][CoordY - 1] != null)
                {
                    Map.TuyauxMaze[CoordX][CoordY - 1].ColorUpdate(PCTile.PCFluidDirection.Down, color);
                }
                break;
            case PCTile.PCFluidDirection.Right:
                if (CoordX + 1 < Map.TuyauxMaze.Length && Map.TuyauxMaze[CoordX + 1][CoordY] != null)
                {
                    Map.TuyauxMaze[CoordX + 1][CoordY].ColorUpdate(PCTile.PCFluidDirection.Left, color);
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
