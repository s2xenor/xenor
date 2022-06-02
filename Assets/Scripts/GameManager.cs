using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
// Ce script permet de gere tout ce qui doit passer entre les scenes
// Egalement les sauvegarde
// Il permet de sauvegarder des donn�es que l'on souhaite r�cup�rer apr�s le chargement de la sc�ne suivante
// ATTENTION : ce script ne doit �tre plac� qu'une seule fois dans le jeu !!!!!!!!!!!!!!!!! Lors de la premi�re scene.
// ATTENTION : On ne doit pas pouvoir revenir sur la premi�re sc�ne car sinon le script va se dupliquer ce qui entraine des bugs
// Conseil : Placer le script dans le menu de chargement.
// NE PAS OUBLIER : ajouter un EmptyObject nomm� GameManger et contenant le script GameManager sur la premi�re sc�ne du jeu.
public class GameManager : MonoBehaviourPunCallbacks
{
    /*
     * Variables Satiques
     * 
     * Permet que l'on puisse y acc�der depuis n'importe quel script m�me s'il n'est pas dans la hierarchie
     */

    // Le Script GameManager lui-m�me
    // Permet que l'on puisse sauvergarder des donn�es depuis n'importe o�
    public static GameManager instance; // Va �tre egal au premier GameManager qu'il trouve dans le jeu


    /*
     * Variables use to determine score
     */
    public int QuarterHeartLost = 0;    //global number of heart lost during the game
    public int TimestampStart = 0;


    public string NextSceneDoor;        //string for the next scene defined by single door in lobby mainly

    /*
     * Variables Publiques des trucs qu'il y a � sauvgarder
     */
    public PlayerSettings playerSettings = new PlayerSettings();

    //NE PAS OUBLIER : idem aevc inventory
    // public Inventory inventory = new Inventory();

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        ResetSave(); // si on veut pas recup�rer les donn�es de la derni�re session quand on relance le jeu.

