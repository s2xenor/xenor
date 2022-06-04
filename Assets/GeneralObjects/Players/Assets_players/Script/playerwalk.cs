using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerwalk : MonoBehaviour
{
    public string horizon;
    public string verti;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouvement = Vector3.ClampMagnitude(new Vector3(Input.GetAxis(horizon), Input.GetAxis(verti), 0),1);//creation du mouvement avec horizon=direction
        animator.SetFloat("Horizontal", mouvement.x);//mise en place de l'animation 
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Magnitude", mouvement.magnitude);
        transform.position = transform.position + mouvement * Time.deltaTime;//deplacement du joueur (changement de coordonn�es du joueur selon un temps proportionelle)
    }
}
