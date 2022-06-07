using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class imageOnline : MonoBehaviour
{
    inventoryOnline inventaire;
    public bool PotionStrength = false;
    public bool PotionDamage = false;
    public bool PotionHealth = false;
    public Animator animator;

    GameManager gameManager;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inventaire = GameObject.FindGameObjectWithTag("Inventory").GetComponent<inventoryOnline>();

        if (inventaire != null)
        {
            if (PhotonNetwork.IsMasterClient) // Get potion number
            {
                inventaire.slot[index] = gameManager.potions[0, index];
            }
            else
            {
                inventaire.slot[index] = gameManager.potions[1, index];
            }

            inventaire.UpdateNumber(index, inventaire.slot[index].ToString());
        }
    }

    public void ShowImage()
    {
        int slot_number = transform.parent.GetSiblingIndex(); //number of slots

        if (inventaire.slot[slot_number] > 0)
        {
            inventaire.slot[slot_number] -= 1;
            inventaire.UpdateNumber(slot_number, inventaire.slot[slot_number].ToString());
            switch (slot_number)
            {
                case 0://healt
                    PotionHealth = true;
                    break;

                case 1://damage
                    PotionDamage = true;
                    break;

                case 2://strength
                    PotionStrength = true;
                    break;
            }
        }
    }

    private void Update()
    {
        if (inventaire != null)
        {
            if (PhotonNetwork.IsMasterClient) // Get potion number
            {
                gameManager.potions[0, index] = inventaire.slot[index];
            }
            else
            {
                gameManager.potions[1, index] = inventaire.slot[index];
            }

            inventaire.UpdateNumber(index, inventaire.slot[index].ToString());
        }
    }
}
