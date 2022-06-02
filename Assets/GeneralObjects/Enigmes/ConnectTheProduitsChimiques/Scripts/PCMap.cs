using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCMap : MonoBehaviourPunCallbacks
{
    /**
     * Variables Publiques
     */
    //Prefabs
    public GameObject tuyau;
    public GameObject vitre;
    //Canvas
    public GameObject canvaTextPopUP;
    
    public int nbPipeOk = 0;

    public int MapSize = 10;

    public string NextSceneName = "MainRoom";

    /**
     * Prefabs
     */
    public GameObject wall_top;
    public GameObject wall_left;
    public GameObject wall_right;
    public GameObject wall_down;
    public GameObject corner_top_left;
    public GameObject corner_top_right;
    public GameObject corner_bottom_left;
    public GameObject corner_bottom_right;
    public GameObject floor;
    public GameObject left_door_design;
    public GameObject top_door_design;
    public GameObject top_door_activated_design;
    public GameObject single_door;
    public GameObject playerPrefab;
    /**
     * Variables Priv�es
     */
    private PCMazeGenerator mazeGenerator;
    public List<(int,int)> StartsAndEnds => mazeGenerator.StartsAndEnds;

    public List<Tuyau> Tuyaux = new List<Tuyau>();
    
    private Tuyau[][] tuyauxMaze;
    public Tuyau[][] TuyauxMaze => tuyauxMaze;


    GameObject[] gos;
    private bool screenNeedDelete = true;
    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(0, 0), Quaternion.identity); // Spawn master player on network
            StartGeneration();
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(0, 0), Quaternion.identity); // Spawn player on network
        }

    }

    void Update()
    {
        gos = GameObject.FindGameObjectsWithTag("Loading");

        // Delete loading screen
        if (screenNeedDelete && GameObject.FindGameObjectsWithTag("Player").Length == 2 && gos.Length != 0 || true)
        {
            screenNeedDelete = false;
            foreach (GameObject go in gos)
                go.GetComponent<FetchCam>().Del();
        }

        //Cheat code to enlock door
        if (Input.GetKeyDown(KeyCode.P))
        {
            EndOfGame();
        }
    }

    // Start is called before the first frame update
    public void StartGeneration()
    {
        mazeGenerator = new PCMazeGenerator(MapSize);
        PhotonView photonView = PhotonView.Get(this);

        //Ajout des tuyaux solitaires (qui peuvent potentiellemnt faire des chemin alternatifs mais servent surtout � augmenter le difficult�e de l'�nigme
        for (int i = 0; i < mazeGenerator.Maze.Length; i++)
        {
            for (int j = 0; j < mazeGenerator.Maze[0].Length; j++)
            {
                if (mazeGenerator.Maze[i][j].TileType == PCTile.PCTileType.None)
                {
                    int nbalea = Random.Range(0, 6);
                    switch (nbalea)
                    {
                        case 0:
                            mazeGenerator.Maze[i][j].GlobalAddDirection(PCTile.PCFluidDirection.Left, PCTile.PCFluidDirection.Right);
                            break;
                        case 1:
                            mazeGenerator.Maze[i][j].GlobalAddDirection(PCTile.PCFluidDirection.Left, PCTile.PCFluidDirection.Up);
                            break;
                        case 2:
                            mazeGenerator.Maze[i][j].GlobalAddDirection(PCTile.PCFluidDirection.Left, PCTile.PCFluidDirection.Right);
                            mazeGenerator.Maze[i][j].GlobalAddDirection(PCTile.PCFluidDirection.Down, PCTile.PCFluidDirection.Up);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Initialisation du tableau de tuyaux
        tuyauxMaze = new Tuyau[mazeGenerator.MapSize][];
        int tabTaille = mazeGenerator.MapSize + 2;
        for (int i = 0; i < mazeGenerator.MapSize; i++)
        {
            tuyauxMaze[i] = new Tuyau[tabTaille];
        }

        //Instantie de toutes les tiles avec la bonne image
        for (int coordY = 0; coordY < mazeGenerator.MapSize; coordY++)
        {
            //bord gauche et droit
            PhotonNetwork.Instantiate(vitre.name, new Vector3((float)0.32 * -1 - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(vitre.name, new Vector3((float)0.32 * mazeGenerator.MapSize - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
            //contenu
            for (int coordX = 0; coordX < mazeGenerator.MapSize; coordX++)
            {
                PCTile tile = mazeGenerator.Maze[coordY][coordX];
                PhotonNetwork.Instantiate(vitre.name, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
                if (tile.TileType != PCTile.PCTileType.None)
                {
                    photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coordX, coordY, coordY);
                }
            }
        }
        //bord bas et bas
        for (int coordX = -1; coordX <= mazeGenerator.MapSize; coordX++)
        {
            PhotonNetwork.Instantiate(vitre.name, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * -1 + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(vitre.name, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * mazeGenerator.MapSize + (float)0.16, 0), Quaternion.identity);
        }


        int numeroSource = 0;
        //placement des sources et des arriv�es
        foreach ((int,int) coords in mazeGenerator.StartsAndEnds)
        {
            Tuyau pipe = PhotonNetwork.Instantiate(tuyau.name, new Vector3((float)0.32 * coords.Item2 - (float)0.16, (float)0.32 * coords.Item1 + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
            if (coords.Item1 == -1)
            {
                photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coords.Item2, coords.Item1, 0);
                pipe.TileData = new PCTile(PCTile.PCTileType.Source, PCTile.PCFluidDirection.Down);
                tuyauxMaze[coords.Item2][0] = pipe;
                pipe.MessageOnScreenCanvas = canvaTextPopUP;
                pipe.AffichageUpdate();
                pipe.InitaliseRotation(coords.Item2, 0);
                pipe.ColorUpdate(PCTile.PCFluidDirection.None, (PCTile.PCFluidColor)numeroSource);
                numeroSource++;
            }
            else
            {
                photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coords.Item2, coords.Item1, mazeGenerator.MapSize);
                pipe.TileData = new PCTile(PCTile.PCTileType.Source, PCTile.PCFluidDirection.Up);
                tuyauxMaze[coords.Item2][mazeGenerator.MapSize+1] = pipe;
                pipe.InitaliseRotation(coords.Item2, mazeGenerator.MapSize);
                pipe.MessageOnScreenCanvas = canvaTextPopUP;
                pipe.AffichageUpdate();
                Tuyaux.Add(pipe);
            }
        }

        //Instancie le sol
        for (int i = -3; i < mazeGenerator.MapSize + 3; i++)
        {
            for (int j = -3; j < mazeGenerator.MapSize + 3; j++)
            {
                if (j==-1 && i >= -1 && i <= mazeGenerator.MapSize)
                {
                    j = mazeGenerator.MapSize + 1;
                }
                PhotonNetwork.Instantiate(floor.name, new Vector3((float)0.32 * i - (float)0.16, (float)0.32 * j + (float)0.16, 0), Quaternion.identity);
            }
        }

        //Instantie les murs
        for (int j = -3; j < mazeGenerator.MapSize + 3; j++)
        {
            PhotonNetwork.Instantiate(wall_left.name, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * j + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(wall_right.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 3) - (float)0.16, (float)0.32 * j + (float)0.16, 0), Quaternion.identity);
        }
        for (int i= -3; i < mazeGenerator.MapSize + 3; i++)
        {
            PhotonNetwork.Instantiate(wall_down.name, new Vector3((float)0.32 * i - (float)0.16, (float)0.32 * -4 + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(wall_top.name, new Vector3((float)0.32 * i - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        }
        PhotonNetwork.Instantiate(corner_bottom_left.name, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * -4 + (float)0.16, 0), Quaternion.identity);
        PhotonNetwork.Instantiate(corner_top_left.name, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        PhotonNetwork.Instantiate(corner_bottom_right.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 3) - (float)0.16, (float)0.32 * -4 + (float)0.16, 0), Quaternion.identity);
        PhotonNetwork.Instantiate(corner_top_right.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 3) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);

        // Instantie porte
        //Design
        PhotonNetwork.Instantiate(left_door_design.name, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * -2 + (float)0.16, 0), Quaternion.identity);
        PhotonNetwork.Instantiate(top_door_design.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        PhotonNetwork.Instantiate(top_door_activated_design.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        //Plaque de pression
        PhotonNetwork.Instantiate(single_door.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 2) + (float)0.16, 0), Quaternion.identity);
        GameObject.FindGameObjectWithTag("Door").GetComponent<SingleDoor>().MessageOnScreenCanvas = canvaTextPopUP;

        //Quand jeu est fini il faut lier la salle suivante et afficher les portes

        //Prochaine salle dans porte


    }

    [PunRPC]
    public void GlobalInstantiatePipe(int coX, int coY, int secondY)
    {
        Tuyau pipe = PhotonNetwork.Instantiate(tuyau.name, new Vector3((float)0.32 * coX - (float)0.16, (float)0.32 * coY + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
        pipe.TileData = tile;
        pipe.MessageOnScreenCanvas = canvaTextPopUP;
        pipe.AffichageUpdate();
        pipe.Map = this;
        pipe.InitaliseRotation(coX, secondY + 1);
        Tuyaux.Add(pipe);
        tuyauxMaze[coX][coY + 1] = pipe;
    }
    /**
     * <summary>Cette fonction permet de v�rouiller ou de d�verouiller la porte de sortie</summary>
     */
    public void EndOfGame(bool finish = true)
    {
        GameObject.FindGameObjectWithTag("DoorsToActivate").GetComponent<SpriteRenderer>().enabled = finish;
    }
}
