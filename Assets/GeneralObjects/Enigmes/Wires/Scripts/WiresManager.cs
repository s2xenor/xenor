using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class WiresManager : MonoBehaviourPunCallbacks
{
    private const int nbWires = 8;
    private const int nbRules = 5;
    private const int nbColors = 4;

    public GameObject wirePrefab;
    public GameObject plugPrefab;
    public GameObject rulesPrefab;          //canvas with notepad background
    public GameObject rulesTextPrefab;      //text ui

    public bool isOn = false;

    public GameObject[] plugsL = new GameObject[nbWires];
    public GameObject[] plugsN = new GameObject[nbWires];
    public int[] wiresColors = new int[nbWires];

    //positions of wires
    private float startY = 0.16f;
    private float startX = -1.76f;

    GameObject parentObj;
    public GameObject playerPrefab;

    public bool isOnPressureWire = false;
    public bool isOnPressureRule = false;
    public bool isMasterOnWire = false;
    public bool isMasterOnRule = false;

    // Start is called before the first frame update
    void Start()
    {
        //PhotonView photonView = PhotonView.Get(this);
        //photonView.RPC("MoveCoo", RpcTarget.MasterClient);
        parentObj = new GameObject("WireEnigmParent");

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-12*0.32f, 5*0.32f), Quaternion.identity); // Spawn master player on network
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-12 * 0.32f, 2 * 0.32f), Quaternion.identity); // Spawn player on network
        }
    }

    public bool IsLevelFinished()
    {
        return true;
    }



    public void GenerateAll()
    {
        if (isOn) return;
        isOn = true;

        int correctWire = 0;

        


        /*
        * SHOW RULES
        */
        //show sinon
        string[] txts = new string[nbRules + 1];
        

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
                    correctWire = unplug;
                    finished = true;
                }
                txts[i] = $"Si il y a {nbFils} fil(s) {randomColor(color)} ou plus, débranchez le {nbToWord(unplug + 1)}";
            }
            else if (rn == 1)
            {
                fil = Random.Range(0, nbWires);
                if (wiresColors[fil] == color && !finished)
                {
                    correctWire = unplug;
                    finished = true;
                }
                txts[i] = $"Si le {nbToWord(fil + 1)} est {randomColor(color)}, débranchez le {nbToWord(unplug + 1)}";
            }
            else
            {
                nbFils = Random.Range(1, nbWires / 2);
                if (findNbColors(color) < nbFils && !finished)
                {
                    correctWire = unplug;
                    finished = true;
                }
                txts[i] = $"Si il y a moins de {nbFils} fil(s) {randomColor(color)}, débranchez le {nbToWord(unplug + 1)}";
            }

        }

        unplug = Random.Range(0, nbWires);
        txts[txts.Length-1] = $"Sinon débranchez le {nbToWord(unplug + 1)}";

        if (!isMasterOnRule)//go to client to instantiate rules
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetTxtProps", RpcTarget.Others, txts);
        }
        else
        {
            SetTxtProps(txts);
        }

                    
        if (!finished) correctWire = unplug;

        /*
         * Creating wires
         */

        //generating a color for each wire
        for (int i = 0; i < nbWires; i++) wiresColors[i] = Random.Range(0, 4);

        if (isMasterOnRule)//client need wires
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetWire", RpcTarget.Others, correctWire, wiresColors, isOnPressureWire);
        }
        else
        {
            SetWire(correctWire, wiresColors, isOnPressureWire);
        }

    }

    [PunRPC]
    public void SetTxtProps(string[] txts)
    {
        GameObject canvasRules = PhotonNetwork.Instantiate(rulesPrefab.name, new Vector3(startX, startY, 0), Quaternion.identity);
        //canvasRules.SetActive(false);
        canvasRules.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        GameObject txt;
        float coefX = 2.47f;
        float coefY = 0.23f - 2 * 0.32f;

        for (int i = 0; i < nbRules+1; i++)
        {
            txt = Instantiate(rulesTextPrefab, new Vector3(startX + coefX, startY - nbRules * 0.32f + coefY, 0), Quaternion.identity, canvasRules.transform);
            txt.GetComponent<Transform>().position = new Vector3(startX + coefX, startY - i * 0.32f + coefY, 0);

            txt.GetComponent<Text>().text = txts[i];
            txt.transform.SetParent(canvasRules.transform);
        }
    }


    [PunRPC]
    public void SetWire(int correctWire, int[] wiresColor, bool active)
    {
        GameObject plugNumber, plugLetter, wire;
        for (int i = 0; i < nbWires; i++)
        {
            float y = startY - i * 0.32f;

            plugNumber = Instantiate(plugPrefab, new Vector3(startX, y, -1), Quaternion.identity, parentObj.transform);
            //plugNumber.AddComponent<SphereCollider>();
            //plugNumber.GetComponent<SphereCollider>().isTrigger = true;


            plugLetter = Instantiate(plugPrefab, new Vector3(startX + 0.32f * 5, y, -1), Quaternion.identity, parentObj.transform);

            plugsN[i] = plugNumber;
            plugsL[i] = plugLetter;

            plugNumber.GetComponent<Plug>().nb = correctWire == i ? 1 : 0; //is correct wire
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


            
            wire = Instantiate(wirePrefab, new Vector3(0, 0, 0), Quaternion.identity, parentObj.transform);  //create a new wire
            plugsN[i].GetComponent<Plug>().wireManager = this;
            plugsN[i].GetComponent<Plug>().wire = wire;                                                         //adding the wire to the plug script
            wire.GetComponent<Wire>().Positions = new Transform[2] { plugsN[i].transform, plug2.transform };    //adding two plugs to the wire script
            wire.GetComponent<Wire>().color = wiresColor[i];                                                    //setting the wire color

            if (!active) //is not on pressure wire
            {
                ShowDistantWires(false);
            }
        }
    }



    [PunRPC]
    public void ShowDistantWires(bool show)
    {
        Debug.Log("show wire"+show);
        parentObj.SetActive(show);
    }

    [PunRPC]
    public void UnPlug(bool correct)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (correct)
            {
                //activate door
            }
            else
            {
                //remove life
                foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (item.GetComponent<PhotonView>().IsMine)
                    {
                        item.GetComponent<player>().vie.Reduce4(1);
                    }
                }
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("DestroyAll", RpcTarget.All);
            }

        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("UnPlug", RpcTarget.MasterClient, correct);
        }
    }



    public void Changes()
    {
        if (isOn)
        {
            if (isOnPressureWire)
            {
                if (isOnPressureRule)//both activated show wires
                {
                    if (isMasterOnWire)
                    {
                        ShowDistantWires(true);
                    }
                    else
                    {
                        PhotonView photonView = PhotonView.Get(this);
                        photonView.RPC("ShowDistantWires", RpcTarget.All, true);
                    }
                }
                else// could be same player who activated both pressure, punish them
                {
                    PhotonView photonView = PhotonView.Get(this);
                    photonView.RPC("DestroyAll", RpcTarget.All);
                }
            }
        }
        else
        {
            if (isOnPressureRule)
            {
                GenerateAll();
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (isOnPressureRule)
        {
            if (isMasterOnRule) Debug.Log("master on rule");
            else Debug.Log("client on rule");
        }
        else Debug.Log("nothing on rule");


        if (isOnPressureWire)
        {
            if (isMasterOnWire) Debug.Log("master on wires");
            else Debug.Log("client on wires");
        }
        else Debug.Log("nothing on wires");

    }


    [PunRPC]
    public void DestroyAll()
    {
        
        isOn = false;
        foreach (var item in GameObject.FindGameObjectsWithTag("wireObject"))
        {
            Destroy(item);
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("ruleObject"))
        {
            Destroy(item);
        }

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

