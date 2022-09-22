using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public GameObject item,acıklamaText;
    public int ID;
    public string type;
    public float number;
    public string description;
    public int amount;
    public int currentAmount;//sanırım görev bitirme için
    public Sprite icon,iconless;
    public int slotNum;
    public GameObject acıklamaPanel;

    public bool empty;


    private void Start()
    {
        acıklamaPanel.SetActive(false);
        if (empty==true)
        {
            this.GetComponent<Image>().sprite = iconless;
        }
    }
    public void SettingSlots()
    {
        if (empty == true)
        {
            this.GetComponent<Image>().sprite = iconless;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        acıklamaPanel.SetActive(true);
        try
        {
            paneldeYaz();
            acıklamaPanel.transform.position = this.gameObject.transform.position;
        }
        catch
        {

        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        acıklamaPanel.SetActive(false);
    }

    public void UpdateSlot()
    {
        this.GetComponent<Image>().sprite = icon;
    }
    public void EmptySlot()
    {
        this.GetComponent<Image>().sprite = iconless;
        try
        {
            GameObject.Destroy(transform.GetChild(0).gameObject);
        }
        catch
        {
            Debug.Log("silemeye çalıştığın obje boş");
        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Debug.Log("slot: " + this.gameObject.name + "im     "+ item.ToString());
        if (ID != 0)
        {
            if (type == "weapon"|| type == "tool")
            {
                item.GetComponent<item>().UseIt();
            }

            if (type == "potion")
            {
                item.GetComponent<item>().ConsumeIt();

                Debug.Log("Eşyayı tüketiyosun Slot.cs");
                amount -= 1;
                Destroy(GetComponent<Transform>().GetChild(0).gameObject);
                if (amount == 0)
                {
                    item = null;
                    ID = 0;
                    type = null;
                    number = 0;
                    description = null;
                    amount = 0;
                    currentAmount = 0;
                    icon = null;
                    empty = true;

                    this.GetComponent<Image>().sprite = iconless;
                    try
                    {
                        GameObject.Destroy(transform.GetChild(0).gameObject);
                    }
                    catch
                    {
                        Debug.Log("silemeye çalıştığın obje boş");
                    }
                }
            }

            paneldeYaz();
        }
    }

    public void NoMoreObj()
    {
        Debug.Log("Eşyayı yok ediyosun Slot.cs");

        if (type == "görev")
        {
            if (number == 1)
            {
                GameObject.Find("Player").GetComponent<Player>().MaxHP += 2;
            }


            if (number == 101)
            {
                GameObject.Find("Player").GetComponent<Player>().para += 5;
            }
            GameObject.Find("Player").GetComponent<Player>().updateTextimiz();
        }

        item = null;
        ID = 0;
        type = null;
        number = 0;
        description = null;
        amount = 0;
        currentAmount = 0;
        icon = null;
        empty = true;

        this.GetComponent<Image>().sprite = iconless;
        try
        {
            GameObject.Destroy(transform.GetChild(0).gameObject);
        }
        catch
        {
            Debug.Log("silemeye çalıştığın obje boş");
        }
    }

    private void paneldeYaz()
    {
        //PANELDE YAZAN ŞEY İÇİN
        if (ID == 0)
        {
            acıklamaText.gameObject.GetComponent<Text>().text = "Sen yeni eşya veya görev alana kadar boş.";
        }
        else
        {
            acıklamaText.gameObject.GetComponent<Text>().text = description.ToString();

            if (type == "potion")
            {
                acıklamaText.gameObject.GetComponent<Text>().text = description.ToString() + "\n" + "Sende " + amount.ToString() + " tane var.";
            }
            if (type == "görev")
            {
                acıklamaText.gameObject.GetComponent<Text>().text = description.ToString() + "\n" + "Kalan " + (amount-currentAmount).ToString();
            }

        }
    }
    
}
