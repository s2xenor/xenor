using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerwalkOnline : MonoBehaviour
{
    public string horizon;
    public string verti;
    public Animator animator;

    PhotonView view;
    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        if (!view.IsMine) // Remove unecessary childs of other player locally
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }      
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            Vector3 mouvement = Vector3.ClampMagnitude(new Vector3(Input.GetAxis(horizon), Input.GetAxis(verti), 0),1);//creation du mouvement avec horizon=direction
            animator.SetFloat("Horizontal", mouvement.x);//mise en place de l'animation 
            animator.SetFloat("Vertical", mouvement.y);
            animator.SetFloat("Magnitude", mouvement.magnitude);
            transform.position = transform.position + mouvement * Time.deltaTime;//deplacement du joueur (changement de coordonn�es du joueur selon un temps proportionelle)
        }
    }
}
