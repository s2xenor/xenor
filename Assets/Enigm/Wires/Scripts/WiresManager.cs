using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WiresManager : MonoBehaviour
{
    private const int nbWires = 8;
    private const int nbRules = 5;
    private const int nbColors = 4;

    public GameObject wirePrefab;
    public GameObject plugPrefab;
    public GameObject camPrefab;
    public GameObject rulesPrefab;
    public GameObject rulesTextPrefab;


    private GameObject[] plugsL = new GameObject[nbWires];
    private GameObject[] plugsN = new GameObject[nbWires];
    private int[] wiresColors = new int[nbWires];


    //positions of wires
    private int startY = -100;
    private int startX = -100;


    // Start is called before the first frame update
    void Start()
    {
        var parentObj = new GameObject("WireEnigmParent");
        //creating all plugs and saving same in array
        for (int i = 0; i < nbWires; i++)
        {
            //int y = 29 - i * 7;
            int y = startY - i * 7;
            GameObject plugNumber = Instantiate(plugPrefab, new Vector3(startX, y, 5), Quaternion.identity, parentObj.transform);
            GameObject plugLetter = Instantiate(plugPrefab, new Vector3(startX + 30, y, 0), Quaternion.identity, parentObj.transform);

            plugsN[i] = plugNumber;
            plugsL[i] = plugLetter;

            plugNumber.GetComponent<Plug>().nb = 0;

        }

        //creating all wires and fixing them to random saved plug
        for (int i = 0; i < nbWires; i++)
        {
            int r = Random.Range(0, nbWires - i);
            GameObject plug2 = plugsL[r];                    //take random plug on right side


            //move all element to the left
            for (int j = r; j < nbWires - 1; j++)
            {
                plugsL[j] = plugsL[j + 1];
            }

            //generate a random color for wire
            int randomColor = Random.Range(0, 4);
            

            GameObject wire = Instantiate(wirePrefab, new Vector3(0, 0, 0), Quaternion.identity, parentObj.transform);  //create a new wire
            plugsN[i].GetComponent<Plug>().wire = wire;                                                         //adding the wire to the plug script
            wire.GetComponent<Wire>().Positions = new Transform[2] { plugsN[i].transform, plug2.transform };    //adding two plugs to the wire script
            wire.GetComponent<Wire>().color = randomColor;                                                      //setting the wire color
            wiresColors[i] = randomColor;                                                                       //saving the wire color 
        }


        //instantiate cam on wire
        GameObject _cam = Instantiate(camPrefab, new Vector3(startX + 15, (float)(startY - nbWires / 2 * 7 + 3.5), 0), Quaternion.identity, parentObj.transform);                                                          //create a new wire
        
        _cam.GetComponent<Camera>().nearClipPlane = 0;
        _cam.GetComponent<Camera>().orthographicSize = (float)(7.2 * nbWires / 2);
        _cam.name = "WiresCam";
        _cam.SetActive(false);

        GameObject MainCam = GameObject.FindWithTag("MainCamera");
        MainCam.SetActive(true);

        /*
        * SHOW RULES
        */
        //show sinon
        GameObject canvasRules = Instantiate(rulesPrefab, new Vector3(0, 0, 0), Quaternion.identity, parentObj.transform);
        canvasRules.GetComponent<Canvas>().worldCamera = MainCam.GetComponent<Camera>();
        //canvasRules.SetActive(false);
        GameObject txt;
        float coef = 0.2f;
        int nbFils, color, unplug, rn, fil;
        bool finished = false;                  //a rule above is already valid
        for (int i = 0; i < nbRules; i++)
        {
            txt = Instantiate(rulesTextPrefab, new Vector3(-1,(0.55f - i * coef), 0), Quaternion.identity, canvasRules.transform);
            //txt.GetComponent<Transform>().position = new Vector3(850, 0, 0);
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
                Debug.Log($"Si il y a {nbFils} fil(s) {randomColor(color)} ou plus, débranchez le {nbToWord(unplug + 1)}");
                txt.GetComponent<Text>().text = $"Si il y a {nbFils} fil(s) {randomColor(color)} ou plus, débranchez le {nbToWord(unplug + 1)}";
            }
            else if (rn == 1)
            {
                fil = Random.Range(0, nbWires);
                if (wiresColors[fil] == color && !finished)
                {
                    plugsN[unplug].GetComponent<Plug>().nb = 1;
                    finished = true;
                }
                Debug.Log($"Si le {nbToWord(fil + 1)} est {randomColor(color)}, débranchez le {nbToWord(unplug + 1)}");
                txt.GetComponent<Text>().text = $"Si le {nbToWord(fil + 1)} est {randomColor(color)}, débranchez le {nbToWord(unplug + 1)}";
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
                txt.GetComponent<Text>().text = $"Si il y a moins de {nbFils} fil(s) {randomColor(color)}, débranchez le {nbToWord(unplug + 1)}";
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
        Debug.Log($"Sinon débranchez le {nbToWord(unplug + 1)}");
        txt = Instantiate(rulesTextPrefab, new Vector3(-1, (0.55f - (nbRules) * coef), 0), Quaternion.identity, canvasRules.transform);
        txt.GetComponent<Text>().text = $"Sinon débranchez le {nbToWord(unplug + 1)}";

        if (!finished)
        {
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
        foreach (int c in wiresColors)
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
        return "fil n°" + nb.ToString();
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

