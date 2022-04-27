using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouvement : MonoBehaviour
{

    public Animator animator;//animation de marche
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 mouvement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Q))
        {

            animator.SetFloat("Magnitude", mouvement.magnitude);// mise en place animation selon sens 
            animator.SetFloat("Horizontal", mouvement.x);// mise en place animation selon sens
            transform.position = transform.position + mouvement * Time.deltaTime;//marcher horizontalement
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetFloat("Magnitude", mouvement.magnitude);// mise en place animation selon sens 
            animator.SetFloat("Vertical", mouvement.y);// mise en place animation selon sens
            transform.position = transform.position + mouvement * Time.deltaTime;//marcher verticalemnt 
        }



        
        

        transform.position = transform.position + mouvement * Time.deltaTime;//marcher selon la direction 
    }
}
