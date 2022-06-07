using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParentMaze : MonoBehaviour
{
    GameObject maze;

    void Start()
    {
        // Put it in maze GameObject (easier to work with)
        maze = GameObject.FindGameObjectWithTag("MainMaze").GetComponent<MainMaze>().mazeContainer;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.parent = maze.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
