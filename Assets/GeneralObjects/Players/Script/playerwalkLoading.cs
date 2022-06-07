using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerwalkLoading : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        animator.SetFloat("Horizontal", 1);//mise en place de l'animation 
        animator.SetFloat("Magnitude", 1);//mise en place de l'animation 
    }
}
