using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

	public int openingDirection;
	// 1 --> need bottom door
	// 2 --> need top door
	// 3 --> need left door
	// 4 --> need right door

	private RoomTemplates templates;
	public bool spawned = false;

	public float waitTime = 4f;

	void Start()
	{
		Destroy(gameObject, waitTime); // Destroy this in waitTime seconds
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		Invoke("Spawn", 0.1f); // Call Spawn() in 0.1 second
	}


	void Spawn() // Spawn rooms
	{
		if(!spawned)
		{
			int rand;
			if(openingDirection == 1) // Top door
			{
				// Need to spawn a room with a BOTTOM door.
				rand = Random.Range(0, templates.bottomRooms.Length); // Random index
				Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation); // Choose random room
			} 
			else if(openingDirection == 2) // Bottom coor
			{
				// Need to spawn a room with a TOP door.
				rand = Random.Range(0, templates.topRooms.Length);
				Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
			} 
			else if(openingDirection == 3) // Right door
			{
				// Need to spawn a room with a LEFT door.
				rand = Random.Range(0, templates.leftRooms.Length);
				Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
			} 
			else if(openingDirection == 4) // Left door
			{
				// Need to spawn a room with a RIGHT door.
				rand = Random.Range(0, templates.rightRooms.Length);
				Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
			}
			spawned = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("SpawnPoint")) // If collides with another SpawnPoint
		{
			if(!other.GetComponent<RoomSpawner>().spawned && !spawned) // If both spawned at the same time
			{
				Instantiate(templates.closedRoom, transform.position, Quaternion.identity); // Spawn closed door
				Destroy(gameObject); // Destroy SpawnPoint
			} 
			spawned = true;
		}
	}
}
