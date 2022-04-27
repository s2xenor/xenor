using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParentMaze : MonoBehaviour
{
    GameObject maze;
    int n = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Put it un maze GameObject (easier to work with)
        maze = GameObject.FindGameObjectWithTag("MainMaze").GetComponent<MainMaze>().mazeContainer;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.parent = maze.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        n++;
        if (collision.tag == "Player")
            Debug.LogError(n + " Error");
    }
}
