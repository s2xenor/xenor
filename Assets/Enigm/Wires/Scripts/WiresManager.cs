using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresManager : MonoBehaviour
{
    private const int nbWires = 8;

    public GameObject wirePrefab;
    public GameObject plugPrefab;

    private GameObject[] plugsL = new GameObject[nbWires];
    private GameObject[] plugsN = new GameObject[nbWires];


    // Start is called before the first frame update
    void Start()
    {


        for (int i = 0; i < nbWires; i++)
        {
            int y = i * 7 - 20;
            GameObject plugNumber = Instantiate(plugPrefab, new Vector3(0, y, 0), Quaternion.identity);
            GameObject plugLetter = Instantiate(plugPrefab, new Vector3(30, y, 0), Quaternion.identity);

            plugsN[i] = plugNumber;
            plugsL[i] = plugLetter;


            //GameObject wire = Instantiate(wirePrefab);
            //wire.GetComponent<Wire>().Positions = new Transform[2] { plugNumber.transform, plugLetter.transform };

            plugNumber.GetComponent<Plug>().nb = i;
            //plugNumber.GetComponent<Plug>().wire = wire;

            //plugLetter.GetComponent<Plug>().nb = i;
            //plugLetter.GetComponent<Plug>().wire = wire;
        }

        for(int i = 0; i < nbWires; i++)
        {
            int r = Random.Range(0, nbWires-i);
            GameObject plug2 = plugsL[r];
            

            //decal all element to the left
            for(int j = r; j < nbWires-1; j++)
            {
                plugsL[j] = plugsL[j + 1];
            }

            int randomColor = Random.Range(0, 4);

            GameObject wire = Instantiate(wirePrefab);
            plugsN[i].GetComponent<Plug>().wire = wire;
            wire.GetComponent<Wire>().Positions = new Transform[2] { plugsN[i].transform, plug2.transform };
            wire.GetComponent<Wire>().color = randomColor;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
