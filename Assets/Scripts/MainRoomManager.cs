using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainRoomManager : MonoBehaviourPunCallbacks
{

    public GameObject playerBoyPrefab;
    public GameObject playerGirlPrefab;


    public GameObject DoorsOpenCrate;
    public GameObject DoorsOpenPipe;
    public GameObject DoorsOpenLabyInvisible;
    public GameObject DoorsOpenArrows;
    public GameObject DoorsOpenWires;
    public GameObject DoorsOpenDonjon;

    public GameObject Doors1;
    public GameObject Doors2;
    public GameObject Doors3;
    public GameObject Doors4;
    public GameObject Doors5;
    public GameObject Doors6;

    private bool shouldLoad = true;
    private bool initStarted = false;
    private int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerBoyPrefab.name, new Vector2(-0.38f, -4.16f), Quaternion.identity); // Spawn master player on network
        }
        else
        {
            PhotonNetwork.Instantiate(playerGirlPrefab.name, new Vector2(-1.8f, -4.3f), Quaternion.identity); // Spawn player on network
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLoad && GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
            GameObject.FindGameObjectWithTag("Loading").GetComponent<FetchCam>().Del();

            if (PhotonNetwork.IsMasterClient)
            {
                shouldLoad = false;
                PhotonView photonView = PhotonView.Get(this);
                Dictionary<string, bool> LevelsCompleted = GameManager.LevelsCompleted;
                photonView.RPC("SetDoors", RpcTarget.All, LevelsCompleted["Crate"], LevelsCompleted["Pipe"], LevelsCompleted["LabyInvisible"], LevelsCompleted["Arrows"], LevelsCompleted["Wires"], LevelsCompleted["Donjon"]);
            }
            shouldLoad = false;
        }

        if (initStarted && PhotonNetwork.IsMasterClient)
        {
            if (state == 0)//wait for dialogue to finish
            {
                if (this.GetComponentInParent<DialogueTriggerG>().IsDialogueFinished())
                {
                    state += 1;
                    GameObject.FindGameObjectWithTag("slot").GetComponent<SpawnObj>().spawnAtStart = true;
                }
            }
            else if (state == 1)//wait for monster to die
            {

                //if monsters are not all died don't continue
                foreach(GameObject monster in GameObject.FindGameObjectsWithTag("Monster"))
                    if (!monster.GetComponent<MonstreOnline>().dead) state = -1;

                if(state == 1)
                {
                    state = 0;
                    this.GetComponentInParent<DialogueTriggerG>().filePath = "Dialogs/apres_premiere_rencontre_avec_monstre";
                    this.GetComponentInParent<DialogueTriggerG>().triggerOnload = true;
                    initStarted = false;
                }
                else
                {
                    state = 1;
                }
            }
        }
    }

    [PunRPC]
    public void SetDoors(bool Crate, bool Pipe, bool LabyInvisible, bool Arrows, bool Wires, bool Donjon)
    {
        int count = 0;
        if (Crate)
        {
            DoorsOpenCrate.SetActive(true); count++;
        }
        if (Pipe)
        {
            DoorsOpenPipe.SetActive(true); count++;
        }
        if (LabyInvisible)
        {
            DoorsOpenLabyInvisible.SetActive(true); count++;
        }
        if (Arrows)
        {
            DoorsOpenArrows.SetActive(true); count++;
        }
        if (Wires)
        {
            DoorsOpenWires.SetActive(true); count++;
        }
        if (Donjon)
        {
            DoorsOpenDonjon.SetActive(true); count++;
        }

        Debug.Log($"Crate:{Crate}; Pipe:{Pipe}; LabyInvisible:{LabyInvisible}; Arrows:{Arrows}; Wires:{Wires}; Donjon:{Donjon}");

        if (count == 1) Doors1.SetActive(true);
        else if (count == 2) Doors2.SetActive(true);
        else if (count == 3) Doors3.SetActive(true);
        else if (count == 4) Doors4.SetActive(true);
        else if (count == 5) Doors5.SetActive(true);
        else if (count == 6) Doors6.SetActive(true);
    }

    
    [PunRPC]
    public void StartInitMonster(bool local = false)
    {
        if (!local)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("StartInitMonster", RpcTarget.All, true);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var monster in GameObject.FindGameObjectsWithTag("Monster"))
                {
                    PhotonNetwork.Destroy(monster);
                }
            }
            this.GetComponentInParent<DialogueTriggerG>().filePath = "Dialogs/premiere_rencontre_avec_monstre";
            this.GetComponentInParent<DialogueTriggerG>().triggerOnload = true;
            Invoke("TmpInitStarted", 0.5f);
        }
    }

    public void TmpInitStarted()
    {
        initStarted = true;
    }

    [PunRPC]
    public void StartDialogue(string dialogue, bool local = false)
    {
        if (!local)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("StartDialogue", RpcTarget.All, dialogue, true);
        }
        else
        {
            this.GetComponentInParent<DialogueTriggerG>().filePath = dialogue;
            this.GetComponentInParent<DialogueTriggerG>().triggerOnload = true;
        }
    }

}
