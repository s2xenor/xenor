using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManagerG : MonoBehaviour
{

	//Variable qui lient les elment d'affichage
	public Text nameText;
	public Text dialogueText;

	//Animateur pour les aimations mdr
	public Animator animator;

	//AudioManarger
	public AudioManager audioManager;

	//File qui stocke les phrase
	private Queue<string> sentences;

	//File qui stocke les noms des fichier audios
	private Queue<string> audioNames;

	private DialogueTriggerG dialogueTrigger; //pour lancer la suite du dialogue

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
		audioNames = new Queue<string>();
		audioManager = FindObjectOfType<AudioManager>();
	}

	//Cette fonction est appel�e pour d�marer le dialogue
	public void StartDialogue(Dialogue dialogue, DialogueTriggerG dialogueTrigger)
	{
		this.dialogueTrigger = dialogueTrigger;

		//Affichage de la bo�te de dialogue
		animator.SetBool("IsOpen", true);

		//affiche du nom de celui qui parle dans la boite de dialogue
		nameText.text = dialogue.name; // Display name

		sentences.Clear(); // Clear previous sentences
		audioNames.Clear();

		foreach (string sentence in dialogue.sentences) // Enqueue sentences to be said
		{
			sentences.Enqueue(sentence);
		}

		foreach (string audioName in dialogue.audioName) // add � la file tous les noms des audios
		{
			audioNames.Enqueue(audioName);
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
		audioManager.Play(audioNames.Dequeue());

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
