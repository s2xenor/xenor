using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	public string name; // Name of talking person

	[TextArea(3, 10)]
	public string[] sentences; // Test said by the person

}
