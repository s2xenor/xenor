using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerwalkLoading : MonoBehaviour
{
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetFloat("Horizontal", 1);//mise en place de l'animation 
        animator.SetFloat("Magnitude", 1); 
    }
}
