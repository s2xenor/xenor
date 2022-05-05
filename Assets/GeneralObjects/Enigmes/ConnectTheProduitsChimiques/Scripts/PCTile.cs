using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCTile
{
    public enum PCTileType
    {
        None = 0,
        Strait = 1,
        Corner = 2,
        Cross = 3,
        Source = 4
    }

    public enum PCFluidDirection
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3,
        None = 4,
        End = 5
    }

    public enum PCFluidColor
    {
        blue = 0,
        pink = 1,
        green = 2
    }

    private PCTileType tileType;
    public PCTileType TileType => tileType;

    

    private PCFluidDirection fluidDirection; 
    public PCFluidDirection FluidDirection => fluidDirection;
    private PCFluidDirection fluidCommingDirection = PCFluidDirection.None;
    public PCFluidDirection FluidCommingDirection => fluidCommingDirection;

    private PCFluidDirection fluidDirection2;
    public PCFluidDirection FluidDirection2 => fluidDirection2;
    private PCFluidDirection fluidCommingDirection2 = PCFluidDirection.None;
    public PCFluidDirection FluidCommingDirection2 => fluidCommingDirection2;

    public PCTile(PCTileType tileType = PCTileType.None, PCFluidDirection fluidDirection = PCFluidDirection.None)
    {
        this.tileType = tileType;
        this.fluidDirection = fluidDirection;
    }

    public void AddDirection(PCFluidDirection enterDir, PCFluidDirection exitDir)
    {
        if (((int)enterDir + (int)exitDir) % 2 == 1)
        {
            if (TileType != PCTileType.None)
            {
                throw new ArgumentException("Bug c pas censé faire ça doit etre traité avant");
            }
            tileType = PCTileType.Corner;
            fluidCommingDirection = enterDir;
            fluidDirection = exitDir;
        }
        else
        {
            if (TileType == PCTileType.None)
            {
                tileType = PCTileType.Strait;
                fluidCommingDirection = enterDir;
                fluidDirection = exitDir;
            }
            else
            {
                //2eme tuayux qui passse sur cette case
                tileType = PCTileType.Cross;
                fluidDirection2 = exitDir;
                fluidCommingDirection2 = enterDir;
            }
        }
    }
}
