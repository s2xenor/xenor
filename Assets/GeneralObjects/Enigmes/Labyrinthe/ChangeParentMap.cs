using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParentMap : MonoBehaviour
{
    GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        // Put it un maze GameObject (easier to work with)
        map = GameObject.FindGameObjectWithTag("MainMaze").GetComponent<MainMaze>().map;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.parent = map.transform;

        // Fix size
        gameObject.transform.localScale = new Vector2(3, 3);
    }
}
