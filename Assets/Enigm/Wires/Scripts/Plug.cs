using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{

    public int nb = -1;
    public GameObject wire;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    void OnMouseDown()
    {
        if (nb != -1)
        {
            if(nb == 1)
            {
                Debug.Log("you are a success");
            }
            else
            {
                Debug.Log("you smell bad");
            }
            Destroy(wire);
        }
    }
}
