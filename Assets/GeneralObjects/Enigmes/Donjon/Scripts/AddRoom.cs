using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AddRoom : MonoBehaviour {

	private RoomTemplates templates;

	void Start()
	{
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		templates.rooms.Add(this.gameObject); // Add room to room list
	}
}
