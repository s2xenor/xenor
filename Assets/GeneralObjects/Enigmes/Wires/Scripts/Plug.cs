using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{

    public int nb = -1;
    public GameObject wire;
    public WiresManager wireManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("keydown");
            if (nb != -1)//nb = -1 by default, 0 if it a left plug and 1 if it is the right plug
            {
                if (nb == 1)//is right plug
                {
                    Debug.Log("you are a success");
                    wireManager.UnPlug(true);
                }
                else
                {
                    Debug.Log("you smell bad");
                    wireManager.UnPlug(false);

                    //make player lose life
                    //reset all
                }
                Destroy(wire);//remove wire unpluged
            }
        }
    }


   
}
