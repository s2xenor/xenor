using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParentMaze : MonoBehaviour
{
    GameObject maze;

    // Start is called before the first frame update
    void Start()
    {
        maze = GameObject.FindGameObjectWithTag("Maze");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.parent = maze.transform;
    }
}
