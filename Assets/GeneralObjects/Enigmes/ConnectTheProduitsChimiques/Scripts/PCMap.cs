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

    public int MapSize = 5; //tutos size

    private bool isLevelFinished = false;

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
    public GameObject playerBoyPrefab;
    public GameObject playerGirlPrefab;
    /**
     * Variables Priv�es
     */
    private PCMazeGenerator mazeGenerator;
    public List<(int, int)> StartsAndEnds => mazeGenerator.StartsAndEnds;

    public List<Tuyau> Tuyaux = new List<Tuyau>();

    private Tuyau[][] tuyauxMaze;
    public Tuyau[][] TuyauxMaze => tuyauxMaze;

    private bool shouldStartGeneration = false;

    GameObject[] gos;
    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerBoyPrefab.name, new Vector2(-0.7f, -0.3f), Quaternion.identity); // Spawn master player on network
        }
        else
        {
            mazeGenerator = new PCMazeGenerator(0, this);
            PhotonNetwork.Instantiate(playerGirlPrefab.name, new Vector2(-1f, -0.3f), Quaternion.identity); // Spawn player on network
        }
    }

    void Update()
    {



        if (shouldStartGeneration && GameObject.FindGameObjectsWithTag("Player").Length == 2) // Wait for the 2 players and then the master spawns it
        {
            if (PhotonNetwork.IsMasterClient)
            {
                shouldStartGeneration = false;
                StartGeneration();
            }
        }


    }

    public void ShouldStartGeneration() => shouldStartGeneration = true;

    [PunRPC]
    public void GenerationFinished()
    {
        Invoke("GenerationFinished2", 0.5f);
    }

    private void GenerationFinished2()
    {
        GameObject.FindGameObjectWithTag("Loading").GetComponent<FetchCam>().Del();
    }


    [PunRPC]
    public void StartDialogue(bool local = false)
    {
        if (local) this.GetComponentInParent<DialogueTriggerG>().triggerOnload = true;
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("StartDialogue", RpcTarget.All, true);
        }
    }



    // Start is called before the first frame update
    public void StartGeneration()
    {
        mazeGenerator = new PCMazeGenerator(MapSize, this);
        PhotonView photonView = PhotonView.Get(this);

        for (int i = 0; i < mazeGenerator.startsAndEnds.Count; i++)
        {
            photonView.RPC("GlobalStartsAndEnds", RpcTarget.Others, mazeGenerator.startsAndEnds[i].Item1, mazeGenerator.startsAndEnds[i].Item2);

        }

        photonView.RPC("GlobalStartAfterGen", RpcTarget.All, mazeGenerator.MapSize);

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
                            mazeGenerator.Maze[i][j].AddDirection(PCTile.PCFluidDirection.Left, PCTile.PCFluidDirection.Right);
                            break;
                        case 1:
                            mazeGenerator.Maze[i][j].AddDirection(PCTile.PCFluidDirection.Left, PCTile.PCFluidDirection.Up);
                            break;
                        case 2:
                            mazeGenerator.Maze[i][j].AddDirection(PCTile.PCFluidDirection.Left, PCTile.PCFluidDirection.Right);
                            mazeGenerator.Maze[i][j].AddDirection(PCTile.PCFluidDirection.Down, PCTile.PCFluidDirection.Up);
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
                    if (tile.FluidDirection2 != PCTile.PCFluidDirection.None) 
                    {
                        photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coordX, coordY, coordY+1, (int)tile.TileType, (int)tile.FluidDirection, 0, (int)tile.FluidDirection2);
                    }
                    else
                    {
                        photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coordX, coordY, coordY+1, (int)tile.TileType, (int)tile.FluidDirection, 1, -1);
                    }
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
        foreach ((int, int) coords in mazeGenerator.StartsAndEnds)
        {
            if (coords.Item1 == -1)
            {
                photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coords.Item2, coords.Item1, 0, (int)PCTile.PCTileType.Source, (int)PCTile.PCFluidDirection.Down, 2, numeroSource); //source
                numeroSource++;
            }
            else
            {
                photonView.RPC("GlobalInstantiatePipe", RpcTarget.All, coords.Item2, coords.Item1, mazeGenerator.MapSize+1, (int)PCTile.PCTileType.Source, (int)PCTile.PCFluidDirection.Up, 3, -1);

            }
        }

        //Instancie le sol
        for (int i = -3; i < mazeGenerator.MapSize + 3; i++)
        {
            for (int j = -3; j < mazeGenerator.MapSize + 3; j++)
            {
                if (j == -1 && i >= -1 && i <= mazeGenerator.MapSize)
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
        for (int i = -3; i < mazeGenerator.MapSize + 3; i++)
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
        //PhotonNetwork.Instantiate(top_door_activated_design.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        photonView.RPC("SetupDoorExit", RpcTarget.All, false);

        //Plaque de pression
        PhotonNetwork.Instantiate(single_door.name, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 2) + (float)0.16, 0), Quaternion.identity);
        //GameObject.FindGameObjectWithTag("Door").GetComponent<SingleDoor>().MessageOnScreenCanvas = canvaTextPopUP;

        //Quand jeu est fini il faut lier la salle suivante et afficher les portes

        //Prochaine salle dans porte

        photonView.RPC("GenerationFinished", RpcTarget.All);
    }

    [PunRPC]
    public void SetupDoorExit(bool active)
    {
        GameObject t = Instantiate(top_door_activated_design, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        t.tag = "DoorsToActivate";
        t.SetActive(active);
    }

    [PunRPC]
    public void GlobalInstantiatePipe(int coX, int coY, int secondY, int source, int direction, int from, int val)
    {
        PCTile tile;
        if (from == 0) tile = new PCTile((PCTile.PCTileType)source, (PCTile.PCFluidDirection)direction, (PCTile.PCFluidDirection)val);  //multi direction
        else tile = new PCTile((PCTile.PCTileType)source, (PCTile.PCFluidDirection)direction);

        Tuyau pipe = Instantiate(tuyau, new Vector3((float)0.32 * coX - (float)0.16, (float)0.32 * coY + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
        pipe.TileData = tile;
        pipe.MessageOnScreenCanvas = canvaTextPopUP;
        pipe.AffichageUpdate();
        pipe.Map = this;
        pipe.InitaliseRotation(coX, secondY);

        if (from == 2) pipe.ColorUpdate(PCTile.PCFluidDirection.None, (PCTile.PCFluidColor)val); //source
        else Tuyaux.Add(pipe); 

        if(from == 3) tuyauxMaze[coX][secondY] = pipe; //exit
        else tuyauxMaze[coX][secondY] = pipe;
    }

    [PunRPC]
    public void GlobalStartAfterGen(int size){
        //Initialisation du tableau de tuyaux
        tuyauxMaze = new Tuyau[size][];
        int tabTaille = size + 2;
        for (int i = 0; i<size; i++)
        {
            tuyauxMaze[i] = new Tuyau[tabTaille];
        }
    }

    [PunRPC]
    public void GlobalRotatate(int coX, int coY, bool resend = false)
    {
        if (resend)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("GlobalRotatate", RpcTarget.All, coX, coY, false);
        }
        else
        {
            tuyauxMaze[coX][coY].Rotate();
        }
    }

    [PunRPC]
    public void GlobalStartsAndEnds(int val1, int val2)
    {
        mazeGenerator.startsAndEnds.Add((val1, val2));
    }




    /**
     * <summary>Cette fonction permet de v�rouiller ou de d�verouiller la porte de sortie</summary>
     */
    public void EndOfGame(bool finish = true)
    {
        isLevelFinished = finish;
        GameObject.FindGameObjectsWithTag("DoorsToActivate")[0].SetActive(finish);
    }

    public bool IsLevelFinished() => isLevelFinished;
}
