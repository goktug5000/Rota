using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class item : MonoBehaviour
{

    private GameObject player;
    public int ID;
    public string type;//weapon,potion,slotluk,görev
    public float number;
    /*görevse
        1-beyaz
        101-tree20
    */
    public string description;
    public int amount;//pot falansa adet, görevse kaçtane yapacağın
    public Sprite icon;
    public bool pickedUp;
    public  GameObject MyName;

    public Blade bladeCS;

    private void Start()
    {
        player = GameObject.Find("Player");
        bladeCS = GameObject.Find("L-Omuz").GetComponent<Blade>();
        MyName = this.transform.GetChild(0).gameObject;
        if (type == "para"|| type == "odun")
        {
            MyName.GetComponent<TextMeshPro>().text = type + " (" + amount + ")";
        }

        MyName.SetActive(false);
    }
    public void ConsumeIt()
    {
        if (type == "potion")
        {
            player.GetComponent<Player>().HP = player.GetComponent<Player>().HP + number;
            //player.GetComponent<Player>().RemoveItemAtSlot();
        }
    }
    private void OnMouseOver()
    {
        MyName.SetActive(true);
    }
    private void OnMouseExit()
    {
        MyName.SetActive(false);
    }
    public void UseIt()
    {
        if (type == "weapon")
        {
            if (ID >= 101 && ID <= 200)
            {
                Debug.Log("kullanılanın silah olduğu item.cs");
                bladeCS.giyilenEsya = "kilic";
                player.GetComponent<Player>().weaponDamage = number;
                player.GetComponent<Player>().myDamage(number);
            }
        }
        if (type == "tool")
        {
            if (ID >= 201 && ID <= 210)
            {
                Debug.Log("kullanılanın silah değil balta olduğu item.cs");
                bladeCS.giyilenEsya = "balta";
                player.GetComponent<Player>().weaponDamage = number;
                player.GetComponent<Player>().myDamage(number);
            }
            
        }
        if (type == "slotluk")
        {
            //player.GetComponent<Player>().RemoveItemAtSlot();
        }
    }

    public int getID()
    {
        return ID;
    }

}