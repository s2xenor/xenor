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

        //creating all plugs and saving same in array
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

        //creating all wires and fixing them to random saved plug
        for(int i = 0; i < nbWires; i++)
        {
            int r = Random.Range(0, nbWires-i);
            GameObject plug2 = plugsL[r];
            

            //decal all element to the left
            for(int j = r; j < nbWires-1; j++)
            {
                plugsL[j] = plugsL[j + 1];
            }

            //generate a random color for wire
            int randomColor = Random.Range(0, 4);

            GameObject wire = Instantiate(wirePrefab);                                                          //create a new wire
            plugsN[i].GetComponent<Plug>().wire = wire;                                                         //adding the wire to the plug script
            wire.GetComponent<Wire>().Positions = new Transform[2] { plugsN[i].transform, plug2.transform };    //adding two plugs to the wire script
            wire.GetComponent<Wire>().color = randomColor;                                                      //setting the wire color
            wiresColors[i] = randomColor;                                                                       //saving the wire color 
        }


        /*
        * SHOW RULES
        */
        //show sinon
        int nbFils, color, unplug, rn, fil;
        bool finished = false;                  //a rule above is already valid
        for (int i = 0; i < nbRules; i++)
        {
            rn = Random.Range(0, 3);            //choose a random type of rule
            color = Random.Range(0, nbColors);  //determine the color of the rule
            unplug = Random.Range(0, nbWires);  //the wire to unplug if rule validated
            if (rn == 0)
            {
                nbFils = Random.Range(2, nbWires / 2);
                if (findNbColors(color) >= nbFils && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si il y a {nbFils} fil(s) {randomColor(color)} ou plus, d�branchez le {nbToWord(unplug+1)}");
            }
            else if (rn == 1)
            {
                fil = Random.Range(0, nbWires);
                if (wiresColors[fil] == color && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si le {nbToWord(fil+1)} est {randomColor(color)}, d�branchez le {nbToWord(unplug+1)}");
            }
            else
            {
                nbFils = Random.Range(1, nbWires / 2);
                if (findNbColors(color) < nbFils && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si il y a moins de {nbFils} fil(s) {randomColor(color)}, d�branchez le {nbToWord(unplug + 1)}");
            }
            //else
            //{
            //    fil = Random.Range(0, nbWires);
            //    if (wiresColors[fil] != color && !finished)
            //    {
            //        plugsN[unplug].GetComponent<Plug>().nb = 1;
            //        finished = true;
            //    }
            //    Debug.Log($"Si le {nbToWord(fil + 1)} n'est pas {randomColor(color)}, d�branchez le {nbToWord(unplug+1)}");
            //}
        }

        unplug = Random.Range(0, nbWires);
        Debug.Log($"Sinon d�branchez le {nbToWord(unplug+1)}");
        if (!finished) { 
            plugsN[unplug].GetComponent<Plug>().nb = 1;
        }
        //add fils du type

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //calculate the number of wire of a giwen color
    int findNbColors(int color)
    {
        int nb = 0;
        foreach(int c in wiresColors)
        {
            if (c == color) nb++;
        }
        return nb;
    }

    /*
     * FOR READABILITY
     */
    string nbToWord(int nb)
    {
        if (nb == 1) return "premier fil";
        if (nb == nbWires - 1) return "dernier fil";
        return "fil n�"+nb.ToString();
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

