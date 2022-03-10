using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeGenerator;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMaze : MonoBehaviour //https://infiniteproductionsblog.wordpress.com/maze-generation-in-csharp/
{
    Byte[,] maze; // Maze

    public int line; // 2 * line + 1
    public int column; // 2 * column + 1

    public Transform[] players = new Transform[2]; // Posiion des 2 joueurs
    public float[] startPos = new float[2]; // Debut du labyrinthe (format x,y)
    public int tileSize; // Taille des tiles du labyrinthe

    int[] player1Tile = new int[2]; // Tile du J1
    int[] player2Tile = new int[2]; // Tile du J2

    public Transform mapParent;
    public GameObject wall;

    public int[] offsetMap = new int[2];

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Maze maze1 = new Maze((ushort)line, (ushort)column); // Créé maze
        maze1.dumpMaze();
        List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
        maze1.dumpMaze();
        maze = maze1.LineToBlock(); // Converti en array

        ModMaze();
        Print();
        CreateMap();

    }

    // Update is called once per frame
    void Update()
    {/*
        player1Tile[0] = (int) (players[0].position.x - startPos[0]) / tileSize; // Calcule tile des joueurs selon leur position
        player1Tile[1] = (int) (players[0].position.y - startPos[1]) / tileSize;
        player2Tile[0] = (int) (players[1].position.x - startPos[0]) / tileSize;
        player2Tile[1] = (int) (players[1].position.y - startPos[1]) / tileSize;

        if (player1Tile[0] >= 0 && player1Tile[0] < maze.GetLength(1) && player1Tile[1] >= 0 && player1Tile[1] < maze.GetLength(0))
        {
            maze[player1Tile[1], player1Tile[0]] = 2; // Debug
            Print();

            if (maze[player1Tile[1],player1Tile[0]] == 1)
            {
                Debug.Log("Hit");
                // Damage Player1
            }
        }

        if (player2Tile[0] >= 0 && player2Tile[0] < column && player2Tile[1] >= 0 && player2Tile[1] < line)
        {
            if (maze[player2Tile[1], player2Tile[0]] == 1)
            {
                // Damage Player2
            }
        }*/
    }

    void ModMaze() // Modifie maze
    {
        int lenMinus1 = maze.GetLength(1) - 1, rand = UnityEngine.Random.Range(1, maze.GetLength(0) - 1);
        maze[0, rand] = 0; // Clear entree
        maze[1, rand] = 0; // Clear entree

        rand = UnityEngine.Random.Range(1, maze.GetLength(0) - 1);
        maze[lenMinus1, rand] = 0; // Clear sortie
        maze[lenMinus1 - 1, rand] = 0; // Clear sortie
        /*
        for (UInt16 i = 1; i < lenMinus1; i++) // Clear les murs de la 1ere et derniere ligne
        {
            maze[1, i] = 0;
            maze[lenMinus1 - 1, i] = 0;
        }*/
    }

    void CreateMap() // Create map for labyrinthe
    {
        float size = 0.36f * 2;

        for (UInt16 i = 0; i < maze.GetLength(0); i++)
        {
            for (UInt16 j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i,j] == 1)
                {
                    GameObject truc = PhotonNetwork.Instantiate(wall.name, new Vector2(i * size + offsetMap[0], j * size + offsetMap[1]), Quaternion.identity);
                    truc.AddComponent<BoxCollider2D>(); // Journee immersion
                }
            }
        }
    }

    void Print() // Print maze
    {
        string str = string.Empty;
        for (UInt16 i = 0; i < maze.GetLength(0); i++)
        {
            for (UInt16 j = 0; j < maze.GetLength(1); j++)
            {
                str += maze[i,j].ToString();
            }
            str += "\n";
        }
        Debug.Log(str);
    }
}
