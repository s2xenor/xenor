using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class imageOnline : MonoBehaviour
{
    inventoryOnline inventaire;
    public bool PotionStrength = false;
    public bool PotionDamage = false;
    public bool PotionHealth = false;

    // Start is called before the first frame update
    void Start()
    {
        inventaire = GameObject.FindGameObjectWithTag("Inventory").GetComponent<inventoryOnline>();
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
}
