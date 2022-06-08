using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class qui stocke le dialogue que va lire DialogueManager
[System.Serializable]
public class Dialogue {

	public string name; // Name of talking person

	public string[] sentences; // Test said by the person

	public string[] audioName;

	public string nextDialogPath; // Name of teh file contaning the next dialog to play
}