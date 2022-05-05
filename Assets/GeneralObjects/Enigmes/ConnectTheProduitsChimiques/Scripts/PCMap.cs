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

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = new PCMazeGenerator();

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
                }
            }
        }
        //bord bas et bas
        for (int coordX = -1; coordX <= mazeGenerator.MapSize; coordX++)
        {
            Instantiate(vitre, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * -1 + (float)0.16, 0), Quaternion.identity);
            Instantiate(vitre, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * mazeGenerator.MapSize + (float)0.16, 0), Quaternion.identity);
        }


        Debug.Log(mazeGenerator.StartsAndEnds.Count);
        //placement des sources et des arrivées
        foreach ((int,int) coords in mazeGenerator.StartsAndEnds)
        {
            Tuyau pipe = Instantiate(tuyau, new Vector3((float)0.32 * coords.Item2 - (float)0.16, (float)0.32 * coords.Item1 + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
            //si en bas (en haut du tableau)
            if (coords.Item2 == -1)
            {
                pipe.TileData = new PCTile(PCTile.PCTileType.Source, PCTile.PCFluidDirection.Down);
            }
            else
            {
                pipe.TileData = new PCTile(PCTile.PCTileType.Source, PCTile.PCFluidDirection.Up);
            }
            pipe.AffichageUpdate();
        }        

        // Instantie porte
        //Prochaine salle dans porte


        //Instantie les murs
    }
}
