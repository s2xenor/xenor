using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeGenerator;

public class MainMaze : MonoBehaviour
{
    int x = 10;
    int y = 10;

    // Start is called before the first frame update
    void Start()
    {   //https://infiniteproductionsblog.wordpress.com/maze-generation-in-csharp/
        Maze maze1 = new Maze((ushort)x, (ushort)y); // Créé maze
        maze1.dumpMaze();
        List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
        maze1.dumpMaze();
        Byte[,] blockmaze = maze1.LineToBlock();

        int lenMinus1 = blockmaze.GetLength(1) - 1; // Fait entree et clear la 1ere et derniere colonne
        blockmaze[0, UnityEngine.Random.Range(1,y)] = 0;
        blockmaze[lenMinus1, UnityEngine.Random.Range(1,y)] = 0;
        for (UInt16 i = 1; i < lenMinus1; i++)
        {
            blockmaze[1, i] = 0;
            blockmaze[lenMinus1 - 1, i] = 0;
        }

        string str = string.Empty; // Print maze
        for (UInt16 i = 0; i < blockmaze.GetLength(1); i++)
        {
            for (UInt16 j = 0; j < blockmaze.GetLength(0); j++)
            {
                str += ' ' + blockmaze[j, i].ToString();
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
