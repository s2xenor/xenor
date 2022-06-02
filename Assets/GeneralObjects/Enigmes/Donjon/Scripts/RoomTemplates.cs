using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

	bool beganTimer = false;

	void Update()
	{
		if (waitTime <= 0 && spawnedBoss == false)
		{
			for (int i = 0; i < rooms.Count; i++) 
			{
				if(i == rooms.Count-1)
				{
					if (PhotonNetwork.IsMasterClient)
						PhotonNetwork.Instantiate(boss.name, rooms[i].transform.position, Quaternion.identity); // Spawn boss
					
					spawnedBoss = true;

					GameObject.FindGameObjectWithTag("Loading").GetComponent<FetchCam>().Del();
				}
			}
		} 
		else if (beganTimer)
        {
			waitTime -= Time.deltaTime; // Update timer
        }
		else if (GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
			beganTimer = true;
        }
	}
}
