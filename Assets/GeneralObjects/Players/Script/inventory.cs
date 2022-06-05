using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    bool activation=false;
    GameObject panel;
    public int[] slot;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;
        panel = transform.GetChild(0).gameObject;
        slot = new int[panel.transform.childCount];
    }

    // Update is called once per frame
    void Update()
    {
        
        
            if (Input.GetKeyDown(KeyCode.I))
            {
                activation = !activation;
                GetComponent<Canvas>().enabled = activation;
            }
        
    }

    public void UpdateNumber(int amount, string str)
    {
        panel.transform.GetChild(amount).GetChild(1).GetComponent<Text>().text=str;
    }
}
