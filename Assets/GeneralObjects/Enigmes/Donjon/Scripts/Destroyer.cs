using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroyer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(other.gameObject); // Destroy room that overlaps with center room
	}
}
