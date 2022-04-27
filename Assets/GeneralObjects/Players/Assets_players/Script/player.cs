using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//player class
public class player : MonoBehaviour
{
    //Attack of the player 
    public int Strength = 15;

    //inventory associate to the player 
    private inventaire inventory;
    // canvas of the inventory 
    [SerializeField] private case_design Case_Design;       

    private void Awake()
    {
        inventory = new inventaire();
        Case_Design.SetInventory(inventory);
    }
}
