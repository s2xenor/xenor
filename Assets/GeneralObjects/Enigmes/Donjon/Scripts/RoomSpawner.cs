using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomSpawner : MonoBehaviour {

	public int openingDirection;
	// 1 --> need bottom door
	// 2 --> need top door
	// 3 --> need left door
	// 4 --> need right door

	private RoomTemplates templates;
	public bool spawned = false;

	public float waitTime = 4f;

	bool master;

	void Awake()
	{
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		master = PhotonNetwork.IsMasterClient;
	}

    private void Update()
    {
		if (!spawned && GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
			Destroy(gameObject, waitTime); // Destroy this in waitTime seconds

			Invoke("Spawn", 0.1f); // Call Spawn() in 0.1 second
        }
	}

	void Spawn() // Spawn rooms
	{
		if (!master)
        {
			spawned = true;
        }
		else if (!spawned)
		{
			int rand;
			if(openingDirection == 1) // Top door
			{
				// Need to spawn a room with a BOTTOM door.
				rand = Random.Range(0, templates.bottomRooms.Length); // Random index
				PhotonNetwork.Instantiate(templates.bottomRooms[rand].name, transform.position, templates.bottomRooms[rand].transform.rotation); // Choose random room
			} 
			else if(openingDirection == 2) // Bottom coor
			{
				// Need to spawn a room with a TOP door.
				rand = Random.Range(0, templates.topRooms.Length);
				PhotonNetwork.Instantiate(templates.topRooms[rand].name, transform.position, templates.topRooms[rand].transform.rotation);
			} 
			else if(openingDirection == 3) // Right door
			{
				// Need to spawn a room with a LEFT door.
				rand = Random.Range(0, templates.leftRooms.Length);
				PhotonNetwork.Instantiate(templates.leftRooms[rand].name, transform.position, templates.leftRooms[rand].transform.rotation);
			} 
			else if(openingDirection == 4) // Left door
			{
				// Need to spawn a room with a RIGHT door.
				rand = Random.Range(0, templates.rightRooms.Length);
				PhotonNetwork.Instantiate(templates.rightRooms[rand].name, transform.position, templates.rightRooms[rand].transform.rotation);
			}
			spawned = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("SpawnPoint")) // If collides with another SpawnPoint
		{
			if(!other.GetComponent<RoomSpawner>().spawned && !spawned) // If both spawned at the same time
			{
				PhotonNetwork.Instantiate(templates.closedRoom.name, transform.position, Quaternion.identity); // Spawn closed door
				Destroy(gameObject); // Destroy SpawnPoint
			} 
			spawned = true;
		}
	}
}
