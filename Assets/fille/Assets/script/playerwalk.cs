using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerwalk : MonoBehaviour
{
    public string horizon;
    public string verti;
    public Animator animator;//animation de marche 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouvement = new Vector3(Input.GetAxis(horizon), Input.GetAxis(verti), 0.0f);

       
            animator.SetFloat("Horizontal", mouvement.x);// mise en place animation selon sens
            animator.SetFloat("Vertical", mouvement.y);// mise en place animation selon sens
        animator.SetFloat("Magnitude", mouvement.magnitude);// mise en place animation selon sens 

        transform.position = transform.position + mouvement * Time.deltaTime;//marcher selon la direction 
    }
}
