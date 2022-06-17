using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DialogueTriggerG : MonoBehaviourPunCallbacks
{

    //nom du fichier json ^� est stock� le dialogue
    //NE PAS OUBLIER : de bien d�fnir le bon de premier ficheir l� oi� on mets le dialogue trigger
    public string filePath;

    public bool triggerOnload = false;


    void Start()
    {
       
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
        if (filePath != "")
        {
            //Lecture du fichier json
            var dialoguesData = Resources.Load<TextAsset>(filePath);

            //cr�ation de l'objet qui stocke les phrases du dialogue en fonction du json
            Dialogue dialogue = JsonUtility.FromJson<Dialogue>(dialoguesData.text);

            //changement du chemin du nom du prochain fichier
            filePath = dialogue.nextDialogPath;

            //D�but du dialogue
            FindObjectOfType<DialogueManagerG>().StartDialogue(dialogue, this);
        }
        
    }

    public bool IsDialogueFinished()
    {
        return FindObjectOfType<DialogueManagerG>().IsDialogueFinished();
    }

}
