using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private inventaire inventory;
    [SerializeField] private design_inventory design;

    private void Awake()
    {
        inventory = new inventaire();
        design.SetInventory(inventory);
    }
}
