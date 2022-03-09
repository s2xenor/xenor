using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instuctions : MonoBehaviour
{

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed) return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject MainCam = GameObject.FindWithTag("MainCamera");
            GameObject wiresCam = GameObject.FindWithTag("WiresCam");
            wiresCam.SetActive(true);
            MainCam.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject MainCam = GameObject.FindWithTag("MainCamera");
            GameObject wiresCam = GameObject.FindWithTag("WiresCam");
            MainCam.SetActive(true);
            wiresCam.SetActive(false);
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //Si ce qui est entrée en collision est un joueur
        if (other.tag == "local") pressed = true;
    }

   
    private void OnTriggerExit2D(Collider2D other)
    {
        //Si ce qui sort est un joueur
        if (other.tag == "local") pressed = false;
    }
}
