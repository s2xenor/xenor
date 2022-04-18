using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> prefabs;

    private const int startX = 0;
    private const int startY = 0;

    private const int x = 15;
    private const int y = 15;

    private int[,] laby = new int[x, y];

    void Start()
    {
        //for (int x = 0; x < laby.GetLength(0); x++)
        //{
        //    for (int y = 0; y < laby.GetLength(1); y++)
        //    {
        //        if (laby[x, y] == null) laby[x, y] = 0;

        //    }
        //}
        (int Xt, int Yt) = (0, Random.Range(0, y - 1));
        laby[Xt, Yt] = 2010;//down but not up
        Xt += 1;
        int previous = (int)Direction.down;
        while(Xt < x)
        {
            int r = Random.Range(0, 3);
            if (r == 0) // go down
            {
                laby[Xt, Yt] = direction("down")+addPrevious(previous);
                Xt += 1;
                previous = (int)Direction.down;
            } else if (r == 1 && Yt + 1 < y && previous != direction("left")) //go right
            {
                laby[Xt, Yt] = (int)Direction.right+addPrevious(previous);
                Yt += 1;
                previous = (int) Direction.right;
            } else if (r == 2 && Yt - 1 >= 0 && previous != direction("right"))//go left
            {
                laby[Xt, Yt] = (int)Direction.left + addPrevious(previous);
                Yt -= 1;
                previous = (int) Direction.left;
            }
        }




        for (int x = 0; x < laby.GetLength(0); x++)
        {
            string tmp = "";
            for (int y = 0; y < laby.GetLength(1); y++)
            {
                tmp += " " + laby[x, y];
            }
            Debug.Log(tmp);
        }
        createLaby();
        //prefabRules(2010);


    }

    private int[] toByteArray(int val)
    {
        List<int> t = new List<int>();
        if (val < 8) t.Add(0);
        if (val < 4) t.Add(0);
        if (val < 2) t.Add(0);
        while(val > 0)
        {
            t.Add(val % 2);
            val /= 2;
        }
        return t.ToArray();
    }

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

    private int direction(string dir)
    {

        switch (dir)
        {
            case "none":
                return 0000;
            case "left":
                return 0001;
            case "down":
                return 0010;
            case "down-left":
                return 0011;
            case "right":
                return 0100;
            case "right-left":
                return 0101;
            case "right-down":
                return 0110;
            case "right-down-left":
                return 0111;
            case "up":
                return 1000;
            case "up-left":
                return 1001;
            case "up-down":
                return 1010;
            case "up-down-left":
                return 1011;
            case "up-right":
                return 1100;
            case "up-right-left":
                return 1101;
            case "up-right-down":
                return 1110;
            case "up-right-down-left":
                return 1111;
            default:
                return 0;
        }

    }

    private GameObject prefabRules(int rule)
    {
        List<int> t = new List<int>();
        int tempRule;
        int[] bitArray;
        bool goodNumber;
        if (rule == 0)
        {
            //return new GameObject();
            return prefabs[Random.Range(1, 15)];
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
            for (int i = 0; i < t.Count; i++)
            {
                //Debug.Log(t[i]);
            }
            if(t.Count > 0) return prefabs[t[Random.Range(0, t.Count)]];
            return prefabs[Random.Range(1, 15)];

            return new GameObject();
        }
    }

    private void createLaby()
    {
        var parentObj = new GameObject("ArrowsParent");

        for (int x = 0; x < laby.GetLength(0); x++)
        {
            for (int y = 0; y < laby.GetLength(1); y++)
            {
                Instantiate(prefabRules(laby[x,y]), new Vector3(y*0.32f, -x*0.32f), Quaternion.identity, parentObj.transform);   
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
