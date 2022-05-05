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

    /**
     * Variables Privées
     */
    private PCMazeGenerator mazeGenerator;
    public List<(int,int)> StartsAndEnds => mazeGenerator.StartsAndEnds;

    public List<Tuyau> Tuyaux = new List<Tuyau>();
    
    private Tuyau[][] tuyauxMaze;
    public Tuyau[][] TuyauxMaze => tuyauxMaze;

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = new PCMazeGenerator();

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
        //placement des sources et des arrivées
        foreach ((int,int) coords in mazeGenerator.StartsAndEnds)
        {
            //(y,x)
            //Debug.Log("Coords:");
            //Debug.Log(coords.Item1);
            //Debug.Log(coords.Item2);
            Tuyau pipe = Instantiate(tuyau, new Vector3((float)0.32 * coords.Item2 - (float)0.16, (float)0.32 * coords.Item1 + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
            pipe.Map = this;
            //si en bas (en haut du tableau)
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
                //Debug.Log("Tab:");
                //Debug.Log(mazeGenerator.MapSize);
                //Debug.Log(tuyauxMaze[0].Length);
                //Debug.Log(tuyauxMaze.Length);
                tuyauxMaze[coords.Item2][mazeGenerator.MapSize] = pipe;
                pipe.InitaliseRotation(coords.Item2, mazeGenerator.MapSize);
                pipe.MessageOnScreenCanvas = canvaTextPopUP;
                pipe.AffichageUpdate();
                Tuyaux.Add(pipe);
            }
        }        

        // Instantie porte
        //Prochaine salle dans porte


        //Instantie les murs
    }
}
