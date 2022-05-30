using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ce script permet de gere tout ce qui doit passer entre les scenes
// Egalement les sauvegarde
// Il permet de sauvegarder des données que l'on souhaite récupérer après le chargement de la scène suivante
// ATTENTION : ce script ne doit être placé qu'une seule fois dans le jeu !!!!!!!!!!!!!!!!! Lors de la première scene.
// ATTENTION : On ne doit pas pouvoir revenir sur la première scène car sinon le script va se dupliquer ce qui entraine des bugs
// Conseil : Placer le script dans le menu de chargement.
// NE PAS OUBLIER : ajouter un EmptyObject nommé GameManger et contenant le script GameManager sur la première scène du jeu.
public class GameManager : MonoBehaviour
{
    /*
     * Variables Satiques
     * 
     * Permet que l'on puisse y accèder depuis n'importe quel script même s'il n'est pas dans la hierarchie
     */

    // Le Script GameManager lui-même
    // Permet que l'on puisse sauvergarder des données depuis n'importe où
    public static GameManager instance; // Va être egal au premier GameManager qu'il trouve dans le jeu


    /*
     * Variables Publiques des trucs qu'il y a à sauvgarder
     */
    public PlayerSettings playerSettings = new PlayerSettings();

    //NE PAS OUBLIER : idem aevc inventory
    // public Inventory inventory = new Inventory();

    private string[] LabyBoxNext = new string[] { "room_tuto1", "room_tuto2", "room_tuto3", "room_tuto4", "room_tuto5", "room_2_1"
    , "room_2_2", "room_2_3", "room_2_4", "room_2_5", "room_3_1", "room_4_1"};

    private int LabyBoxNextInt = 0;

    private bool loadNow = true;


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

        ResetSave(); // si on veut pas recupérer les données de la dernière session quand on relance le jeu.

        instance = this;
        SceneManager.sceneLoaded += LoadState; //maintenant quand on load une nouvelle scene on va aussi appeler le truc pour load les données
        SceneManager.sceneLoaded += ChargeCrateLabyrinthScene;
        DontDestroyOnLoad(gameObject); //ne pas supprimer un objet quand on change de scene
    }

    //Fct appellé a chaqque chargement de nouvelle scène (on rappelle que le gamemanager est un objet jamais détruit
    public void ChargeCrateLabyrinthScene(Scene s, LoadSceneMode mode)
    {
        //Si on est dans les salle d'énigme labybox
        //Alors on appelle le générateur de laby box avec la salle que l'on souhaite charger
        if (SceneManager.GetActiveScene().name == "CrateLabyrinthScene")
        {
            //Ola fct est appelé 2 fois au chargement de la scène donc on fais l'action qu'une seule fois sur 2
            if (loadNow)
            {
                //si on a dépaaséé le nb de salle
                if (LabyBoxNextInt >= 12)
                {
                    //charger le loby  
                    SceneManager.LoadScene("MainRoom");
                }
                else
                {
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
                    //on a chargé la salle donc on désarme
                    loadNow = false;
                }
            }
            else
            {
                //on réarme pou charger la salle au prochain appel
                loadNow = true;
            }
        }
    }

    //Fonction SaveState() de sauvegarder toutes les infos que l'on souhaite conserver d'une scene à l'autre
    /**
    * <summary>Permet de sauvegarder toutes les infos que l'on souhaite conserver d'une scene à l'autre</summary>
    * 
    * <returns>Return nothing</returns>
*/
    public void SaveState()
    {
        // On utilise le module JsonUtility pour parse tout l'objet
        // NE PAS OUBLIER : il faudra remplacer apr ce qu'il faut pour aller chercher l'inventaire
        // string inventoryData = JsonUtility.ToJson(inventory);

        // Chemin ou va être enregistrer le JSON
        // persistentDataPath est un dossier qui ne sera jamais modifier par unity même mise à jour
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        // On écrit le fichier
        // NE PAS OUBLIER : Réactiver quand se sera ok
        //System.IO.File.WriteAllText(filePath, inventoryData);
    }

    //Fonction SavePlayerSettings() de sauvegarder tous les settings du player après une modification
    /**
    * <summary>Permet de sauvegarder de sauvegarder tous les settings du player après une modification</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void SavePlayerSettings()
    {
        // On utilise le module JsonUtility pour parse tout l'objet
        string playerSettingsData = JsonUtility.ToJson(playerSettings);

        // Chemin ou va être enregistrer le JSON
        // persistentDataPath est un dossier qui ne sera jamais modifier par unity même mise à jour
        string filePath = Application.persistentDataPath + "/PlayerSettingsData.json";

        // On écrit le fichier
        System.IO.File.WriteAllText(filePath, playerSettingsData);
    }

    //Fonction SaveState() de récuperer les infos sauvgarder dans la scène précédente
    /**
    * <summary>Permet de récuperer les infos sauvgarder dans la scène précédente</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        // Chemin ou est stocké le json de l'inventaire
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        if (System.IO.File.Exists(filePath))
        {
            // On le parse pour récup les infos
            string inventoryData = System.IO.File.ReadAllText(filePath);

            // On recrée un inventaire avec les infos
            // NE PAS OUBLIER : Réactiver quand se sera ok
            //inventory = JsonUtility.FromJson<Inventory>(inventoryData);
        }

        // Chemin ou est stocké le json de l'inventaire
        filePath = Application.persistentDataPath + "/PlayerSettingsData.json";

        if (System.IO.File.Exists(filePath))
        {
            // On le parse pour récup les infos
            string playerSettingsData = System.IO.File.ReadAllText(filePath);

            // On recrée un playerSettings avec les infos
            // NE PAS OUBLIER : Réactiver quand se sera ok
            playerSettings = JsonUtility.FromJson<PlayerSettings>(playerSettingsData);
        }
    }

    //Fonction ResetSave() permet de supprimer toutes les infos stocké dans les Json
    /**
    * <summary>Permet de supprimer toutes les infos stocké dans les Json</summary>
    * 
    * <returns>Return nothing</returns>
    */
    public void ResetSave()
    {
        // Chemin ou est stocké le json de l'inventaire
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        // On détruit le fichier
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
        // Chemin ou est stocké le json de l'inventaire
        string filePath = Application.persistentDataPath + "/PlayerSettingsData.json";

        // On détruit le fichier
        System.IO.File.Delete(filePath);
    }
}