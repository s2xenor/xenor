using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	
	//Variable qui lient les elment d'affichage
	public Text nameText;
	public Text dialogueText;

	//Animateur pour les aimations mdr
	public Animator animator;

	//File qui stocke les phrase
	private Queue<string> sentences;

	private DialogueTrigger dialogueTrigger; //pour lancer la suite du dialogue

	// Use this for initialization
	void Start () 
	{
		sentences = new Queue<string>();
	}

	//Cette fonction est appelée pour démarer le dialogue
	public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger)
	{
		this.dialogueTrigger = dialogueTrigger;

		//Affichage de la boîte de dialogue
		animator.SetBool("IsOpen", true);

		//affiche du nom de celui qui parle dans la boite de dialogue
		nameText.text = dialogue.name; // Display name

		sentences.Clear(); // Clear previous sentences

		foreach (string sentence in dialogue.sentences) // Enqueue sentences to be said
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	//Afficahge de la phrase suivante pour ce perso
	public void DisplayNextSentence()
	{

		if (sentences.Count == 0) // If no more sentences to say
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence) // Make cool animation
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);

		//appel du prochain dialogue (perso qui parle)
		dialogueTrigger.TriggerDialogue();
	}

	public bool IsDialodFinished()
    {
		return sentences.Count == 0;
    }

}
