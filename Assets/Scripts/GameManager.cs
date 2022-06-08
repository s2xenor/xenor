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
    private int TimestampStart;
    public int TimestampEnd;


    private string sceneName = "";
    private string NextScene = null;
    public string NextSceneDoor;        //string for the next scene defined by single door in lobby mainly
    public GameObject menu;
    /*
     * Variables Publiques des trucs qu'il y a � sauvgarder
     */
    public PlayerSettings playerSettings = new PlayerSettings();

    //NE PAS OUBLIER : idem aevc inventory
    // public Inventory inventory = new Inventory();

    public bool continuePrevGame = false; //player continue prev game or not

    //final scene
    private int score;


    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject); //ne pas supprimer un objet quand on change de scene
    }

    private void Update()
    {
        // Show pause menu when escape is pressed
        if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "FinalScene")
            {
                GameObject[] pauseMenu = GameObject.FindGameObjectsWithTag("Pause");
                int n = pauseMenu.Length;

                // If pause menu already exists, destroy id
                if (n != 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Destroy(pauseMenu[0]);
                    }
                }
                else
                {
                    Instantiate(menu, Vector2.zero, Quaternion.identity);
                }
            }
        }


        if (PhotonNetwork.IsMasterClient)   //cheat code 
        {
            if (Input.GetKeyDown(KeyCode.M)) //go to main room
            {
                GoBackToLobby();
            }
            else if (Input.GetKeyDown(KeyCode.N)) //go to next room
            {
                sceneName = SceneManager.GetActiveScene().name;
                PhotonNetwork.LoadLevel("Loading");   //load scene load
                Invoke("LoadNextScene", 0.5f);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                NextScene = "FinalScene";
                PhotonNetwork.LoadLevel("Loading");   //load scene load
                Invoke("LoadNextScene", 0.5f);
            }
        }
    }

    public void SaveData()
    {
        // On utilise le module JsonUtility pour parse tout l'objet
        GameData dataToSave = new GameData(LevelsCompleted, new int[] { CrateIndex, PipeIndex, LabyInviIndex, ArrowIndex, WiresIndex }, new int[] { }, new int[] { });
        string playerSettingsData = JsonUtility.ToJson(dataToSave);

        // Chemin ou va �tre enregistrer le JSON
        // persistentDataPath est un dossier qui ne sera jamais modifier par unity m�me mise � jour
        string filePath = Application.persistentDataPath + "/Game.json";

        // On �crit le fichier
        System.IO.File.WriteAllText(filePath, playerSettingsData);
    }

    public bool LoadData()
    {
        // Chemin ou est stock� le json
        string filePath = Application.persistentDataPath + "/Game.json";

        if (System.IO.File.Exists(filePath))
        {
            // On le parse pour r�cup les infos
            string data = System.IO.File.ReadAllText(filePath);

            GameData datas = JsonUtility.FromJson<GameData>(data);

            LevelsCompleted = datas.LevelsCompleted;    //load levels completed

            //load level in a room
            CrateIndex = datas.LevelIndex[0];
            PipeIndex = datas.LevelIndex[1];
            LabyInviIndex = datas.LevelIndex[2];
            ArrowIndex = datas.LevelIndex[2];
            WiresIndex = datas.LevelIndex[4];
            return true;
        }
        return false;

    }

    public void ContinueGame() {
        if (LoadData()) //if could load prev game
        {
            continuePrevGame = true;
        }
    }




    /*
     * MULTI 
     */

    private Dictionary<string, string> Scenes;  //Dictionnary allowing to change scene name without messing with code
    public static Dictionary<string, bool> LevelsCompleted;

    public void Start()
    {

        Scenes = new Dictionary<string, string>
        {
            { "Crate", "CrateLabyrinthScene" },
            { "Pipe", "ConnectTheProduitsChimiquesScene" },
            { "LabyInvisible", "LabyInv" },
            { "Arrows", "ArrowScene" },
            { "Wires", "WiresScene" },
            { "Donjon", "Donjon" },
            { "Cells", "Cells" },
            { "MainRoom", "MainRoom" },
            { "FinalScene", "FinalScene" },
        };

        LevelsCompleted = new Dictionary<string, bool>
        {
            { "Crate", false },
            { "Pipe", false },
            { "LabyInvisible", false },
            { "Arrows", false },
            { "Wires", false },
            { "Donjon", false },
        };

    }


    //called when a pressure plate is pressed
    int doorActivated = 0;
    public void DoorUpdate(int increment, bool doubleD)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("door update; incremnet: " + increment + "; dooractivated: "+ increment+doorActivated);
            doorActivated += increment; //number of pressure pressed
            if((doubleD && doorActivated >= 2) || (!doubleD && doorActivated >= 1)) //test is good number is pressed based on type of door
            {
                if (IsLevelCompleted())
                {
                    doorActivated = 0;  //reset
                    sceneName = SceneManager.GetActiveScene().name;
                    PhotonNetwork.LoadLevel("Loading");   //load scene load
                    Invoke("LoadNextScene", 0.5f);
                }
            }
        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("DoorUpdate", RpcTarget.MasterClient, increment, doubleD);
        }
    }

    //call adequate function based on scene to check if level finished
    private bool IsLevelCompleted()
    {
        string scenesName = SceneManager.GetActiveScene().name;

        if (scenesName == Scenes["Pipe"]) return GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().IsLevelFinished();
        if (scenesName == Scenes["Wires"]) return GameObject.Find("WireManager").GetComponent<WiresManager>().IsLevelFinished();
        if (scenesName == Scenes["Cells"]) return GameObject.Find("ThisSceneManager").GetComponent<DialogueTrigger>().IsLevelFinished();

        return true;
    }


    //find next scene and load it based on current scene
    public void LoadNextScene()
    {
        Debug.Log("next scene");
        
        Debug.Log(sceneName);
        if (NextScene != null)
        {
            Debug.Log("next scene diff null");
            PhotonNetwork.LoadLevel(Scenes[NextScene]);
            NextScene = null;
        }
        else if (sceneName == Scenes["Cells"])
        {
            TimestampStart = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
        else if (sceneName == Scenes["FinalScene"])
        {
            //todo
        }
        else if (sceneName == Scenes["MainRoom"])
        {
            if (NextSceneDoor != null)
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
            if (sceneName == Scenes["Crate"]) LoadNextCrate();
            else if (sceneName == Scenes["Pipe"]) LoadNextPipe();
            else if (sceneName == Scenes["Arrows"]) LoadNextArrows();
            else if (sceneName == Scenes["Wires"]) LoadNextWires();
            else if (sceneName == Scenes["LabyInvisible"]) LoadNextLabyInvi();
            else //donjon
            {
                LevelsCompleted["Donjon"] = true;
                PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
            }
        }
    }

    private int PipeIndex = 0;
    private void LoadNextPipe()
    {
        if(PipeIndex == 0)//tutos
        {
            PhotonNetwork.LoadLevel(Scenes["Pipe"]);   //load scene pipe
            Invoke("SubNextPipe", 0.5f);

        }
        else if(PipeIndex <= 4) //3 levels after
        {
            PhotonNetwork.LoadLevel(Scenes["Pipe"]);   //load scene pipe
            Invoke("SubNextPipe", 0.5f);

        }
        else
        {
            PipeIndex = 1;
            LevelsCompleted["Pipe"] = true;
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
    }

    private void SubNextPipe()
    {
        if (PipeIndex == 0) GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().StartDialogue(); //tutos should start dialogue
        else GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().MapSize = Random.Range(5, 20);

        GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().ShouldStartGeneration();

        PipeIndex++;
    }


    private string[] ListCrate = new string[] { "room_tuto1", "room_tuto2", "room_tuto3", "room_tuto4", "room_tuto5", "room_2_1"
    , "room_2_2", "room_2_3", "room_2_4", "room_2_5", "room_3_1", "room_4_1"};
    private int CrateIndex = 0;
    private void LoadNextCrate()
    {
        Debug.Log("load next crate");
        if(CrateIndex < ListCrate.Length)
        {
            PhotonNetwork.LoadLevel(Scenes["Crate"]);   //load scene crate
            Invoke("SubNextCrate", 0.5f);
        }
        else //everything done go to main room
        {
            LevelsCompleted["Crate"] = true;
            CrateIndex = 5; //skip tutos
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
    }

    private void SubNextCrate()
    {
        GameObject.FindGameObjectWithTag("BoxLabyGenerator").GetComponent<CrateLabyrinthGenerator>().loadScene(ListCrate[CrateIndex]);      //generate enigm
        CrateIndex++;
    }

    private int ArrowIndex = 0;
    private void LoadNextArrows()
    {
        
        if(ArrowIndex <= 2)
        {
            PhotonNetwork.LoadLevel(Scenes["Arrows"]);
            Invoke("LoadArrows", 0.5f);
        }
        else //everything done go to main room
        {
            LevelsCompleted["Arrows"] = true;
            ArrowIndex = 1;     //skip tuto
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
        }
    }

    private void LoadArrows()
    {
        //create small laby for tutos on master
        if (ArrowIndex == 0)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<ArrowsManager>().x = 7;
            GameObject.FindGameObjectWithTag("Manager").GetComponent<ArrowsManager>().y = 7;
            GameObject.FindGameObjectWithTag("Manager").GetComponent<ArrowsManager>().StartDialogue();

        }
        GameObject.FindGameObjectWithTag("Manager").GetComponent<ArrowsManager>().shouldStart = true;
        ArrowIndex++;
    }

    private int WiresIndex = 0;
    private void LoadNextWires()
    {
        if(WiresIndex == 0) //tutos
        {
            PhotonNetwork.LoadLevel(Scenes["Wires"]);
            Invoke("LoadWires", 0.5f);
            WiresIndex++;
        }
        else if(WiresIndex < 2)
        {
            PhotonNetwork.LoadLevel(Scenes["Wires"]);
            WiresIndex++;
        }
        else
        {
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
            LevelsCompleted["Wires"] = true;
            WiresIndex = 0;
        }

    }

    private void LoadWires()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<WiresManager>().StartDialogue();
    }


    int LabyInviIndex = 0;
    private void LoadNextLabyInvi()
    {
        if (LabyInviIndex < 2)
        {
            LabyInviIndex++;
            PhotonNetwork.LoadLevel(Scenes["LabyInvisible"]);
        }
        else
        {
            PhotonNetwork.LoadLevel(Scenes["MainRoom"]);
            LevelsCompleted["LabyInvisible"] = true;
            LabyInviIndex = 0;
        }

    }

    private void GoBackToOneLevel()
    {
        //remove one level from the scene exited preventing skipping levels
        if (sceneName == Scenes["Crate"]) CrateIndex = CrateIndex > 0 ? CrateIndex-1 : CrateIndex;
        else if (sceneName == Scenes["Pipe"]) PipeIndex = PipeIndex > 0 ? PipeIndex - 1 : PipeIndex;
        else if (sceneName == Scenes["Arrows"]) ArrowIndex = ArrowIndex > 0 ? ArrowIndex - 1 : ArrowIndex;
        else if (sceneName == Scenes["Wires"]) WiresIndex = WiresIndex > 0 ? WiresIndex - 1 : WiresIndex;
        else if (sceneName == Scenes["LabyInvisible"]) LabyInviIndex = LabyInviIndex > 0 ? LabyInviIndex - 1 : LabyInviIndex;
    }

    public void GoBackToLobby()
    {
        sceneName = SceneManager.GetActiveScene().name;
        GoBackToOneLevel();
        PhotonNetwork.LoadLevel("Loading");   //load ²cene load
        NextScene = "MainRoom";
        Invoke("LoadNextScene", 0.5f);
    }

    public int CalculateScore()
    {
        int secDuration = TimestampEnd - TimestampStart;
        int START_SCORE = 15000;
        int COEF_LIFE = 10;
        int COEF_TIME = 1;
        score = START_SCORE - COEF_LIFE * QuarterHeartLost - COEF_TIME * secDuration;
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<FinalScene>().SetScore(score);
        }

        return score;
    }



    
}