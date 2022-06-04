using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DialogueTrigger : MonoBehaviourPunCallbacks
{

	//nom du fichier json ^� est stock� le dialogue
	//NE PAS OUBLIER : de bien d�fnir le bon de premier ficheir l� oi� on mets le dialogue trigger
	public string filePath;

	public bool triggerOnload = false;

	public List<GameObject> objectsToActivate;

    public GameObject playerBoyPrefab;
    public GameObject playerGirlPrefab;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerBoyPrefab.name, new Vector2(0.92f, -0.3f), Quaternion.identity); // Spawn master player on network
        }
        else
        {
            PhotonNetwork.Instantiate(playerGirlPrefab.name, new Vector2(-0.37f, -0.3f), Quaternion.identity); // Spawn player on network
        }
    }
    private void Update()
    {
        if (triggerOnload)
        {
			TriggerDialogue();
			triggerOnload = false;
		}
    }

    public void TriggerDialogue()
	{
		if (filePath != "") {
			//Lecture du fichier json
			var dialoguesData = Resources.Load<TextAsset>(filePath);

			//cr�ation de l'objet qui stocke les phrases du dialogue en fonction du json
			Dialogue dialogue = JsonUtility.FromJson<Dialogue>(dialoguesData.text);

			//changement du chemin du nom du prochain fichier
			filePath = dialogue.nextDialogPath;

			//D�but du dialogue
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue, this);
		}
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ObjectfToActivate", RpcTarget.All);
        }
	}

    [PunRPC]
    public void ObjectfToActivate()
    {
        if (objectsToActivate != null && objectsToActivate.Count != 0)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }

    public bool IsLevelFinished()
    {
        return FindObjectOfType<DialogueManager>().IsDialodFinished();
    }
}
