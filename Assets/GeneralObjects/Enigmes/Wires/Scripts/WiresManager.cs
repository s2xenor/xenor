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
    public GameObject camPrefab;
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

    private bool isOnPressureWire = false;
    private bool isOnPressureRule = false;


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



    [PunRPC]
    public void GenerateAll(bool isFromMaster = false)
    {
        if (isOn) return;
        isOn = true;

        //creating all plugs and saving same in array
        for (int i = 0; i < nbWires; i++)
        {
            //int y = 29 - i * 7;
            float y = startY - i * 0.32f;
            
            GameObject plugNumber = PhotonNetwork.Instantiate(plugPrefab.name, new Vector3(startX, y, -1), Quaternion.identity);
            GameObject plugLetter = PhotonNetwork.Instantiate(plugPrefab.name, new Vector3(startX + 0.32f * 5, y, -1), Quaternion.identity);

            plugNumber.SetActive(false);
            plugLetter.SetActive(false);

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


            GameObject wire = PhotonNetwork.Instantiate(wirePrefab.name, new Vector3(0, 0, 0), Quaternion.identity);  //create a new wire
            plugsN[i].GetComponent<Plug>().wireManager = this;
            plugsN[i].GetComponent<Plug>().wire = wire;                                                         //adding the wire to the plug script
            wire.GetComponent<Wire>().Positions = new Transform[2] { plugsN[i].transform, plug2.transform };    //adding two plugs to the wire script
            wire.GetComponent<Wire>().color = randomColor;                                                      //setting the wire color
            wiresColors[i] = randomColor;                                                                      //saving the wire color 
            wire.SetActive(false);

        }

        /*
        * SHOW RULES
        */
        //show sinon
        GameObject canvasRules = PhotonNetwork.Instantiate(rulesPrefab.name, new Vector3(startX, startY, 0), Quaternion.identity);
        //canvasRules.SetActive(false);
        GameObject txt;
        float coefX = 2.47f;
        float coefY = 0.23f - 2 * 0.32f;

        int nbFils, color, unplug, rn, fil;
        bool finished = false;                  //a rule above is already valid
        for (int i = 0; i < nbRules; i++)
        {
            txt = PhotonNetwork.Instantiate(rulesTextPrefab.name, new Vector3(startX + coefX, startY - i * 0.32f + coefY, 0), Quaternion.identity);
            txt.GetComponent<Transform>().position = new Vector3(startX + coefX, startY - i * 0.32f + coefY, 0);
            canvasRules.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            txt.transform.SetParent(canvasRules.transform);
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
          
        }

        unplug = Random.Range(0, nbWires);
        Debug.Log($"Sinon débranchez le {nbToWord(unplug + 1)}");

        txt = PhotonNetwork.Instantiate(rulesTextPrefab.name, new Vector3(startX + coefX, startY - nbRules * 0.32f + coefY, 0), Quaternion.identity);
        txt.GetComponent<Text>().text = $"Sinon débranchez le {nbToWord(unplug + 1)}";
        txt.transform.SetParent(canvasRules.transform);

        if (!finished)
        {
            plugsN[unplug].GetComponent<Plug>().nb = 1;
        }

        //if (isFromMaster)// rules local and wires distant
        //{
        //    PhotonView photonView = GetComponent<PhotonView>();
        //    SetDistantActive(true, "ruleObject", false);//local rules active
        //    photonView.RPC("SetDistantActive", RpcTarget.All, false, "ruleObject", true);//distant rules inactive

        //    SetDistantActive(false, "wireObject", false);//local wires inactive
        //    photonView.RPC("SetDistantActive", RpcTarget.All, isOnPressureWire, "wireObject", true);//distant rules isOnPressure
        //}
        //else// rules distant and wires local
        //{
        //    PhotonView photonView = PhotonView.Get(this);
        //    photonView.RPC("SetDistantActive", RpcTarget.Others, true, "ruleObject", true);//distant rules active
        //    SetDistantActive(false, "ruleObject", false);//local rules inactive

        //    SetDistantActive(isOnPressureWire, "wireObject", false);//local wires inactive
        //    photonView.RPC("SetDistantActive", RpcTarget.Others, false, "wireObject", true);//distant rules isOnPressure
        //}

        if (isFromMaster)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            SetDistantActive(true, "ruleObject");//local rules active
            photonView.RPC("SetDistantActive", RpcTarget.All, false, "ruleObject");//distant rules inactive
            photonView.RPC("SetDistantActive", RpcTarget.All, isOnPressureWire, "wireObject");//distant rules isOnPressure
        }
        else// rules distant and wires local
        {
            PhotonView photonView = GetComponent<PhotonView>();
            SetDistantActive(isOnPressureWire, "wireObject");//local rules active
            photonView.RPC("SetDistantActive", RpcTarget.All, false, "wireObject");//distant rules inactive
            photonView.RPC("SetDistantActive", RpcTarget.All, true, "ruleObject");//distant rules inactive

        }





    }



    [PunRPC]
    public void SetDistantActive(bool toActive, string tag)
    {
        bool clientOnly = false;
        if (!clientOnly || !PhotonNetwork.IsMasterClient)
        {
            foreach (var elt in GameObject.FindGameObjectsWithTag(tag))
            {
                Debug.Log(elt.name + toActive);
                elt.SetActive(toActive);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void DestroyAll()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("DestroyAll", RpcTarget.MasterClient);
            return;
        }
        isOn = false;

        foreach (var item in GameObject.FindGameObjectsWithTag("wireObject"))
        {
            PhotonNetwork.Destroy(item);
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("ruleObject"))
        {
            PhotonNetwork.Destroy(item);
        }

    }


    [PunRPC]
    public void SetOnPressureWire(bool on, bool fromMaster = false)
    {

        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    PhotonView photonView = PhotonView.Get(this);
        //    photonView.RPC("SetOnPressureWire", RpcTarget.All, on, false);
        //    return;
        //}
        

        Debug.Log("pressure wire " + on);
        isOnPressureWire = on;
        if (!isOnPressureRule)//can be same player activing both plate, punishing him and destroy everything
        {
            DestroyAll();
        }
        else if (GameObject.FindGameObjectsWithTag("wireObject").Length != 0)
        {
            if (fromMaster)
            {
                SetDistantActive(true, "wireObject");
            }
            else
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SetDistantActive", RpcTarget.All, true, "wireObject");
            }
        }
    }

    [PunRPC]
    public void SetOnPressureRules(bool on)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isOnPressureRule = on;
            Debug.Log("pressure rule " + on);
        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetOnPressureRules", RpcTarget.MasterClient, on);
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

