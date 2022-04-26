using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class life : MonoBehaviour
{
    // Start is called before the first frame update
    /* void Start()
     {

     }*/
    public Image cooldown;
    public Image cooldown1;
    public Image cooldown2;
    public Image cooldown3;
    public Image cooldown4;
    public bool coolingDown;
    public float waitTime = 15.0f;

    // Update is called once per frame

    void Update()
    {
        
        
    }




    /*
     *Function that swith to another heart canvas 
     */
    public Image SwitchImage(int number)
    {
        switch(number)
        {
        case 1:
            return cooldown1;
        break;
        case 2:
            return cooldown2;
        break;
        case 3:
            return cooldown3;
        break;
        case 4:
            return cooldown4;
        break;
        default:
            return cooldown;
        }
    }

    /*
     *Function that reduce over the number of 1/2
     */
    void Reduce2(int hearts) //Function that reduce by 1/2 hearts
    {
        if (coolingDown)//Reduce fill amount over the numbers of 1/2 hearts
        {
            float val = 1;
            int j = 1;
            for (int i = 0; i<hearts; i++)
            {
                val-= 0.5f;
                if (val==0)
                {
                    cooldown.fillAmount = 0;
                    cooldown=SwitchImage(j);
                    j++;
                    val = 1;
                }
                else
                {
                    cooldown.fillAmount = val;
                }
            }
        }
    }
    /*
     *Function that reduce over the number of 1/4
     */

    void Reduce4(int hearts) //Function that reduce by 1/4 hearts
    {
        if (coolingDown == true)
        {
            //Reduce fill amount over the numbers of 1/4 hearts
            float val = 1;
            int j = 1;
            for (int i = 0; i < hearts; i++)
            {
                val -= 0.25f;
                if (val == 0)
                {
                    cooldown.fillAmount = 0;
                    cooldown = SwitchImage(j);
                    j++;
                    val = 1;
                }
                else
                {
                    cooldown.fillAmount = val;
                }
            }
        }
    }

}
