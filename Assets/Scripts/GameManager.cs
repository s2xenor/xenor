using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ce script permet de gere tout ce qui doit passer entre les scenes
// Egalement les sauvegarde
// Il permet de sauvegarder des donn�es que l'on souhaite r�cup�rer apr�s le chargement de la sc�ne suivante
// ATTENTION : ce script ne doit �tre plac� qu'une seule fois dans le jeu !!!!!!!!!!!!!!!!! Lors de la premi�re scene.
// ATTENTION : On ne doit pas pouvoir revenir sur la premi�re sc�ne car sinon le script va se dupliquer ce qui entraine des bugs
// Conseil : Placer le script dans le menu de chargement.
// NE PAS OUBLIER : ajouter un EmptyObject nomm� GameManger et contenant le script GameManager sur la premi�re sc�ne du jeu.
public class GameManager : MonoBehaviour
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
     * Variables Publiques des trucs qu'il y a � sauvgarder
     */
    public PlayerSettings playerSettings = new PlayerSettings();

    //NE PAS OUBLIER : idem aevc inventory
    // public Inventory inventory = new Inventory();


    private bool loadNow = true;

    /**
     * Variable pour labyrinthe de boite
     */
    private string[] LabyBoxNext = new string[] { "room_tuto1", "room_tuto2", "room_tuto3", "room_tuto4", "room_tuto5", "room_2_1"
    , "room_2_2", "room_2_3", "room_2_4", "room_2_5", "room_3_1", "room_4_1"};
    private int LabyBoxNextInt = 0;

    /**
     * Variable pour connecte les produits hcimiques
     */
    private int nbLabyDone = 0;

    /**
     * Menu GameObject
     */
    public GameObject menu;

    /*
     * Fonctions
     */

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
        SceneManager.sceneLoaded += ChargeCrateLabyrinthScene;
        DontDestroyOnLoad(gameObject); //ne pas supprimer un objet quand on change de scene
    }

    private void Update()
    {
        // Show pause menu when escape is pressed
        if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.Escape))
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

    //Fct appell� a chaqque chargement de nouvelle sc�ne (on rappelle que le gamemanager est un objet jamais d�truit
    public void ChargeCrateLabyrinthScene(Scene s, LoadSceneMode mode)
    {
        //Si on est dans les salle d'�nigme labybox
        //Alors on appelle le g�n�rateur de laby box avec la salle que l'on souhaite charger
        if (SceneManager.GetActiveScene().name == "CrateLabyrinthScene")
        {
            //Ola fct est appel� 2 fois au chargement de la sc�ne donc on fais l'action qu'une seule fois sur 2
            if (loadNow)
            {
                //si on a d�paas�� le nb de salle
                if (LabyBoxNextInt >= 12)
                {
                    //charger le loby  
                    SceneManager.LoadScene("MainRoom");
                }
                else
                {
                    //LabyBoxNextInt = 3;
                    Debug.Log(LabyBoxNext[LabyBoxNextInt]);
                    Debug.Log(LabyBoxNextInt);
                    //SInon on charge le salle suivante
                    GameObject.FindGameObjectsWithTag("BoxLabyGenerator")[0].GetComponent<CrateLabyrinthGenerator>().loadScene(LabyBoxNext[LabyBoxNextInt]);
                    //On change la salle suivante
                    LabyBoxNextInt++;
                    //si on a finit le tutoriel, alors on saute (ou non) des salles
                    if (LabyBoxNextInt >= 5)
                    {
                        LabyBoxNextInt += Random.Range(0, 3);
                    }
                    //on a charg� la salle donc on d�sarme
                    loadNow = false;
                }
            }
            else
            {
                //on r�arme pou charger la salle au prochain appel
                loadNow = true;
            }
        }
        //Sinon si on est dans la salle connecte les produits chimique
        else if (SceneManager.GetActiveScene().name == "ConnectTheProduitsChimiquesScene")
        {
            if (nbLabyDone >= 3)
            {
                //charger le loby  
                SceneManager.LoadScene("MainRoom");
            }
            else
            {
                //SInon on charge la salle
                nbLabyDone++;
                GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().MapSize = Random.Range(5, 25);
                if (nbLabyDone>=3)
                {
                    GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().NextSceneName = "MainRoom";
                }
                else
                {
                    GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().NextSceneName = "ConnectTheProduitsChimiquesScene";
                }

                GameObject.Find("PipeLabyGenerator").GetComponent<PCMap>().StartGeneration();
            }
        }
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
}