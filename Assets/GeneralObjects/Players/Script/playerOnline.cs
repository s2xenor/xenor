using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//player class
public class playerOnline : MonoBehaviour
{
    //Attack of the player 
    public double Strength = 15;


    public Animator animator;
    //inventory associate to the player 
    inventoryOnline inventaire;


    public Image cooldown;
    public Image cooldown1;
    public Image cooldown2;
    public Image cooldown3;
    public Image cooldown4;


    //life barre of the player 
    life vie;

    PhotonView view;

    AudioSource audioSource;
    public AudioClip pickup;


    public bool boy;

    //damage potion prefab use by the player 
    public GameObject potionPrefab;


    void Start()
    {
        inventaire = transform.GetComponentInChildren<inventoryOnline>();//retrieve the prefab of the inventory 
        vie = transform.GetComponentInChildren<life>();// create life bar
    }


    void Awake()
    {
        vie= new life(cooldown, cooldown1, cooldown2, cooldown3, cooldown4);
        view = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.tag.Contains("slot") && view.IsMine)
        {
            audioSource.clip = pickup;  
            audioSource.Play();
        }

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
                colision.gameObject.GetComponent<MonstreOnline>().GetDamage((float)(Strength / (Strength - 1)));//make damage on monster 
                break;
        }
    }


    void Update()
    {
        if (GameObject.Find("Image").GetComponent<imageOnline>().PotionHealth)//Strength potion 
        {
            view.RPC("HealRPC", RpcTarget.All);
        }
        if (GameObject.Find("Image(1)").GetComponent<imageOnline>().PotionDamage)//damage potion
        {
            PhotonNetwork.Instantiate(potionPrefab.name, transform.position, Quaternion.identity);

            GameObject.Find("Image(1)").GetComponent<imageOnline>().PotionDamage = false;//remove the object of the inventory
        }
        if (GameObject.Find("Image(2)").GetComponent<imageOnline>().PotionStrength)// health potion 
        {
            view.RPC("StrengthRPC", RpcTarget.All);
        }
        if (!vie.die)
        {
            animator.SetFloat("Die", 0);//Remove the die animation 
            gameObject.GetComponent<playerwalkOnline>().enabled = true;
        }
        if (vie.die)
        {
            animator.SetFloat("Die", 1);//make the die animation 
            gameObject.GetComponent<playerwalkOnline>().enabled = false;
        }

    }


    /*
     *function that wait 30 and put back Strength to 15
     */
    IEnumerator waiter()
    {
        if (Strength > 15)
        {
            yield return new WaitForSeconds(30);
            Strength /= 1.2f;

            if (Strength < 15)
                Strength = 15;
        }
    }


    public void GetDamage()//make damage on the player 
    {
        vie.Reduce2(1); //Reduce 1/2 of the life bar
    }

    [PunRPC]
    void StrengthRPC()
    {
        Strength = Strength * 1.2;//increase the Strength
        GameObject.Find("Image(2)").GetComponent<imageOnline>().PotionStrength = false;//remove the object of the inventory 
        StartCoroutine(waiter());//make wait 30 before put back Strength to 15
    }

    [PunRPC]
    void HealRPC()
    {
        PotionOnline potion1 = new PotionOnline(PotionOnline.Type.Heal, 5);
        potion1.Effect(this, vie); //heal the palyer
        GameObject.Find("Image").GetComponent<imageOnline>().PotionHealth = false;
    }
}
