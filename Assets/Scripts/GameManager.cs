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

    private string[] LabyBoxNext = new string[] { "room_tuto1.json", "room_tuto2.json", "room_tuto3.json", "room_tuto4.json", "room_tuto5.json", "room_2_1.json"
    , "room_2_2.json", "room_2_3.json", "room_2_4.json", "room_2_5.json", "room_3_1.json", "room_4_1.json", "loby"};

    private int LabyBoxNextInt = 0;


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
        DontDestroyOnLoad(gameObject); //ne pas supprimer un objet quand on change de scene
    }

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "CrateLabyrinthScene")
        {
            if (LabyBoxNextInt == 12)
            {
                //charger le loby  
            }
            GameObject.FindGameObjectsWithTag("BoxLabyGenerator")[0].GetComponent<CrateLabyrinthGenerator>().loadScene(LabyBoxNext[LabyBoxNextInt]);
            LabyBoxNextInt++;
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