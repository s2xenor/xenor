using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeGenerator;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMaze : MonoBehaviour
{
    Byte[,] maze; // Maze

    public int line; // 2 * line + 1
    public int column; // 2 * column + 1

    public GameObject mazeContainer;
    public GameObject map;

    public GameObject wallMaze;
    public GameObject wallMap;

    bool hasSpawned = false;
    bool master = false;

    // Start is called before the first frame update
    void Start()
    {
        master = PhotonNetwork.IsMasterClient;

        map.SetActive(false);

        if (!master) return;

        hasSpawned = true;

        Maze maze1 = new Maze((ushort)line, (ushort)column); // Create maze

        List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
        maze = maze1.LineToBlock(); // Convert into array

        // Set up labyrinthe
        ModMaze();
    }

    private void Update()
    {
        if (hasSpawned && GameObject.FindGameObjectsWithTag("Player").Length == 2 && master)
        {
            CreateMap();
            hasSpawned = false;
        }
    }

    void ModMaze() // Modify maze
    {
        int lenMinus1 = maze.GetLength(1) - 1, rand = UnityEngine.Random.Range(1, maze.GetLength(0) - 1);
        maze[0, rand] = 0; // Clear entree
        maze[1, rand] = 0; // Clear entree

        rand = UnityEngine.Random.Range(1, maze.GetLength(0) - 1);
        maze[lenMinus1, rand] = 0; // Clear sortie
        maze[lenMinus1 - 1, rand] = 0; // Clear sortie
    }

    void CreateMap() // Create map for labyrinthe
    {
        float size = 0.32f / 2, size2 = .32f;

        for (UInt16 i = 0; i < maze.GetLength(0); i++)
        {
            for (UInt16 j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i,j] == 1)
                {
                    // Create wall
                    PhotonNetwork.Instantiate(wallMap.name, new Vector2(i * size, j * size), Quaternion.identity);

                    PhotonNetwork.Instantiate(wallMaze.name, new Vector2(3.2f - 4 * .32f + .16f + i * size2, .16f + .32f * -8 + j * size2), Quaternion.identity);
                }
            }
        }

        // Adjust size
        mazeContainer.transform.localScale = new Vector2(4, 4);
        map.transform.localScale = new Vector2(50, 50);
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
