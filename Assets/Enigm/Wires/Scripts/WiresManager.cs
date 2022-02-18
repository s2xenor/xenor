using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresManager : MonoBehaviour
{
    private const int nbWires = 8;
    private const int nbRules = 5;
    private const int nbColors = 4;

    public GameObject wirePrefab;
    public GameObject plugPrefab;

    private GameObject[] plugsL = new GameObject[nbWires];
    private GameObject[] plugsN = new GameObject[nbWires];
    private int[] wiresColors = new int[nbWires];

    // Start is called before the first frame update
    void Start()
    {


        for (int i = 0; i < nbWires; i++)
        {
            int y = 29 - i * 7;
            GameObject plugNumber = Instantiate(plugPrefab, new Vector3(0, y, 0), Quaternion.identity);
            GameObject plugLetter = Instantiate(plugPrefab, new Vector3(30, y, 0), Quaternion.identity);

            plugsN[i] = plugNumber;
            plugsL[i] = plugLetter;


            //GameObject wire = Instantiate(wirePrefab);
            //wire.GetComponent<Wire>().Positions = new Transform[2] { plugNumber.transform, plugLetter.transform };

            plugNumber.GetComponent<Plug>().nb = 0;
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
            wiresColors[i] = randomColor;
        }


        /*
        * SHOW RULES
        */
        //show sinon
        int nbFils, color, unplug, rn, fil;
        bool finished = false;
        for (int i = 0; i < 5; i++)
        {
            rn = Random.Range(0, 3);
            color = Random.Range(0, nbColors);
            unplug = Random.Range(0, nbWires);
            if (rn == 0)
            {
                nbFils = Random.Range(2, nbWires / 2);
                if (findNbColors(color) >= nbFils && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si il y a {nbFils} fil(s) {randomColor(color)} ou plus, débranchez le {nbToWord(unplug+1)}");
            }
            else if (rn == 1)
            {
                fil = Random.Range(0, nbWires);
                if (wiresColors[fil] == color && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si le {nbToWord(fil+1)} est {randomColor(color)}, débranchez le {nbToWord(unplug+1)}");
            }
            else
            {
                nbFils = Random.Range(1, nbWires / 2);
                if (findNbColors(color) < nbFils && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si il y a moins de {nbFils} fil(s) {randomColor(color)}, débranchez le {nbToWord(unplug + 1)}");
            }
            //else
            //{
            //    fil = Random.Range(0, nbWires);
            //    if (wiresColors[fil] != color && !finished)
            //    {
            //        plugsN[unplug].GetComponent<Plug>().nb = 1;
            //        finished = true;
            //    }
            //    Debug.Log($"Si le {nbToWord(fil + 1)} n'est pas {randomColor(color)}, débranchez le {nbToWord(unplug+1)}");
            //}
        }

        unplug = Random.Range(0, nbWires);
        Debug.Log($"Sinon débranchez le {nbToWord(unplug+1)}");
        if (!finished) { 
            plugsN[unplug].GetComponent<Plug>().nb = 1;
        }
        //add fils du type

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    int findNbColors(int color)
    {
        int nb = 0;
        foreach(int c in wiresColors)
        {
            if (c == color) nb++;
        }
        return nb;
    }
    string nbToWord(int nb)
    {
        if (nb == 1) return "premier fil";
        if (nb == nbWires - 1) return "dernier fil";
        return "fil n°"+nb.ToString();
    }

    string randomColor(int color)
    {
        switch (color)
        {
            case (0)://rouge
                return "rouge";
            case 1://bleu
                return "bleu";
            case 2://jaune
                return "jaune";
            default://rose 3
                return "violet";
        }
    }
}

