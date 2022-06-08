using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//player class
public class player : MonoBehaviour
{
    
    //Attack of the player 
    public double Strength = 15;
    public bool boy;

    //animation blend of the player 
    public Animator animator;

    //inventory associate to the player 
    inventory inventaire;

    //life barre of the player 
    public life vie;
    

    //damage potion prefab use by the player 
    public GameObject potionPrefab; 
    

    void Start()
    {
        inventaire = GameObject.Find("Inventory").GetComponent<inventory>();//retrieve the prefab of the inventory 
        if(boy)
            vie = GameObject.Find("life_hearts_boy").GetComponent<life>();// create life barre if boy 
        else
            vie= GameObject.Find("life_hearts_girl").GetComponent<life>();// create life barre if girl
    }

    /*
     * Function to retrieve object
     */

    void OnCollisionEnter2D(Collision2D colision)
    {

        switch (colision.gameObject.tag)//take the tag of the object
        {
            case "slot"://strength potion 
                inventaire.slot[0] += 1;
                inventaire.UpdateNumber(0, inventaire.slot[0].ToString());//add to the inventory 
                Destroy(colision.gameObject);// destroy the GameObject of the scene
                break;

            case "slot (1)"://damage potion 
                inventaire.slot[1] += 1;
                inventaire.UpdateNumber(1, inventaire.slot[1].ToString());
                Destroy(colision.gameObject);
                break;

            case "slot (2)"://health potion 
                inventaire.slot[2] += 1;
                inventaire.UpdateNumber(2, inventaire.slot[2].ToString());
                Destroy(colision.gameObject);
                break;

            case "Monster":
                colision.gameObject.GetComponent<Monstre>().GetDamage((float)(Strength/(Strength-1)));//make damage on monster 
                break;
        }
    }


    void Update()
    {
        if (GameObject.Find("Image").GetComponent<image>().PotionStrength)//Strength potion 
        {
            Strength = Strength * 1.2;//increase the Strength
            GameObject.Find("Image").GetComponent<image>().PotionStrength = false;//remove the object of the inventory 
        }
        if (GameObject.Find("Image(1)").GetComponent<image>().PotionDamage )//damage potion
        {
           
                Vector2 mouvement = new Vector2(Input.GetAxis(GetComponent<playerwalk>().horizon), Input.GetAxis(GetComponent<playerwalk>().verti));//create a mouvement for the potion
                mouvement.Normalize();
                GameObject potion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
                potion.GetComponent<Rigidbody2D>().velocity = mouvement * 3.0f;//throw the potion
                Destroy(potion, 2.0f);//destroy the object on the map 
     
                GameObject.Find("Image(1)").GetComponent<image>().PotionDamage = false;//remove the object of the inventory
        }
        if (GameObject.Find("Image(2)").GetComponent<image>().PotionHealth)// health potion 
        {
            Potion potion1 = new Potion(Potion.Type.Heal, 5); 
            potion1.Effect(this, vie); //heal the palyer
            GameObject.Find("Image(2)").GetComponent<image>().PotionHealth = false;
        }
        if (!vie.die)
        {
            animator.SetFloat("Die", 0);//Remove the die animation 
        }
        if(vie.die)
        {
            animator.SetFloat("Die", 1);//make the die animation 
        }
        StartCoroutine(waiter());//make wait 30 before put back Strength to 15
    }


    /*
     *function that wait 30 and put back Strength to 15
     */
    IEnumerator waiter()
    {
        if (Strength > 15)
        {
            yield return new WaitForSeconds(30);
            Strength = 15;
        }
    }

    
    public void GetDamage()//make damage on the player 
    {
        vie.Reduce2(1); //Reduce 1/2 of the life bar
    }
}
