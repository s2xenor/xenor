using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{

    public int nb = -1;
    public GameObject wire;
    public WiresManager wireManager;


    private bool isTrigger = false;

    // Update is called once per frame
    void Update()
    {

        if (isTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (nb != -1)//nb = -1 by default, 0 if it a left plug and 1 if it is the right plug
            {
                Destroy(wire);//remove wire unpluged
                if (nb == 1)//is right plug
                {
                    wireManager.UnPlug(true);
                }
                else
                {
                    wireManager.UnPlug(false);

                    //make player lose life
                    //reset all
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) => isTrigger = true;
    private void OnTriggerExit2D(Collider2D collision) => isTrigger = false;


}
