using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMap : MonoBehaviour
{
    /**
     * Variables Publiques
     */
    //Prefabs
    public GameObject tuyau;
    public GameObject vitre;
    //Canvas
    public GameObject canvaTextPopUP;
    
    public bool DoorLocked = true;
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

    /**
     * Variables Priv�es
     */
    private PCMazeGenerator mazeGenerator;
    public List<(int,int)> StartsAndEnds => mazeGenerator.StartsAndEnds;

    public List<Tuyau> Tuyaux = new List<Tuyau>();
    
    private Tuyau[][] tuyauxMaze;
    public Tuyau[][] TuyauxMaze => tuyauxMaze;

    private void Start()
    {
        StartGeneration();
    }

    // Start is called before the first frame update
    public void StartGeneration()
    {
        mazeGenerator = new PCMazeGenerator(MapSize);

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
            Instantiate(vitre, new Vector3((float)0.32 * -1 - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
            Instantiate(vitre, new Vector3((float)0.32 * mazeGenerator.MapSize - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
            //contenu
            for (int coordX = 0; coordX < mazeGenerator.MapSize; coordX++)
            {
                PCTile tile = mazeGenerator.Maze[coordY][coordX];
                Instantiate(vitre, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
                if (tile.TileType != PCTile.PCTileType.None)
                {
                    //tuyau.GetComponent<Tuyau>().TileData = tile;
                    Tuyau pipe = Instantiate(tuyau, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
                    pipe.TileData = tile;
                    pipe.MessageOnScreenCanvas = canvaTextPopUP;
                    pipe.AffichageUpdate();
                    pipe.Map = this;
                    pipe.InitaliseRotation(coordX, coordY + 1);
                    Tuyaux.Add(pipe);
                    tuyauxMaze[coordX][coordY + 1] = pipe;
                }
            }
        }
        //bord bas et bas
        for (int coordX = -1; coordX <= mazeGenerator.MapSize; coordX++)
        {
            Instantiate(vitre, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * -1 + (float)0.16, 0), Quaternion.identity);
            Instantiate(vitre, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * mazeGenerator.MapSize + (float)0.16, 0), Quaternion.identity);
        }


        int numeroSource = 0;
        //placement des sources et des arriv�es
        foreach ((int,int) coords in mazeGenerator.StartsAndEnds)
        {
            Tuyau pipe = Instantiate(tuyau, new Vector3((float)0.32 * coords.Item2 - (float)0.16, (float)0.32 * coords.Item1 + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
            pipe.Map = this;
            if (coords.Item1 == -1)
            {
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
                Instantiate(floor, new Vector3((float)0.32 * i - (float)0.16, (float)0.32 * j + (float)0.16, 0), Quaternion.identity);
            }
        }

        //Instantie les murs
        for (int j = -3; j < mazeGenerator.MapSize + 3; j++)
        {
            Instantiate(wall_left, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * j + (float)0.16, 0), Quaternion.identity);
            Instantiate(wall_right, new Vector3((float)0.32 * (mazeGenerator.MapSize + 3) - (float)0.16, (float)0.32 * j + (float)0.16, 0), Quaternion.identity);
        }
        for (int i= -3; i < mazeGenerator.MapSize + 3; i++)
        {
            Instantiate(wall_down, new Vector3((float)0.32 * i - (float)0.16, (float)0.32 * -4 + (float)0.16, 0), Quaternion.identity);
            Instantiate(wall_top, new Vector3((float)0.32 * i - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        }
        Instantiate(corner_bottom_left, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * -4 + (float)0.16, 0), Quaternion.identity);
        Instantiate(corner_top_left, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        Instantiate(corner_bottom_right, new Vector3((float)0.32 * (mazeGenerator.MapSize + 3) - (float)0.16, (float)0.32 * -4 + (float)0.16, 0), Quaternion.identity);
        Instantiate(corner_top_right, new Vector3((float)0.32 * (mazeGenerator.MapSize + 3) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);

        // Instantie porte
        //Design
        Instantiate(left_door_design, new Vector3((float)0.32 * -4 - (float)0.16, (float)0.32 * -2 + (float)0.16, 0), Quaternion.identity);
        Instantiate(top_door_design, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        Instantiate(top_door_activated_design, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 3) + (float)0.16, 0), Quaternion.identity);
        //Plaque de pression
        Instantiate(single_door, new Vector3((float)0.32 * (mazeGenerator.MapSize + 1) - (float)0.16, (float)0.32 * (mazeGenerator.MapSize + 2) + (float)0.16, 0), Quaternion.identity);
        GameObject.FindGameObjectWithTag("Door").GetComponent<SingleDoor>().MessageOnScreenCanvas = canvaTextPopUP;

        //Quand jeu est fini il faut lier la salle suivante et afficher les portes

        //Prochaine salle dans porte


    }

    void Update()
    {
        //Cheat code pour d�v�rouiller la porte
        if (Input.GetKeyDown(KeyCode.P))
        {
            EndOfGame();
        }
    }

    /**
     * <summary>Cette fonction permet de v�rouiller ou de d�verouiller la porte de sortie</summary>
     */
    public void EndOfGame()
    {
        DoorLocked = !DoorLocked;
        if (DoorLocked)
        {
            GameObject.FindGameObjectWithTag("DoorsToActivate").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("Door").GetComponent<SingleDoor>().nextSceneName = "";
        }
        else
        {
            GameObject.FindGameObjectWithTag("DoorsToActivate").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.FindGameObjectWithTag("Door").GetComponent<SingleDoor>().nextSceneName = NextSceneName;
        }
    }
}
