using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class inventoryOnline : MonoBehaviour
{
    bool activation = false;
    public GameObject panel;
    public int[] slot;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;
        slot = new int[panel.transform.childCount];
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && Input.GetKeyDown(KeyCode.I))
        {
            activation = !activation;
            GetComponent<Canvas>().enabled = activation;
        }
    }

    public void UpdateNumber(int amount, string str)
    {
        panel.transform.GetChild(amount).GetChild(1).GetComponent<Text>().text = str;
    }
}
