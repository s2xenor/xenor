using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    LineRenderer lr; //the line renderer/wire
    public Transform[] Positions = new Transform[2] { null, null }; //positions for the wire to be plug 
    public int color = -1;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(color != -1)
        {
            //set color
            switch (color)
            {
                case (0)://rouge
                    lr.startColor = new Color(0.43f, 0.03f, 0.1f);
                    lr.endColor = new Color(0.43f, 0.03f, 0.1f);
                    break;
                case 1://bleu
                    lr.startColor = new Color(0.02f, 0.23f, 0.6f);
                    lr.endColor = new Color(0.02f, 0.23f, 0.6f);
                    break;
                case 2://jaune
                    lr.startColor = new Color(0.78f, 0.81f, 0f);
                    lr.endColor = new Color(0.78f, 0.81f, 0f);
                    break;
                case 3://rose
                    lr.startColor = new Color(0.58f, 0.44f, 0.86f);
                    lr.endColor = new Color(0.58f, 0.44f, 0.86f);
                    break;
            }
            //jaune, bleu, vert, rose
            color = -1;
        }



        if (Positions[0] == null) return;
        //set point of wire
        lr.positionCount = Positions.Length;
        for (int i = 0; i < Positions.Length; i++)
        {
            lr.SetPosition(i, Positions[i].position);
        }

        Positions[0] = Positions[1] = null;
    }
}