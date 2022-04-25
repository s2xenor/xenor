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

    public GameObject map;
    public GameObject mazeContainer;

    public GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        Maze maze1 = new Maze((ushort)line, (ushort)column); // Create maze

        List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
        maze = maze1.LineToBlock(); // Convert into array

        // Set up labyrinthe
        ModMaze();
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

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
        float size = 0.36f * 2;

        for (UInt16 i = 0; i < maze.GetLength(0); i++)
        {
            for (UInt16 j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i,j] == 1)
                {
                    // Create wall and put it in map childrens
                    GameObject mur = Instantiate(wall, new Vector2(i * size, j * size), Quaternion.identity);
                    mur.GetComponent<BoxCollider2D>().enabled = false;
                    mur.transform.parent = map.transform;
                    
                    mur = Instantiate(wall, new Vector2(i * size, j * size), Quaternion.identity);
                    mur.GetComponent<SpriteRenderer>().enabled = false;
                    mur.transform.parent = mazeContainer.transform;
                }
            }
        }

        map.transform.localScale = new Vector2(1, 1);
        mazeContainer.transform.localScale = new Vector2(2, 2);

        map.SetActive(false);
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
