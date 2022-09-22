using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float HP, MaxHP;
    public string türüm;
    public Player PlayerScript;
    public GameObject Wood;

    void Start()
    {
        PlayerScript = GameObject.Find("Player").transform.GetComponent<Player>();

        if (türüm == "tree20")
        {
            HP = MaxHP = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (HP <= 0)
        {
            int myGörevNum = 0;
            if (türüm == "tree20")
            {
                myGörevNum = 101;
            }
            PlayerScript.checkQuests(myGörevNum);
            PlayerScript.gainExp(1);
//EŞYA DÜŞÜRME KISMI
            try
            {
                if (türüm == "tree20")
                {
                    Debug.Log("eşya düşürme Resource.cs");
                    var newObj = Instantiate(Wood, this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                    newObj.GetComponent<item>().amount = 5;
                }
            }
            catch
            {

            }
            

            Destroy(this.gameObject);
        }
    }
    public void receiveDamage(float dmg)
    {
        HP -= dmg;
        //Debug.Log("Enemy HP: "+HP);
    }
}
