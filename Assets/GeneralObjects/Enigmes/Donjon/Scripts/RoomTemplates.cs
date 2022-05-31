using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {

	public GameObject[] bottomRooms; // List of rooms with a door at the bottom
	public GameObject[] topRooms;
	public GameObject[] leftRooms;
	public GameObject[] rightRooms;

	public GameObject closedRoom; // Room without doors

	public List<GameObject> rooms; // List of rooms in dongeon

	public float waitTime; // Timer to spawn boss
	private bool spawnedBoss;
	public GameObject boss; // Boss room

	void Update()
	{
		if (waitTime <= 0 && spawnedBoss == false)
		{
			for (int i = 0; i < rooms.Count; i++) 
			{
				if(i == rooms.Count-1)
				{
					Instantiate(boss, rooms[i].transform.position, Quaternion.identity); // Spawn boss
					spawnedBoss = true;
				}
			}
		} 
		else
			waitTime -= Time.deltaTime; // Update timer
	}
}
