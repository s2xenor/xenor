using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArrowsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> prefabsL; //prefabs laby
    public List<GameObject> prefabsR; //prefabs room


    private const int startX = 0;
    private const int startY = 0;

    private const int x = 12;
    private const int y = 12;

    private int[,] laby = new int[x, y];

    public Transform[] players = new Transform[2]; // Position des 2 joueurs
    public float tileSize = 0.32f; // Taille des tiles du labyrinthe

    int[] player1Tile = new[] { 0, 0 };
    int[] player2Tile = new[] { 0, 0 };

    int[] player1TmpTile = new[] { 0, 0 };
    int[] player2TmpTile = new[] { 0, 0 };

    void Start()
    {
        //get player
        GameObject[] playersTmp = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < playersTmp.Length; i++)
        {
            players[i] = playersTmp[i].GetComponent<Transform>();
        } 

        (int Xt, int Yt) = (0, Random.Range(0, y - 1));
        laby[Xt, Yt] = 2010;//down but not up
        Xt += 1;
        int previous = (int)Direction.down;
        while(Xt < x)
        {
            int r = Random.Range(0, 3);
            if (r == 0) // go down
            {
                laby[Xt, Yt] = (int)Direction.down+addPrevious(previous);
                Xt += 1;
                previous = (int)Direction.down;
            } 
            else if (r == 1 && Yt + 1 < y && previous != (int)Direction.left) //go right
            {
                laby[Xt, Yt] = (int)Direction.right+addPrevious(previous);
                Yt += 1;
                previous = (int) Direction.right;
            } 
            else if (r == 2 && Yt - 1 >= 0 && previous != (int)Direction.right)//go left
            {
                laby[Xt, Yt] = (int)Direction.left + addPrevious(previous);
                Yt -= 1;
                previous = (int) Direction.left;
            }
        }



        //print laby
        //for (int x = 0; x < laby.GetLength(0); x++)
        //{
        //    string tmp = "";
        //    for (int y = 0; y < laby.GetLength(1); y++)
        //    {
        //        tmp += " " + laby[x, y];
        //    }
        //    Debug.Log(tmp);
        //}


        //print binary
        //for (int i = 0; i < 16; i++)
        //{
        //    int[] t = toByteArray(i);
        //    string f = "";
        //    for (int j = 0; j < t.Length; j++)
        //    {
        //        f += t[j].ToString();
        //    }
        //    Debug.Log(f);

        //}

        //for (int i = 0; i < 16; i++)
        //{

        //    Debug.Log(toBin(i));

        //}

        //Debug.Log(toByteArray(1));
        //prefabRules(2010);
        createLaby();

        //printBitArray(DeciToArray(50, 4));

    }

    //convert to byteArray on 4 bits 
    private int[] toByteArray(int val)
    {
        if (val > 15) return new int[] { 1, 1, 1, 1 };
        int[] byteArr = new[] { 0, 0, 0, 0 };
        
        for(int i = 0; i < 4; i++)
        {
            byteArr[i] = (val % 2);
            val /= 2;
        }

        //reverse array
        (byteArr[0], byteArr[3]) = (byteArr[3], byteArr[0]);
        (byteArr[1], byteArr[2]) = (byteArr[2], byteArr[1]);

        return byteArr;
    }

    private void printBitArray(int[] bitArr)
    {
        string f = "";
        for (int j = 0; j < bitArr.Length; j++)
        {
            f += bitArr[j].ToString();
        }
        Debug.Log(f);
    }

    //deci to binary but in decimal representation
    private int toBin(int val)
    {
        int[] binArr = toByteArray(val);
        int bin = 0;
        int power = 1;
        for(int i = binArr.Length-1; i >= 0; i--)
        {
            bin += power * binArr[i];
            power *= 10;
        }
        return bin;
    }

 
    //21 => [2,1]
    private int[] DeciToArray(int num, int b = 0)
    {
        List<int> result = new List<int>();
        int len = num.ToString().Length;
        
        while (num != 0)
        {
            result.Insert(0, num % 10);
            num = num / 10;
        }
        for (int i = len; i < b; i++)
        {
            result.Insert(0, 0);
        }

        return result.ToArray();
    }

    //add previous direction to not be opposite
    private int addPrevious(int previous)
    {
        if(previous == -1) return 0;
        if (previous == (int)Direction.right) return 2 * (int)Direction.left;//si avant aller droite mtn peux pas aller gauche
        if (previous == (int)Direction.left) return 2 * (int)Direction.right;
        return 2 * (int)Direction.up; 
    }

    enum Direction
    {
        none = 0000,
        left = 0001,
        down = 0010,
        down_left = 0011,
        right = 0100,
        right_left = 0101,
        right_down = 0110,
        right_down_left = 0111,
        up = 1000,
        up_left = 1001,
        up_down = 1010,
        up_down_left = 1011,
        up_right = 1100,
        up_right_left = 1101,
        up_right_down = 1110,
        up_right_down_left = 1111
    }

    private int prefabRules(int rule)
    {
        List<int> t = new List<int>();
        int tempRule;
        int[] bitArray;
        bool goodNumber;
        if (rule == 0)
        {
            //return new GameObject();
            return Random.Range(1, 15);
        }
        else
        {
            //2010
            for(int i = 1; i < 15; i++)
            {
                tempRule = rule;
                goodNumber = true;
                for (int j =0; j < 4; j++)
                {
                    bitArray = toByteArray(i);
                    if (tempRule%10 == 2 && 1 == bitArray[3-j] )
                    {
                        goodNumber = false;
                    }
                    if(tempRule%10 == 1 && 0 == bitArray[3-j])
                    {
                        goodNumber = false;
                    }
                    tempRule = tempRule / 10;
                }
                if (goodNumber) t.Add(i);
            }


            //for (int i = 0; i < t.Count; i++)
            //{
            //    //Debug.Log(t[i]);
            //}

            //for(int i = 0; i < t.Count; i++)
            //{
            //    Instantiate(prefabRules(t[i]), new Vector3(i * 0.32f, 0), Quaternion.identity);
            //}

            return t[Random.Range(0, t.Count)];
            //if (t.Count > 0) 
            //return prefabsL[Random.Range(1, 15)];

            //return new GameObject();
        }
    }

    private void createLaby()
    {
        var parentObj = new GameObject("ArrowsParent");
        GameObject tmp;
        //floor top and bottom
        for(int j = 0; j < 3; j++)
        {
            for(int i = 0; i < x; i++)
            {
                Instantiate(prefabsR[0], new Vector3(startX + i * 0.32f + 0.1f, startY + (3-j) * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);//floor top
                Instantiate(prefabsR[0], new Vector3(startX + i * 0.32f + 0.1f, startY - (2 - j + y) * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);//floor bottom
            }
        }
        //wall left and right
        for(int i = 0; i < x+6; i++)
        {
            tmp = Instantiate(prefabsR[7], new Vector3(startX - 1 * 0.32f + 0.1f, startY - (i-3) * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);//left wall
            tmp.AddComponent<BoxCollider2D>();
            tmp = Instantiate(prefabsR[3], new Vector3(startX + x * 0.32f + 0.1f, startY - (i-3) * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);//right wall
            tmp.AddComponent<BoxCollider2D>();

        }

        int[,] spawnLaby = new int[x, y];
        int r = Random.Range(0, 3);

        for (int x = 0; x < laby.GetLength(0); x++)
        {
            for (int y = 0; y < laby.GetLength(1); y++)
            {
                r = Random.Range(0, 8);
                if(r == 1 || r == 0) //setup both
                {
                    spawnLaby[x, y] = 2;
                }
                else if(r%2 == 0) //setup local
                {
                    spawnLaby[x, y] = 0;
                }
                else //setup distant
                {
                    spawnLaby[x, y] = 1;
                }
            }
        }


       int prefabNb;
        for (int x = 0; x < laby.GetLength(0); x++)
        {
            for (int y = 0; y < laby.GetLength(1); y++)
            {
                prefabNb = prefabRules(laby[x, y]);
                laby[x, y] = toBin(prefabNb);
                if(spawnLaby[x, y] == 0) //setup local
                {
                    Instantiate(prefabsL[prefabNb], new Vector3(startX + y * 0.32f+0.1f, startY - x * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);
                    PhotonNetwork.Instantiate(prefabsL[0].name, new Vector3(startX + y * 0.32f + 0.1f, startY - x * 0.32f - 0.32f), Quaternion.identity);
                }
                else if(spawnLaby[x, y] == 1) //setup distant
                {
                    Instantiate(prefabsL[0], new Vector3(startX + y * 0.32f + 0.1f, startY - x * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);
                    PhotonNetwork.Instantiate(prefabsL[prefabNb].name, new Vector3(startX + y * 0.32f + 0.1f, startY - x * 0.32f - 0.32f), Quaternion.identity);
                }
                else //both (== 2)
                {
                    Instantiate(prefabsL[prefabNb], new Vector3(startX + y * 0.32f + 0.1f, startY - x * 0.32f - 0.32f), Quaternion.identity, parentObj.transform);
                    PhotonNetwork.Instantiate(prefabsL[prefabNb].name, new Vector3(startX + y * 0.32f + 0.1f, startY - x * 0.32f - 0.32f), Quaternion.identity);
                }
            }
        }
    }

    private bool RespectRules(int tile, Direction dir)
    {
        int[] tileBitArray = DeciToArray(tile,4);
        int[] dirBitArray = DeciToArray((int) dir, 4);
        for (int i = 0; i < tileBitArray.Length; i++)
        {
            if (tileBitArray[i] < dirBitArray[i]) return false;
        }
        return true;
    }

    private bool CheckPlayerMovement()
    {
        if (player1Tile[1] < 0 || player1Tile[0] < 0 || player1Tile[1] >= y || player1Tile[0] >= x) return true;
        if (player1TmpTile[1] < 0 || player1TmpTile[0] < 0 || player1TmpTile[1] >= y || player1TmpTile[0] >= x) return true;


        int tile = laby[player1Tile[1], player1Tile[0]];
        int newTile = laby[player1TmpTile[1], player1TmpTile[0]];

        if (player1Tile[0] != player1TmpTile[0]) //change x
        {
            if (player1Tile[0] > player1TmpTile[0]) //go left
            {
                if (!RespectRules(tile, Direction.left)) return false; //previous tile has not left arrow
                if (RespectRules(newTile, Direction.right)) return false; //current tile has a right arrow
            }
            else //go right
            {
                if (!RespectRules(tile, Direction.right)) return false;
                if (RespectRules(newTile, Direction.left)) return false;
            }
        }
        else if(player1Tile[1] != player1TmpTile[1]) //change y
        {
            if (player1Tile[1] > player1TmpTile[1]) //go up
            {
                if (!RespectRules(tile, Direction.up)) return false;
                if (RespectRules(newTile, Direction.down)) return false;
            }
            else //go down
            {
                if (!RespectRules(tile, Direction.down)) return false;
                if (RespectRules(newTile, Direction.up)) return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {

        player1TmpTile[0] = (int)((players[0].position.x - startX) / tileSize);
        player1TmpTile[1] = (int)((players[0].position.y - startY) / tileSize) * -1;
        if (player1Tile[0] != player1TmpTile[0] || player1Tile[1] != player1TmpTile[1])
        {

            if (!CheckPlayerMovement())//player break rules, apply consequences
            {
                Debug.Log("consequences");
            }
            player1Tile[0] = (int)((players[0].position.x - startX) / tileSize); // Calculate position of player in maze
            player1Tile[1] = (int)((players[0].position.y - startY) / tileSize)*-1;
            //player2Tile[0] = (int)((players[1].position.x - startX) / tileSize);
            //player2Tile[1] = (int)((players[1].position.y - startY) / tileSize);

            Debug.Log($"{player1Tile[0]}, {player1Tile[1]}");
        }

    }
}
