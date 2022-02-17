using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    LineRenderer lr;
    public Transform[] Positions;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lr != null)
        {
            lr.positionCount = Positions.Length;
            for(int i = 0; i < Positions.Length; i++){
                lr.SetPosition(i, Positions[i].position);
            }

            if (Input.GetMouseButtonDown(0))
             {
                Destroy(lr);
            }
         }

    }
}
