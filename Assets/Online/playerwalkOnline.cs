using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerwalkOnline : MonoBehaviour
{
    public string horizon;
    public string verti;
    public Animator animator;
    Vector3 movement;

    PhotonView view;
    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        if (!view.IsMine) // Remove unecessary childs of other player locally
        {
            for (int i = 2; i < transform.childCount - 1; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }      
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {
            movement = Vector3.ClampMagnitude(new Vector3(Input.GetAxis(horizon), Input.GetAxis(verti)), 1);//creation du mouvement avec horizon=direction
            animator.SetFloat("Horizontal", movement.x);//mise en place de l'animation 
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            transform.position = transform.position + movement * Time.deltaTime;//deplacement du joueur (changement de coordonn�es du joueur selon un temps proportionelle)
        }
    }
}
