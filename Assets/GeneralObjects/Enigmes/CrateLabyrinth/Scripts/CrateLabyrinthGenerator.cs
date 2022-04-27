using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

// Ce script permet d'initialiser la piece pour les labyrinthe de boite, doit etre appeler à chaque nouvelle piece Crate labyrinth enigm
// NE PAS OUBLIER : les player doivent avoir le tag "Player" !!!
public class CrateLabyrinthGenerator : MonoBehaviour
{
    /*
     * Variables Publique
     */
    //Variables contenants les prefabs qui seront déposées par le script
    public GameObject movableCratePrefab;
    public GameObject unmovableCratePrefab;
    public GameObject stoolPrefab;
    public GameObject dumpsterPrefab;

    //Affichage
    public GameObject messageOnScreenCanvas;

    /*
     * Fonctions
     */
    // Start is called before the first frame update
    void Start()
    {
        // Initialisation de moyen pour lier les boites et le joueur lorsque l'on veut tirer une boite
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))//pour tout les players
        {
            // On ajouter un <FixedJoint2D> et on met autoConfigureConnectedAnchor à false (sinon ça lie pas les objets)
            // POur lier les obj sur chaque boite y aura un script qui permettra de lier la boite au FixedJoint2D ou de les délier (avec touche E)
            // Attention : faudra penser à verifier que y a pas déjà une boite accrochée...
            player.AddComponent<FixedJoint2D>();
            player.GetComponent<FixedJoint2D>().enabled = false;
            player.GetComponent<FixedJoint2D>().autoConfigureConnectedAnchor = false;
        }
    }

    private void Update()
    {
        //CheatCode pour CrateLabyrinthScene
        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name == "CrateLabyrinthScene")
        {
            //si on appuie sur la touche P, ça nous tp à la fin du niveau
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            players[0].GetComponent<Transform>().position = new Vector3(-4, 1, 0);
            players[1].GetComponent<Transform>().position = new Vector3(-4, (float)-0.5, 0);
        }
    }

    //Fonction qi va cherche les données du fichier Json et qui les formates
    private List<Vector3[]> getData(string filename)
    {
        //Lecture du fichier JSON
        var fileContent = Resources.Load<TextAsset>("CrateRooms/" + filename);
        //Analyse du fichier
        // Cree une liste de liste de vector3
        List<Vector3[]> roomData = JsonConvert.DeserializeObject<List<Vector3[]>>(fileContent.text);
        return roomData;
    }

    public void loadScene(string roomToLoad)
    {
        //Récupérer dans une variable le canvas d'ineraction
        GameObject canvaTextPopUP = GameObject.Find("TextPopUpCanvas");

        //Chargement des coordonnées des boites de la pièce
        List<Vector3[]> roomData = getData(roomToLoad);

        //Instanciation des Boites qui bouge
        foreach (Vector3 coord in roomData[0])
        {
            Instantiate(movableCratePrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Instanciation des Boites qui bouge pas
        foreach (Vector3 coord in roomData[1])
        {
            Instantiate(unmovableCratePrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Instanciation des tabourets
        foreach (Vector3 coord in roomData[2])
        {
            Instantiate(stoolPrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Instanciation des poubelles
        foreach (Vector3 coord in roomData[3])
        {
            Instantiate(dumpsterPrefab, new Vector3((float)0.32 * coord.x - (float)0.16, (float)0.32 * coord.y + (float)0.16, 0), Quaternion.identity);
        }

        //Lier aux boites qui bougent le canvas d'intéraction
        foreach (GameObject movableCrate in GameObject.FindGameObjectsWithTag("Box"))
        {
            movableCrate.GetComponent<MovableCrate>().MessageOnScreenCanvas = messageOnScreenCanvas;
        }
    }
}

