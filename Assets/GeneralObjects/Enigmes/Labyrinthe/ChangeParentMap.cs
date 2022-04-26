using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParentMap : MonoBehaviour
{
    GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.parent = map.transform;
    }
}
