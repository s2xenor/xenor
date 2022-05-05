using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMazeGenerator
{
    private int mapSize;
    public int MapSize => mapSize;

    private PCTile[][] maze;
    public PCTile[][] Maze => maze;

    private List<(int, int)> startsAndEnds = new List<(int, int)>();
    public List<(int, int)> StartsAndEnds => startsAndEnds;

    public PCMazeGenerator()
    {
        mapSize = Random.Range(10, 50);
        maze = new PCTile[mapSize][];

        //Initialisation du labyrinthe
        InitMaze();

        //Création du labyrinthe de produit chimique
        GenerateMaze();
    }

    /**
     * <summary>Initialisation d'un labyrinthe vide</summary>
     */
    private void InitMaze()
    {
        //Initialisation du labyrinthe
        //Partie Haute du laby et basse du laby (milieur sera rajouter après)
        //milieu et un mur de vers inffranchissable et en dessous la tuile est à la bonne position
        for (int i = 0; i < mapSize; i++)
        {
            maze[i] = new PCTile[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                maze[i][j] = new PCTile();
            }
        }
    }

    /**
     * <summary>Renvoie toutes le dirrections possible depuis cette case/summary>
     * 
     * <param name="oldDir">Direction sortant de la tuile précendente</param>
     * <param name="i">ligne de l'actuel tuile à créer/modifier</param>
     * <param name="j">colone de l'actuelle tuile à créer/modifier</param>
     * 
     * <returns>List des directions possibles</returns>
     */
    private List<PCTile.PCFluidDirection> GetPossibleDirections(PCTile.PCFluidDirection oldDir, int i, int j)
    {
        List<PCTile.PCFluidDirection> result = new List<PCTile.PCFluidDirection>();

        PCTile tileActu = maze[i][j];
        if (tileActu.TileType == PCTile.PCTileType.Cross)
        {
            //on arrive sur un tuile déjà à son maximum de capacité donc c mort aucune sol
            return result;
        }

        if (tileActu.TileType == PCTile.PCTileType.Corner)
        {
            //aucune possibilité, c déjà le max pour ce type de tuile c mort aussi
            return result;
        }

        if (tileActu.TileType == PCTile.PCTileType.Strait)
        {
            //On peut faire une croix
            if ((int)oldDir % 2 != (int)tileActu.FluidDirection % 2)
            {
                result.Add(oldDir);
            }
            // on ne pourra pas tourner donc seule solution
            return result;
        }

        if (j - 1 >= 0 && oldDir != PCTile.PCFluidDirection.Right)
        {
            //y a un tuile a gauche donc on paut check
            PCTile tileGauche = maze[i][j - 1];
            if (tileGauche.TileType == PCTile.PCTileType.None)
            {
                result.Add(PCTile.PCFluidDirection.Left);
            }
            else if (tileGauche.TileType == PCTile.PCTileType.Strait && (int)PCTile.PCFluidDirection.Left % 2 != (int)tileGauche.FluidDirection % 2)
            {
                if (j - 2 >= 0)
                {
                    result.Add(PCTile.PCFluidDirection.Left);
                }
            }
        }

        if (j + 1 < mapSize && oldDir != PCTile.PCFluidDirection.Left)
        {
            PCTile tileDroite = maze[i][j + 1];
            if (tileDroite.TileType == PCTile.PCTileType.None)
            {
                result.Add(PCTile.PCFluidDirection.Right);
            }
            else if (tileDroite.TileType == PCTile.PCTileType.Strait && (int)PCTile.PCFluidDirection.Right % 2 != (int)tileDroite.FluidDirection % 2)
            {
                if (j + 2 < mapSize)
                {
                    result.Add(PCTile.PCFluidDirection.Right);
                }
            }
        }


        //regardez la new dir aussi
        if (i + 1 < mapSize)
        {
            PCTile tileBas = maze[i + 1][j];
            if (tileBas.TileType == PCTile.PCTileType.None)
            {
                result.Add(PCTile.PCFluidDirection.Down);
            }
            else if (tileBas.TileType == PCTile.PCTileType.Strait && (int)oldDir % 2 != (int)tileBas.FluidDirection % 2 && tileBas.FluidDirection != PCTile.PCFluidDirection.Down)
            {
                if (i + 2 <= mapSize)
                {
                    result.Add(PCTile.PCFluidDirection.Down);
                }
            }
        }
        else
        {
            result.Add(PCTile.PCFluidDirection.End);
        }

        return result;
    }

    /**
     * <summary>Génère le chemin pour ce tuyaux</summary>
     * 
     * <param name="oldDir">Direction sortant de la tuile précendente</param>
     * <param name="i">ligne de l'actuel tuile à créer/modifier</param>
     * <param name="j">colone de l'actuelle tuile à créer/modifier</param>
     * <param name="color">couleur en cour de génération</param>
     * 
     * <returns>Integers de la colone du de la fin du tuyaux ou un NULL si chemin impossible</returns>
     */
    private int? GeneratePath(PCTile.PCFluidDirection oldDir, int i, int j, PCTile.PCFluidColor color)
    {
        if (i < 0 || j < 0 || i >= maze.Length || j >= maze.Length)
        {
            return null;
        }
        PCTile oldTile = maze[i][j];

        List<PCTile.PCFluidDirection> possibles = GetPossibleDirections(oldDir, i, j);

        if (possibles.Count == 0)
        {
            return null;
        }

        int nbAlea = Random.Range(0, possibles.Count);

        PCTile.PCFluidDirection choosenOne = possibles[nbAlea];

        //Changement de la Tuile
        if (choosenOne == PCTile.PCFluidDirection.End)
        {
            //c la fin des haricots
            maze[i][j].AddDirection(oldDir, PCTile.PCFluidDirection.Down, color);
            return j;
        }
        else
        {
            maze[i][j].AddDirection(oldDir, choosenOne, color);
        }

        //Calcul des nouvelles valeurs
        int newI = i;
        int newJ = j;
        if (choosenOne == PCTile.PCFluidDirection.Down)
        {
            newI++;
        }
        else if (choosenOne == PCTile.PCFluidDirection.Left)
        {
            newJ--;
        }
        else
        {
            newJ++;
        }


        //Récursions
        int? result = GeneratePath(choosenOne, newI, newJ, color);
        while (result == null)
        {
            //On rétabli la case comme elle était avant
            maze[i][j] = oldTile;
            possibles = GetPossibleDirections(oldDir, i, j);
            if (possibles.Count == 0)
            {
                //ttes les directions possibles ont été testée et elle mène toute à un échec
                return null;
            }
            //on choisit une nouvelle direction aléatoir parmis clle pas encore testée
            nbAlea = Random.Range(0, possibles.Count);
            choosenOne = possibles[nbAlea];
            //on applique la dir sur la case
            //BUG: old-tile est modifié
            maze[i][j].AddDirection(oldDir, choosenOne, color);
            newI = i;
            newJ = j;
            if (choosenOne == PCTile.PCFluidDirection.Down)
            {
                newI++;
            }
            else if (choosenOne == PCTile.PCFluidDirection.Left)
            {
                newJ--;
            }
            else
            {
                newJ++;
            }
            result = GeneratePath(choosenOne, newI, newJ, color);
        }

        return result;
    }

    /**
     * <summary>Génère un labyrinthe réaliseable</summary>
     */
    private void GenerateMaze()
    {
        //On tire 3 nombre aléatoire différents pour les 3 débuts
        List<int> starts = new List<int>() { Random.Range(0, mapSize) };
        for (int i = 0; i < 2; i++)
        {
            int nbAlea = Random.Range(0, mapSize);
            while (starts.Contains(nbAlea))
            {
                nbAlea = Random.Range(0, mapSize);
            }
            starts.Add(nbAlea);
        }

        //On demarre a chauqe fois d'un pont avec un direction vers le bas et on va chercher le chemin
        //On retroune la fin et on l'ajoute
        List<int> ends = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log((PCTile.PCFluidColor)i);
            int? toAdd = GeneratePath(PCTile.PCFluidDirection.Down, 0, starts[i], (PCTile.PCFluidColor)i);
            if (toAdd == null)
            {
                //Impossible de faire ce PC
                InitMaze();
                GenerateMaze();
                return;
            }
            ends.Add((int)toAdd);
        }

        //On a joute à startsAndEnds
        foreach (int start in starts)
        {
            startsAndEnds.Add((-1, start));
        }
        foreach (int end in ends)
        {
            startsAndEnds.Add((mapSize, end));
        }
    }
}
