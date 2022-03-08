using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private inventaire inventory;
    [SerializeField] private case_design Case_Design;

    private void Awake()
    {
        inventory = new inventaire();
        Case_Design.SetInventory(inventory);
    }
}