        instance = this;
        SceneManager.sceneLoaded += LoadState; //maintenant quand on load une nouvelle scene on va aussi appeler le truc pour load les donn�es
        DontDestroyOnLoad(gameObject); //ne pas supprimer un objet quand on change de scene
        Screen.fullScreen = true;
    }

    //Fonction SaveState() de sauvegarder toutes les infos que l'on souhaite conserver d'une scene � l'autre
    /**
    * <summary>Permet de sauvegarder toutes les infos que l'on souhaite conserver d'une scene � l'autre</summary>
    * 
    * <returns>Return nothing</returns>
*/
    public void SaveState()
    {
        // On utilise le module JsonUtility pour parse tout l'objet
        // NE PAS OUBLIER : il faudra remplacer apr ce qu'il faut pour aller chercher l'inventaire
        // string inventoryData = JsonUtility.ToJson(inventory);

        // Chemin ou va �tre enregistrer le JSON
        // persistentDataPath est un dossier qui ne sera jamais modifier par unity m�me mise � jour
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        // On �crit le fichier
        // NE PAS OUBLIER : R�activer quand se sera ok
        //System.IO.File.WriteAllText(filePath, inventoryData);
    }

    //Fonction SavePlayerSettings() de sauvegarder tous les settings du player apr�s une modification
    /**
    * <summary>Permet de sauvegarder de sauvegarder tous les settings du player apr�s une modification</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void SavePlayerSettings()
    {
        // On utilise le module JsonUtility pour parse tout l'objet
        string playerSettingsData = JsonUtility.ToJson(playerSettings);

        // Chemin ou va �tre enregistrer le JSON
        // persistentDataPath est un dossier qui ne sera jamais modifier par unity m�me mise � jour
        string filePath = Application.persistentDataPath + "/PlayerSettingsData.json";

        // On �crit le fichier
        System.IO.File.WriteAllText(filePath, playerSettingsData);
    }

    //Fonction SaveState() de r�cuperer les infos sauvgarder dans la sc�ne pr�c�dente
    /**
    * <summary>Permet de r�cuperer les infos sauvgarder dans la sc�ne pr�c�dente</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        // Chemin ou est stock� le json de l'inventaire
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        if (System.IO.File.Exists(filePath))
        {
            // On le parse pour r�cup les infos
            string inventoryData = System.IO.File.ReadAllText(filePath);

            // On recr�e un inventaire avec les infos
            // NE PAS OUBLIER : R�activer quand se sera ok
            //inventory = JsonUtility.FromJson<Inventory>(inventoryData);
        }

        // Chemin ou est stock� le json de l'inventaire
        filePath = Application.persistentDataPath + "/PlayerSettingsData.json";

        if (System.IO.File.Exists(filePath))
        {
            // On le parse pour r�cup les infos
            string playerSettingsData = System.IO.File.ReadAllText(filePath);

            // On recr�e un playerSettings avec les infos
            // NE PAS OUBLIER : R�activer quand se sera ok
            playerSettings = JsonUtility.FromJson<PlayerSettings>(playerSettingsData);
        }
    }

    //Fonction ResetSave() permet de supprimer toutes les infos stock� dans les Json
    /**
    * <summary>Permet de supprimer toutes les infos stock� dans les Json</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void ResetSave()
    {
        // Chemin ou est stock� le json de l'inventaire
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        // On d�truit le fichier
        System.IO.File.Delete(filePath);
    }

    //Fonction ResetPlayerSettings() permet de supprimer tous les settings du player
    /**
    * <summary>Permet de supprimer tous les settings du player</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void ResetPlayerSettings()
    {
        // Chemin ou est stock� le json de l'inventaire
        string filePath = Application.persistentDataPath + "/PlayerSettingsData.json";

        // On d�truit le fichier
        System.IO.File.Delete(filePath);
    }


    /*
     * MULTI 
     */

    Dictionary<string, string> Scenes;  //Dictionnary allowing to change scene name without messing with code

    public void Start()
    {
            
        Scenes = new Dictionary<string, string>();
        Scenes.Add("Crate", "");
        Scenes.Add("Pipe", "");
        Scenes.Add("LabyInvisible", "");
        Scenes.Add("Arrows", "");
        Scenes.Add("Wires", "");
        Scenes.Add("Donjon", "");
        Scenes.Add("Cells", "Cells");
        Scenes.Add("MainRoom", "MainRoom");

    }


    //called when a pressure plate is pressed
    int doorActivated = 0;
    public void DoorUpdate(int increment, bool doubleD)
    {
        doorActivated += increment; //number of pressure pressed
        if((doubleD && doorActivated >= 2) || (!doubleD && doorActivated >= 1)) //test is good number is pressed based on type of door
        {
            if (LevelCompleted())
            {
                doorActivated = 0;  //reset
                LoadNextScene();    //go to next scene
            }
        }
    }

    //call adequate function based on scene to check if level finished
    private bool LevelCompleted()
    {
        string scenesName = SceneManager.GetActiveScene().name;

        if (scenesName == Scenes["Connect"]) return false;
        if (scenesName == Scenes["Wires"]) return GameObject.Find("WireManager").GetComponent<WiresManager>().IsLevelFinished();
        if (scenesName == Scenes["Cells"]) return GameObject.Find("ThisSceneManager").GetComponent<DialogueTrigger>().IsLevelFinished();

        return true;
    }

    //find next scene and load it based on current scene
    private void LoadNextScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Scenes["Cells"])
        {
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
        else if (sceneName == Scenes["MainRoom"])
        {
            if(NextSceneDoor != null)
            {
                switch (NextSceneDoor)
                {
                    case "Crate":
                        LoadNextCrate();
                        break;
                    case "Pipe":
                        LoadNextPipe();
                        break;
                    case "Arrows":
                        LoadNextArrows();
                        break;
                    case "Wires":
                        LoadNextWires();
                        break;
                    default:    //LabyInvisible Donjon
                        PhotonNetwork.LoadLevel(Scenes[NextSceneDoor]);
                        break;
                }
            }
        }
        else //is in level
        {
            switch (sceneName)
            {
                case "Crate":
                    LoadNextCrate();
                    break;
                case "Pipe":
                    LoadNextPipe();
                    break;
                case "Arrows":
                    LoadNextArrows();
                    break;
                case "Wires":
                    LoadNextWires();
                    break;
                default:    //LabyInvisible Donjon
                    PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
                    break;
            }

        }
    }

    private int PipeIndex = 0;
    private void LoadNextPipe()
    {
        if(PipeIndex <= 3)
        {
            PhotonNetwork.LoadLevel(Scenes["Pipe"]);   //load scene pipe
            GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().MapSize = Random.Range(5, 20);
            GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().StartGeneration();
            PipeIndex++;
        }
        else
        {
            PipeIndex = 0;
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
    }


    private string[] ListCrate = new string[] { "room_tuto1", "room_tuto2", "room_tuto3", "room_tuto4", "room_tuto5", "room_2_1"
    , "room_2_2", "room_2_3", "room_2_4", "room_2_5", "room_3_1", "room_4_1"};
    private int CrateIndex = 0;
    private void LoadNextCrate()
    {
        if(CrateIndex < ListCrate.Length)
        {
            PhotonNetwork.LoadLevel(Scenes["Crate"]);   //load scene crate
            GameObject.FindGameObjectsWithTag("BoxLabyGenerator")[0].GetComponent<CrateLabyrinthGenerator>().loadScene(ListCrate[CrateIndex]);      //generate enigm
            CrateIndex++;
        }
        else //everything done go to main room
        {
            CrateIndex = 5; //skip tutos
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
    }

    private int ArrowIndex = 0;
    private void LoadNextArrows()
    {
        if (ArrowIndex == 0) //tutos
        {

        }
        else if(ArrowIndex <= 2)
        {

        }
        else //everything done go to main room
        {
            ArrowIndex = 1;     //skip tuto
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
    }

    private void LoadNextWires()
    {

    }




}