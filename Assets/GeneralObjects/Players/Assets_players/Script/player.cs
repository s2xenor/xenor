using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//player class
public class player : MonoBehaviour
{
    //Attack of the player 
    public double Strength = 15;


    public Animator animator;
    //inventory associate to the player 
    inventory inventaire;


    public Image cooldown;
    public Image cooldown1;
    public Image cooldown2;
    public Image cooldown3;
    public Image cooldown4;


    //life barre of the player 
    public life vie;
       

    void Start()
    {
        inventaire = GameObject.Find("Inventory").GetComponent<inventory>();
        vie= new life(cooldown, cooldown1, cooldown2, cooldown3, cooldown4);
    }

    void OnCollisionEnter2D(Collision2D colision)
    {

        switch (colision.gameObject.tag)
        {
            case "slot"://strength
                inventaire.slot[0] += 1;
                inventaire.UpdateNumber(0, inventaire.slot[0].ToString());
                Destroy(colision.gameObject);
                break;

            case "slot (1)"://damage
                inventaire.slot[1] += 1;
                inventaire.UpdateNumber(1, inventaire.slot[1].ToString());
                Destroy(colision.gameObject);
                break;

            case "slot (2)"://health
                inventaire.slot[2] += 1;
                inventaire.UpdateNumber(2, inventaire.slot[2].ToString());
                Destroy(colision.gameObject);
                break;
        }
    }


    void Update()
    {
        /*if (GameObject.Find("Image").GetComponent<image>().PotionStrength)
        {
            Strength = Strength * 1.2;
            GameObject.Find("Image").GetComponent<image>().PotionStrength = false;
        }
        if (GameObject.Find("Image(2)").GetComponent<image>().PotionHealth)
        {
            Potion potion1 = new Potion(Potion.Type.Heal, 5);
            potion1.Effect(this, null, vie); 
            GameObject.Find("Image(2)").GetComponent<image>().PotionHealth = false;
        }*/
        if(vie.die)
        {
            animator.SetFloat("die", 0);
        }
        if(!vie.die)
        {
            animator.SetFloat("Die", 0);
        }
        if(vie.die)
        {
            animator.SetFloat("Die", 1);
        }
        StartCoroutine(waiter());

    }

    IEnumerator waiter()
    {
        if (Strength > 15)
        {
            yield return new WaitForSeconds(30);
            Strength = 15;
        }
    }
}
