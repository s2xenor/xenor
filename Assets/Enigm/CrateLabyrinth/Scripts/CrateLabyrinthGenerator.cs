using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Récupérer dans une variable le canvas d'ineraction
        GameObject canvaTextPopUP = GameObject.Find("TextPopUpCanvas");

        //Ajouter toutes les Boites
        //GameObject.Instantiate
        Instantiate(movableCratePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //Lier aux boites le canvas d'intéraction
        foreach (GameObject movableCrate in GameObject.FindGameObjectsWithTag("Box"))
        {
            movableCrate.GetComponent<MovableCrate>().MessageOnScreenCanvas = messageOnScreenCanvas;
        }
    }
}
 
