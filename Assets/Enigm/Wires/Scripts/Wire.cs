using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    LineRenderer lr;
    public Transform[] Positions;
    public int color = 0;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        if(color == 0)
        {
        }
        switch (color)
        {
            case (0)://rouge
                lr.startColor = new Color((float)(0.43), (float)(0.03), (float)(0.1));
                lr.endColor = new Color((float)(0.43), (float)(0.03), (float)(0.1));
                break;
            case 1://bleu
                lr.startColor = new Color((float)(0.02), (float)(0.23), (float)(0.6));
                lr.endColor = new Color((float)(0.02), (float)(0.23), (float)(0.6));
                break;
            case 2://jaune
                lr.startColor = new Color((float)(0.78), (float)(0.81), (float)(0));
                lr.endColor = new Color((float)(0.78), (float)(0.81), (float)(0));
                break;
            case 3://rose
                lr.startColor = new Color((float)(0.58), (float)(0.44), (float)(0.86));
                lr.endColor = new Color((float)(0.58), (float)(0.44), (float)(0.86));
                break;
        }
        //jaune, bleu, vert, rose
    }

    // Update is called once per frame
    void Update()
    {
        lr.positionCount = Positions.Length;
        for (int i = 0; i < Positions.Length; i++)
        {
            lr.SetPosition(i, Positions[i].position);
        }
    }
}