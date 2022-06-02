using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
    public Sprite Cross_BB;
    public Sprite Cross_BR;
    public Sprite Cross_BG;
    public Sprite Cross_RB;
    public Sprite Cross_RR;
    public Sprite Cross_RG;
    public Sprite Cross_GB;
    public Sprite Cross_GR;
    public Sprite Cross_GG;

    /*
     * Fonctions
     */

    //Appelée a chaque frame
    private void Update()
    {
        //check si on appuie sur E ssi la plaque est préssée
        if (pressed && Input.GetKeyDown(KeyCode.R) && TileData.TileType != PCTile.PCTileType.Cross)
        {
            //teléporte les joueurs
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Rotate", RpcTarget.All);
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
        if (TileData.TileType != PCTile.PCTileType.Cross) //si c'est une croix on peut pas le bouger
        {
            //On affiche le message qui indique au joueur comment intéragir avec la porte.
            MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press R to rotate the pipe");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TileData.TileType != PCTile.PCTileType.Cross) //si c'est une croix on peut pas le bouger
        {
            //SI on fait pas ça le message part dès qu'il quitte une autre boite donc obligé
            //On affiche le message qui indique au joueur comment intéragir avec la porte.
            MessageOnScreenCanvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press R to rotate the pipe");
        }
    }

    /**
     * <summary>Permet de rotation l'asset et de changer les variables qui se basent sur sa rotation pour faire passer ou non le liquide</summary>
     * 
     * A chaque appel, remet tous les tuyaux a 0 et recalcule la trajectoire du liquide
     */
    [PunRPC]
    public void Rotate()
    {
        Rotation++;
        Rotation %= 4;

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

        //si c'était bon alors on revérouille la porte en attendant de voir si c encore bon
        if (Map.nbPipeOk == 3)
        {
            Map.EndOfGame(false);
        }
        //Remise a zéro du nombre de tuyaux reliés à la fin
        Map.nbPipeOk = 0;

        this.GetComponent<Transform>().Rotate(new Vector3(0, 0, 90));
        //update l'image (si connectée a fluid)
        //remise a 0 des tuayux (tous)
        foreach (Tuyau tuyau in Map.Tuyaux)
        {
            tuyau.AffichageUpdate();
        }

        //actualise du passage du liquide dans les tuyaux
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

    /**
     * <summary>Permet d'initialiser les variables de rotation des tuayux (au lancement du jeu uniquement)</summary>
     * 
     * <param name="coordX">Coordonée X du tuayau</param>
     * <param name="coordY">Coord Y du tuyau</param>
     * 
     * Par rapport à la position initiale des assets
     */
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
                    fluidCommingDirection = PCTile.PCFluidDirection.Right;
                    fluidDirection = PCTile.PCFluidDirection.End;
                }
                break;
            default:
                break;
        }
    }

    /**
     * <summary>Remet l'affichage à 0 (vide tous les tuayux de leur contenu)</summary>
     */
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

    /**
     * <summary>Ajoute le liquide dans le tuayux actuel (si il viens de la bonne direction) (et si le liquide passe, appel la prochaine case pour une éventuelle actualisation)</summary>
     * 
     * <param name="commingFrom">Sens d'où provient le liquide</param>
     * <param name="color">Couleur en cours de traitement</param>
     */
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
                    //Suivant le sens de circulation du fluide, le liquide sort d'un coté ou de l'autre
                    if (commingFrom == fluidCommingDirection)
                    {
                        pCFluidDirection = fluidDirection;
                    }
                    else
                    {
                        pCFluidDirection = fluidCommingDirection;
                    }
                    //Appel du prochain tuyau
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            case PCTile.PCTileType.Corner:
                //Si on est dans un tuyaux coin
                //Si le fluid viens de la bonne direction (peut importe le sens)
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
                    //Suivant le sens de circulation du fluide, le liquide sort d'un coté ou de l'autre
                    if (commingFrom == fluidCommingDirection)
                    {
                        pCFluidDirection = fluidDirection;
                    }
                    else
                    {
                        pCFluidDirection = fluidCommingDirection;
                    }
                    //Appel du prochain tuyau
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            case PCTile.PCTileType.Cross:
                //Dans tt les cas le liquide va passer => question est juste de mettre le bon sprite
                if (commingFrom == PCTile.PCFluidDirection.Down || commingFrom == PCTile.PCFluidDirection.Up)
                {
                    //on traite la couleur du dessus
                    //On est donc avec les varaiables 2 d'utilisation
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_BN;
                            //si y a une deuxième couleur (dans la premiere variable car on est sur la deuxième actuellement)
                            if (Colored)
                            {
                                if (dir1Color == PCTile.PCFluidColor.blue)
                                {
                                    this.GetComponent<SpriteRenderer>().sprite = Cross_BB;
                                }
                                switch (dir1Color)
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
                            //si y a une deuxième couleur (dans la premiere variable car on est sur la deuxième actuellement)
                            if (Colored)
                            {
                                if (dir1Color == PCTile.PCFluidColor.pink)
                                {
                                    this.GetComponent<SpriteRenderer>().sprite = Cross_RR;
                                }
                                switch (dir1Color)
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
                            //si y a une deuxième couleur (dans la premiere variable car on est sur la deuxième actuellement)
                            if (Colored)
                            {
                                if (dir1Color == PCTile.PCFluidColor.green)
                                {
                                    this.GetComponent<SpriteRenderer>().sprite = Cross_GG;
                                }
                                switch (dir1Color)
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
                    //On update les valeur des deuxième variable (on était bien sur les deuxième variable car les croix ne peuvent pas tourner et c défini comme ça plus haut)
                    Colored2 = true; // sa couleur est affichée
                    dir1Color2 = color; // et sa couleur est color
                }
                else
                {
                    //Dans ce cas on est dans le tuyaux du dessous qui va de gauche a droite. on traite la couleur du dessous
                    switch (color)
                    {
                        case PCTile.PCFluidColor.blue:
                            this.GetComponent<SpriteRenderer>().sprite = Cross_NB;
                            //on traite la couleur du deossous
                            //On est donc avec les varaiables 1 d'utilisation
                            //si y a une deuxième couleur (dans la deuxième variable car on est sur la premiere actuellement)
                            if (Colored2)
                            {
                                if (dir1Color2 == PCTile.PCFluidColor.blue)
                                {
                                    this.GetComponent<SpriteRenderer>().sprite = Cross_BB;
                                }
                                switch (dir1Color2)
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
                            //si y a une deuxième couleur (dans la deuxième variable car on est sur la premiere actuellement)
                            if (Colored2)
                            {
                                if (dir1Color2 == PCTile.PCFluidColor.pink)
                                {
                                    this.GetComponent<SpriteRenderer>().sprite = Cross_RR;
                                }
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
                            //si y a une deuxième couleur (dans la deuxième variable car on est sur la premiere actuellement)
                            if (Colored2)
                            {
                                if (dir1Color2 == PCTile.PCFluidColor.green)
                                {
                                    this.GetComponent<SpriteRenderer>().sprite = Cross_GG;
                                }
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
                    //On update les valeur des premier variable (on était bien sur les premier variable car les croix ne peuvent pas tourner et c défini comme ça plus haut)
                    Colored = true; // sa couleur est affichée
                    dir1Color = color; // et sa couleur est color

                }

                //direction de sortie du fluide (opposée à celle d'entrée)
                if (commingFrom == PCTile.PCFluidDirection.Down) 
                {
                    pCFluidDirection = PCTile.PCFluidDirection.Up;
                }
                else if (commingFrom == PCTile.PCFluidDirection.Up)
                {
                    pCFluidDirection = PCTile.PCFluidDirection.Down;
                }
                else if (commingFrom == PCTile.PCFluidDirection.Left)
                {
                    pCFluidDirection = PCTile.PCFluidDirection.Right;
                }
                else
                {
                    pCFluidDirection = PCTile.PCFluidDirection.Left;
                }

                //Appel prochain tuyau
                NextTuyauxColor(pCFluidDirection, color);
                break;
            case PCTile.PCTileType.Source:
                //Si on est sur un tuyaux source
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

                    //on appelle avec les direction de sortie (potentiellement END si c une source receveuse)
                    pCFluidDirection = fluidDirection;
                    NextTuyauxColor(pCFluidDirection, color);
                }
                break;
            default:
                break;
        }
    }

    /**
     * <summary>Fait changer la couleur du tuayux suivant ssi il existe + gére le cas de fin</summary>
     * 
     * <param name="pCFluidDirection">Direction de sortie du liquide du tuayux actuel</param>
     * <param name="color">couleur en cours de traitement</param>
     */
    private void NextTuyauxColor(PCTile.PCFluidDirection pCFluidDirection, PCTile.PCFluidColor color)
    {
        //A chaque fois on va inverser la direction du tuayux parce que on a en parametre la direction de sortie de CE tuyaux et qu'on veut la direction d'entre du tuyau PROCHAIN
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
                //c bon pour un tuayux (le liquide arrive à la fin
                Map.nbPipeOk++;
                //si c bon pour les trois tuayux
                if (Map.nbPipeOk == 3)
                {
                    //le jeux est finit ! 
                    Map.EndOfGame();
                }
                break;
            default:
                break;
        }
    }
}
