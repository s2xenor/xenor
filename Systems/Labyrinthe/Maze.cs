using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace MazeGenerator
{
    /* This code was adapted from
     * https://infiniteproductionsblog.wordpress.com/maze-generation-in-csharp/
     */

    [Flags]
    public enum Direction : byte { North = 0x1, West = 0x2, South = 0x4, East = 0x8 };

    class Maze
    {
        public Byte[,] maze { get; private set; }
        public UInt16 width { get; private set; }
        public UInt16 height { get; private set; }

        public System.Random random = new System.Random((int)DateTime.Now.Ticks & (0x0000FFFF)); // Pseudo-random in CS

        public Maze(UInt16 width, UInt16 length, UInt16 startx = 0, UInt16 starty = 0)
        {
            this.width = width;
            this.height = height;

            maze = BuildBaseMaze(width, length);
        }

        public Byte[,] BuildBaseMaze(UInt16 width, UInt16 length) // Creates array of maze
        {
            Byte[,] maze = new Byte[width, length];

            return maze;
        }

        public List<UInt16[]> GenerateTWMaze_GrowingTree() // Creates maze
        {
            List<UInt16[]> CarvedMaze = new List<UInt16[]>();
            List<UInt16[]> cells = new List<UInt16[]>();

            Array DirectionArray = Enum.GetValues(typeof(Direction));
            
            UInt16 x = (Byte)(random.Next(maze.GetLength(0) - 1) + 1);
            UInt16 y = (Byte)(random.Next(maze.GetLength(1) - 1) + 1);
            Int16 nx;
            Int16 ny;

            CarvedMaze.Add(new UInt16[3] { x, y, 1 });

            cells.Add(new UInt16[2] { x, y });

            while (cells.Count > 0)
            {
                Int16 index = (Int16)chooseIndex((UInt16)cells.Count);
                UInt16[] cell_picked = cells[index];

                x = cell_picked[0];
                y = cell_picked[1];
                CarvedMaze.Add(new UInt16[3] { x, y, 1 });

                Direction[] tmpdir = RandomizeDirection();

                foreach (Direction way in tmpdir)  // Shuffled dir
                {
                    SByte[] move = DoAStep(way);

                    nx = (Int16)(x + move[0]);
                    ny = (Int16)(y + move[1]);

                    if (nx >= 0 && ny >= 0 && nx < maze.GetLength(0) && ny < maze.GetLength(1) && maze[nx, ny] == 0)
                    {
                        CarvedMaze.Add(new UInt16[3] { (UInt16)nx, (UInt16)ny, 2 });

                        maze[x, y] |= (byte)way;
                        maze[nx, ny] |= (byte)OppositeDirection(way);
                        
                        cells.Add(new UInt16[2] { (UInt16)nx, (UInt16)ny });

                        index = -1;
                        CarvedMaze.Add(new UInt16[3] { (UInt16)nx, (UInt16)ny, 3 });
                        break;
                    }
                }
                //**end dir loop

                // Deletes this cell from list if none found
                if (index != -1)
                {
                    UInt16[] cell_removed = cells[index];

                    cells.RemoveAt(index);
                    CarvedMaze.Add(new UInt16[3] { (UInt16)x, (UInt16)y, 4 });
                }
            }

            return CarvedMaze;
        }


        public UInt16 chooseIndex(UInt16 max) // Chooses index
        {
            UInt16 index = 0;

            if (max >= 1)
            {
                index = (UInt16)(max - 1);
            }

            return index;
        }


        public Direction[] RandomizeDirection()
        {
            Direction[] randir;

            Array tmparray = Enum.GetValues(typeof(Direction));

            randir = (Direction[])tmparray;

            Shuffle<Direction>(randir);

            return randir;
        }


        // comes from http://www.dotnetperls.com/fisher-yates-shuffle
        private void Shuffle<T>(T[] array)
        {
            int n = array.Length;

            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }


        public Direction OppositeDirection(Direction forward) // Direction current index can take
        {
            Direction opposite = Direction.North;

            switch (forward)
            {
                case Direction.North:
                    opposite = Direction.South;
                    break;
                case Direction.South:
                    opposite = Direction.North;
                    break;
                case Direction.East:
                    opposite = Direction.West;
                    break;
                case Direction.West:
                    opposite = Direction.East;
                    break;
            }

            return opposite;
        }

        public SByte[] DoAStep(Direction facingDirection) // Go to next cell in current direction
        {
            SByte[] step = { 0, 0 };

            switch (facingDirection)
            {
                case Direction.North:
                    step[0] = 0;
                    step[1] = -1;
                    break;
                case Direction.South:
                    step[0] = 0;
                    step[1] = 1;
                    break;
                case Direction.East:
                    step[0] = 1;
                    step[1] = 0;
                    break;
                case Direction.West:
                    step[0] = -1;
                    step[1] = 0;
                    break;
            }

            return step;
        }

        public Byte[,] LineToBlock() // Inserts wall cells in new array / Converts into array with walls
        {
            Byte[,] blockmaze;

            if (maze == null || maze.GetLength(0) <= 1 && maze.GetLength(1) <= 1) // Maze too small
            {
                return null;
            }

            blockmaze = new Byte[2 * maze.GetLength(0) + 1, 2 * maze.GetLength(1) + 1]; // Build new maze

            for (UInt16 wall = 0; wall < 2 * maze.GetLength(1) + 1; wall++) // Add walls on each side of the maze
            {
                blockmaze[0, wall] = 1;
            }

            for (UInt16 wall = 0; wall < 2 * maze.GetLength(0) + 1; wall++) // Add walls on each side of the maze
            {
                blockmaze[wall, 0] = 1;
            }

            for (UInt16 y = 0; y < maze.GetLength(1); y++)
            {
                for (UInt16 x = 0; x < maze.GetLength(0); x++) // Browse maze
                {
                    blockmaze[2 * x + 1, 2 * y + 1] = 0;

                    if ((maze[x, y] & (Byte)Direction.East) != 0)
                        blockmaze[2 * x + 2, 2 * y + 1] = 0;
                    else
                        blockmaze[2 * x + 2, 2 * y + 1] = 1; // Add wall

                    if ((maze[x, y] & (Byte)Direction.South) != 0)
                        blockmaze[2 * x + 1, 2 * y + 2] = 0;
                    else
                        blockmaze[2 * x + 1, 2 * y + 2] = 1; // Add wall


                    blockmaze[2 * x + 2, 2 * y + 2] = 1;
                }
            }

            return blockmaze;
        }

        public void Print() // Print maze
        {
            string str = string.Empty;
            for (UInt16 i = 0; i < maze.GetLength(0); i++)
            {
                for (UInt16 j = 0; j < maze.GetLength(1); j++)
                {
                    str += maze[i, j].ToString();
                }
                str += "\n";
            }
            UnityEngine.Debug.Log(str);
        }
    }
}
