using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Photon.Pun;

// Ce script permet d'initialiser la piece pour les labyrinthe de boite, doit etre appeler � chaque nouvelle piece Crate labyrinth enigm
// NE PAS OUBLIER : les player doivent avoir le tag "Player" !!!
public class CrateLabyrinthGenerator : MonoBehaviourPunCallbacks
{
    /*
     * Variables Publique
     */
    //Variables contenants les prefabs qui seront d�pos�es par le script
    public GameObject movableCratePrefab;
    public GameObject unmovableCratePrefab;
    public GameObject stoolPrefab;
    public GameObject dumpsterPrefab;
    public GameObject playerBoyPrefab;
    public GameObject playerGirlPrefab;
    //Affichage
    public GameObject messageOnScreenCanvas;

    bool master = false;
    bool load = false;
    string room;


    /*
     * Fonctions
     */
    // Start is called before the first frame update
    void Start()
    {
        master = PhotonNetwork.IsMasterClient;

        if (master)
        {
            PhotonNetwork.Instantiate(playerBoyPrefab.name, new Vector3(1.6f, 0.95f, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerGirlPrefab.name, new Vector3(1.6f, -0.3f, 0), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (load && GameObject.FindGameObjectsWithTag("Player").Length == 2) // Wait for the 2 players and then the master spawns it
        {
            if (master)
            {
                loadScene();
            }

            load = false;

            Invoke("RemoveLoadingScreen", 1);
        }
    }

    void RemoveLoadingScreen()
    {
        GameObject.FindGameObjectWithTag("Loading").GetComponent<FetchCam>().Del();
    }

    //Fonction qi va cherche les donn�es du fichier Json et qui les formates
    private List<Vector3[]> getData(string filename)
    {
        //Lecture du fichier JSON
        var fileContent = Resources.Load<TextAsset>("CrateRooms/" + filename);
        //Analyse du fichier
        // Cree une liste de liste de vector3
        List<Vector3[]> roomData = JsonConvert.DeserializeObject<List<Vector3[]>>(fileContent.text);
        return roomData;
    }

    [PunRPC]
    public void loadScene(string roomToLoad, bool local = false)
    {
        if (!local)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("loadScene", RpcTarget.All, roomToLoad, true);
        }
        else
        {
            if (master) room = roomToLoad;
            // Allow the room to spawn
            load = true;
        }
    }

    void loadScene()
    {
        string roomToLoad = room;

        //R�cup�rer dans une variable le canvas d'ineraction
        GameObject canvaTextPopUP = GameObject.Find("TextPopUpCanvas");

        //Chargement des coordonn�es des boites de la pi�ce
        List<Vector3[]> roomData = getData(roomToLoad);

        //Instanciation des Boites qui bouge
        foreach (Vector3 coord in roomData[0])
        {
            //Instantiate(movableCratePrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(movableCratePrefab.name, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Instanciation des Boites qui bouge pas
        foreach (Vector3 coord in roomData[1])
        {
            //Instantiate(unmovableCratePrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(unmovableCratePrefab.name, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Instanciation des tabourets
        foreach (Vector3 coord in roomData[2])
        {
            //Instantiate(stoolPrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(stoolPrefab.name, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Instanciation des poubelles
        foreach (Vector3 coord in roomData[3])
        {
            //Instantiate(dumpsterPrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(dumpsterPrefab.name, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }
    }
}

