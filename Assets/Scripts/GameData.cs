using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Dictionary<string, bool> LevelsCompleted;
    public int[] LevelIndex;
    public int[] Inventory;
    public int[] Hearts;
    public GameData(Dictionary<string, bool> LevelsCompleted, int[] LevelIndex, int[] Inventory, int[] Hearts)
    {
        this.LevelIndex = LevelIndex;
        this.LevelsCompleted = LevelsCompleted;
        this.Inventory = Inventory;
        this.Hearts = Hearts;
    }


}
