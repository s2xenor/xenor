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
    public bool coolingDown;
    public float waitTime = 15.0f;

    // Update is called once per frame
    void Update()
    {
        if (coolingDown == true)
        {
            //Reduce fill amount over 30 seconds
            cooldown.fillAmount -= 5.0f / waitTime * Time.deltaTime;
        }
    }
}
