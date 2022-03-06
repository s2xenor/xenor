using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace MazeGenerator
{
    public enum PickMethod : byte { Newest, Oldest, Random, Cyclic };

    [Flags]
    public enum Direction : byte { North = 0x1, West = 0x2, South = 0x4, East = 0x8 };

    class Maze
    {
        public Byte[,] maze { get; private set; }
        public UInt16[] spawnpoint { get; private set; }
        public UInt16[] exitpoint { get; private set; }
        public UInt16 width { get; private set; }
        public UInt16 height { get; private set; }

        public Random random = new Random((int)DateTime.Now.Ticks & (0x0000FFFF));

        private UInt16 CyclePick = 0;

        public Maze(UInt16 width, UInt16 length, UInt16 startx = 0, UInt16 starty = 0)
        {
            Debug.Print(string.Format("W={0} L={1}", width, length));

            this.width = width;
            this.height = height;

            maze = BuildBaseMaze(width, length);
        }


        public void cell(UInt16 x, UInt16 y, byte value = 0)
        {
            if (x <= this.width && y <= this.height)
            {
                maze[x, y] = value;
            }
        }


        public Byte[,] BuildBaseMaze(UInt16 width, UInt16 length)
        {
            Byte[,] maze = new Byte[width, length];

            // with this you're able to shape the maze "body", ie not using a regular rectangle/square

            //maze[2, 0] = maze[3, 0] = maze[4, 0] = maze[5, 0] = 255;
            //maze[2, 1] = maze[3, 1] = maze[4, 1] = maze[5, 1] = 255;
            //maze[3, 2] = 255;
            //maze[9, 3] = maze[9, 4] = maze[8, 4] = 255;

            return maze;
        }


        public void dumpMaze()
        {
            if (maze != null)
            {
                for (UInt16 y = 0; y < maze.GetLength(1); y++)
                {
                    string xline = string.Empty;

                    for (UInt16 x = 0; x < maze.GetLength(0); x++)
                    {
                        xline += ' ' + maze[x, y].ToString();
                    }

                    Debug.Print(string.Format("M[{0}]={1}", y, xline));
                }
            }
        }


        public List<UInt16[]> GenerateTWMaze_GrowingTree()
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
                Int16 index = (Int16)chooseIndex((UInt16)cells.Count, PickMethod.Newest);
                UInt16[] cell_picked = cells[index];

                x = cell_picked[0];
                y = cell_picked[1];
                CarvedMaze.Add(new UInt16[3] { x, y, 1 });

                Direction[] tmpdir = RandomizeDirection();

                foreach (Direction way in tmpdir)  //shuffled dir
                //foreach (Direction way in DirectionArray) //    Enum.GetValues(typeof(Direction)))    //sequential dir, for test
                {
                    SByte[] move = DoAStep(way);

                    nx = (Int16)(x + move[0]);
                    ny = (Int16)(y + move[1]);

                    if (nx >= 0 && ny >= 0 && nx < maze.GetLength(0) && ny < maze.GetLength(1) && maze[nx, ny] == 0)
                    {
                        CarvedMaze.Add(new UInt16[3] { (UInt16)nx, (UInt16)ny, 2 });

                        //Debug.Print(string.Format("xy=[{0},{1}] Unvisited cell [nx,ny]={2},{3}", x, y, nx, ny));

                        //Debug.Print(string.Format("B maze[x,y]={0} maze[nx,ny]={1}", maze[x, y], maze[nx, ny]));
                        maze[x, y] |= (byte)way;
                        maze[nx, ny] |= (byte)OppositeDirection(way);
                        //Debug.Print(string.Format("A maze[x,y]={0} maze[nx,ny]={1}", maze[x, y], maze[nx, ny]));

                        //Debug.Print(string.Format("add cell [{0}][{1}]", nx, ny));
                        cells.Add(new UInt16[2] { (UInt16)nx, (UInt16)ny });
                        //Debug.Print(string.Format("Cells #={0}", cells.Count));

                        index = -1;
                         CarvedMaze.Add(new UInt16[3] { (UInt16)nx, (UInt16)ny, 3 });
                        break;
                    }
                    else
                    {
                        //Debug.Print("out of maze cells or already processed");
                    }
                }
                //**end dir loop
                //Debug.Print("==END DIR LOOP==");

                //delete this cell from list if none found
                if (index != -1)
                {
                    UInt16[] cell_removed = cells[index];

                    cells.RemoveAt(index);
                    //Debug.Print(string.Format("Cells {0} [{1},{2}] removed, #={3}", index, cell_removed[0], cell_removed[1], cells.Count));
                    CarvedMaze.Add(new UInt16[3] { (UInt16)x, (UInt16)y, 4 });
                }

                //Debug.Print("=== CELL LOOP END ===");
            }

            return CarvedMaze;
        }


        public UInt16 chooseIndex(UInt16 max, PickMethod pickmet)
        {
            UInt16 index = 0;

            switch (pickmet)
            {
                case PickMethod.Cyclic:
                    CyclePick = (UInt16)((CyclePick + 1) % max);
                    index = CyclePick;
                    break;

                case PickMethod.Random:
                    Random random = new Random((int)DateTime.Now.Ticks & (0x0000FFFF));
                    index = (UInt16)(random.Next(max - 1));
                    break;

                case PickMethod.Oldest:
                    index = 0;
                    break;

                case PickMethod.Newest:
                default:
                    if (max >= 1)
                    {
                        index = (UInt16)(max - 1);
                    }
                    else
                    {
                        index = 0;
                    }

                    break;
            }
            return index;
        }


        public Direction chooseARandomDirection()
        {
            Direction randir;

            var EnumToArray = Enum.GetValues(typeof(Direction));
            Byte tmp1 = (Byte)(random.Next(EnumToArray.Length - 1));
            randir = (Direction)EnumToArray.GetValue(tmp1);

            Debug.Print(string.Format("Dir=[{0}]", randir));

            return randir;
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
            //Random _random = new Random();

            int n = array.Length;

            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }


        public Direction OppositeDirection(Direction forward)
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


        public SByte[] DoAStep(Direction facingDirection)
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


        public Byte[,] LineToBlock()
        {
            Byte[,] blockmaze;

            if (maze == null || maze.GetLength(0) <= 1 && maze.GetLength(1) <= 1)
            {
                return null;
            }

            blockmaze = new Byte[2 * maze.GetLength(0) + 1, 2 * maze.GetLength(1) + 1];

            for (UInt16 wall = 0; wall < 2 * maze.GetLength(1) + 1; wall++)
            {
                blockmaze[0, wall] = 1;
            }

            for (UInt16 wall = 0; wall < 2 * maze.GetLength(0) + 1; wall++)
            {
                blockmaze[wall, 0] = 1;
            }

            for (UInt16 y = 0; y < maze.GetLength(1); y++)
            {
                for (UInt16 x = 0; x < maze.GetLength(0); x++)
                {
                    blockmaze[2 * x + 1, 2 * y + 1] = 0;

                    //Debug.Print(string.Format("M[{0},{1}]={2} & dir={3}", x, y, maze[x, y], maze[x, y] & (Byte)Direction.East));
                    if ((maze[x, y] & (Byte)Direction.East) != 0)
                    {
                        blockmaze[2 * x + 2, 2 * y + 1] = 0; // B
                    }
                    else
                    {
                        blockmaze[2 * x + 2, 2 * y + 1] = 1;
                    }

                    if ((maze[x, y] & (Byte)Direction.South) != 0)
                    {
                        blockmaze[2 * x + 1, 2 * y + 2] = 0; // C
                    }
                    else
                    {
                        blockmaze[2 * x + 1, 2 * y + 2] = 1;
                    }


                    blockmaze[2 * x + 2, 2 * y + 2] = 1;
                }
            }

            return blockmaze;
        }
    }
}
