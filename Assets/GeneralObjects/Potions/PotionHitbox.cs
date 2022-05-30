using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Monster")
            gameObject.transform.parent.GetComponent<Potion>().Effect(collision);
    }
}
