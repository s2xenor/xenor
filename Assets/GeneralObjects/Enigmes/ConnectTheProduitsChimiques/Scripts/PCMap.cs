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

    /**
     * Variables Privées
     */
    private PCMazeGenerator mazeGenerator;

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = new PCMazeGenerator();

        //Récupérer dans une variable le canvas d'ineraction
        GameObject canvaTextPopUP = GameObject.Find("TextPopUpCanvas");
        tuyau.GetComponent<Tuyau>().MessageOnScreenCanvas = canvaTextPopUP;

        //Instantie de toutes les tiles avec la bonne image
        for (int coordY = 0; coordY < mazeGenerator.MapSize; coordY++)
        {
            for (int coordX = 0; coordX < mazeGenerator.MapSize; coordX++)
            {
                PCTile tile = mazeGenerator.Maze[coordY][coordX];
                Instantiate(vitre, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity);
                if (tile.TileType != PCTile.PCTileType.None)
                {
                    //tuyau.GetComponent<Tuyau>().TileData = tile;
                    Tuyau pipe = Instantiate(tuyau, new Vector3((float)0.32 * coordX - (float)0.16, (float)0.32 * coordY + (float)0.16, 0), Quaternion.identity).GetComponent<Tuyau>();
                    pipe.TileData = tile;
                    pipe.AffichageUpdate();
                }
            }
        }

        // Instantie porte
        //Prochaine salle dans porte

        //Instantie les murs
    }
}
