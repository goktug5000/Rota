using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Blade bladeScript;
    public ForCanvas forCanvasScript;
    public Camera cam;
    public Enemy EnemyScript;
    
    public Text HPmiz,EXPmiz,combatPointimiz,StrLvlimiz,AttakSpeedLvlimiz;
    public Text Paramiz, Woodumuz, Stoneimiz, Skinimiz;
    public GameObject died,L_omuz,rocketFire;
    public NavMeshAgent agent;
    public Rigidbody playerRB;
    public Animator anim;

    private Vector3 anan;//YÜRÜME İŞLEMİNDE KULLANDIĞIM ŞEY
    private float velocity;

    private float RegenHP, expCurrent, expForNext, levelCombat, skill_CombatPoint, my_attackSpeed=0.5f, my_Str=10, MaxOil,oil;
    public float MaxHP, HP,weaponDamage = 10;

    public int para,wood,stone,skin;//envanterde olmayıp sınırsız biriktirilenler
    private int allSlots, enabledSlots;
    private GameObject[] slot;
    public GameObject slotHolder;
    private GameObject itemPickedUp;


    private void Start()
    {
        MaxHP = 60;
        HP = 60;
        MaxOil = 100;
        oil = 1000;
        expCurrent = 0;
        expForNext = 10;
        levelCombat = 0;
        skill_CombatPoint = 5;
        para=wood=stone=skin = 0;
        playerRB = this.gameObject.GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponent<Animator>();

        bladeScript = L_omuz.transform.GetComponent<Blade>();
        myDamage(weaponDamage);

        EXPmiz.text = "Exp: " + expCurrent.ToString() + " /" + expForNext.ToString();
        combatPointimiz.text = skill_CombatPoint.ToString();
        StrLvlimiz.text = "Str: "+my_Str.ToString();
        AttakSpeedLvlimiz.text = my_attackSpeed.ToString();

        Paramiz.text = para.ToString();
        Woodumuz.text = wood.ToString();
        Stoneimiz.text = stone.ToString();
        Skinimiz.text = skin.ToString();

        rocketFire.SetActive(false);

        allSlots = 8;
        SetSlots();

        itemPickedUp = GameObject.Find("Sword00");
        item Item = itemPickedUp.GetComponent<item>();
        AddItem(itemPickedUp, Item.ID, Item.type, Item.number, Item.description, Item.amount, Item.icon);
        slot[0].GetComponent<Slot>().UpdateSlot();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            allSlots = 32;
            SetSlots();
        }

//YÜRÜMEK İÇİN
        RaycastHit hit;
        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(ray.ToString());
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        

//HP İLE İLGİLİ
        HPmiz.text = ("HP:  " + (HP - HP % 1).ToString() + " /" + MaxHP.ToString());
        if (HP < MaxHP)
        {
            HP+=(0.5f)*Time.deltaTime;
        }
        if (HP > MaxHP)
        {
            HP = MaxHP;
            Debug.Log("HP = Max HP");
        }
        if (HP < 0)
        {
            this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
            died.SetActive(true);
        }

//HITBOX AÇICI
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            this.gameObject.GetComponent<Collider>().enabled=true;
        }
        else
        {
            this.gameObject.GetComponent<Collider>().enabled = false;
        }


//HIZ HESAPLAYICI

        velocity = ((transform.position - anan).magnitude) / Time.deltaTime;
        anan = transform.position;

//ROKETLERİ ATEŞLE
        if (Input.GetKey(KeyCode.LeftShift) && oil > 0 && velocity > 2)
        {
            agent.speed = 18;
            oil -= 1 * Time.deltaTime;
            rocketFire.SetActive(true);
        }
        if ((!Input.GetKey(KeyCode.LeftShift) || oil < 0 || velocity < 2)&& bladeScript.hizlanabilir==true)
        {
            agent.speed = 6;
            rocketFire.SetActive(false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            agent.speed = 0;
            agent.SetDestination(this.gameObject.transform.position);
            rocketFire.SetActive(false);
        }


//YÜRÜME ANİMASYONU

        if (velocity > 2)
        {
            anim.SetBool("walkB", true);
        }
        else
        {
            anim.SetBool("walkB", false);
        }
    }
//Muharebe Becerileri
    public void GelistirMyDamage()
    {
        if (skill_CombatPoint > 0)
        {
            my_Str += 1f;
            skill_CombatPoint--;

            combatPointimiz.text = skill_CombatPoint.ToString();
            StrLvlimiz.text = "Str: " + my_Str.ToString();
            myDamage(weaponDamage);
        }   
    }

    /* SALDIRI HIZI GELİŞTİRME KAPALI
     
    public void GelistirMyAtkSpeed()
    {
        if (skill_CombatPoint > 0)
        {
            if (my_attackSpeed < 2)
            {
                my_attackSpeed += 0.2f;
                skill_CombatPoint--;
                AttakSpeedLvlimiz.text = my_attackSpeed.ToString();
                myDamage();
            } 
        }    
    }*/

//Calculate DAMAGE
    public void myDamage(float weaponDamagee)
    {
        float safHasar;
        safHasar = weaponDamagee * (my_Str/ 10);
        Debug.Log(safHasar);
        bladeScript.Hasarim(safHasar, my_attackSpeed);
    }
//RECEIVE DAMAGE
    public void receiveDamagePlayer(float dmg)
    {
        HP -= dmg;
        forCanvasScript.intoToWar(7);
        //Debug.Log("HP: " + HP);
    }
    public void gainExp(float gainedExp)
    {
        expCurrent += gainedExp;
        if (expCurrent >= expForNext)
        {
            expCurrent = expCurrent - expForNext;

            levelCombat++;
            skill_CombatPoint++;
            combatPointimiz.text = skill_CombatPoint.ToString();

            expForNext = levelCombat * 10;
        }
        EXPmiz.text = "Exp: " + expCurrent.ToString() + " /" + expForNext.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemPickedUp = other.gameObject;
            item Item = itemPickedUp.GetComponent<item>();
            if (Item.type == "para")
            {
                para+= itemPickedUp.GetComponent<item>().amount;
                Paramiz.text = para.ToString();
                Destroy(itemPickedUp);
            }
            else if (Item.type == "odun")
            {
                wood += itemPickedUp.GetComponent<item>().amount;
                Woodumuz.text = wood.ToString();
                Destroy(itemPickedUp);
            }
            else if (Item.type == "tas")
            {
                stone += itemPickedUp.GetComponent<item>().amount;
                Stoneimiz.text = stone.ToString();
                Destroy(itemPickedUp);
            }
            else if (Item.type == "deri")
            {
                skin += itemPickedUp.GetComponent<item>().amount;
                Skinimiz.text = skin.ToString();
                Destroy(itemPickedUp);
            }
            else if (Item.type == "slotluk"|| Item.type == "weapon"|| Item.type == "tool" || Item.type == "potion"||Item.type=="görev")
            {
                AddItem(itemPickedUp, Item.ID, Item.type, Item.number, Item.description, Item.amount, Item.icon);
            }

        }
        /*
        else if (other.tag == "NPC")
        {
            NPC npc = other.gameObject.GetComponent<NPC>();
            npc.ShowTheQuest();
        }
        */
        else {
            Debug.Log("tanımlanamayana girdin");
        }
    }
    public void updateTextimiz()
    {
        Paramiz.text = para.ToString();
        Woodumuz.text = wood.ToString();
        Stoneimiz.text = stone.ToString();
        Skinimiz.text = skin.ToString();
    }


    public void distanceBetweenEnemy(GameObject ClosestEnemy)
    {
        Vector3 enemyDir = ClosestEnemy.transform.position - transform.position;
        float angleOfEnemy = Vector3.Angle(enemyDir, this.gameObject.transform.forward);
        Debug.Log(angleOfEnemy);
    }

    public void SetSlots()
    {
        slot = new GameObject[allSlots];
        for (int q = 0; q < allSlots; q++)
        {
            slot[q] = slotHolder.transform.GetChild(q).gameObject;
            if (slot[q].GetComponent<Slot>().item == null)
            {
                slot[q].GetComponent<Slot>().empty = true;
                slot[q].GetComponent<Slot>().SettingSlots();
            }
        }
    }

    
    public void checkQuests(int görevNum)
    {

        Debug.Log("checkQuests() player.cs");

        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().type == "görev")
            {
                if(slot[i].GetComponent<Slot>().number == görevNum)
                {
                    Debug.Log("görev yapıyorum");
                    Debug.Log(i);
                    slot[i].GetComponent<Slot>().currentAmount += 1;
                    if (slot[i].GetComponent<Slot>().currentAmount >= slot[i].GetComponent<Slot>().amount)
                    {
                        slot[i].GetComponent<Slot>().NoMoreObj();
                    }
                                    return;
                }
                else
                {
                    Debug.Log("böyle görevin yok Player.cs");
                    
                }

            }

        }
    }
    

    void AddItem(GameObject itemObject, int itemID, string itemType,float itemNumber, string itemDescripton, int itemAmount, Sprite itemIcon)
    {
        bool potKontrol = false;
        //Debug.Log("add itemdesin");
        if (itemType == "potion")
        {
            
            for (int i = 0; i < allSlots; i++)
            {
                if(slot[i].GetComponent<Slot>().type == "potion")
                {
                    Debug.Log("Başka potun var");
                    potKontrol = true;


                    itemObject.GetComponent<item>().pickedUp = true;

                    slot[i].GetComponent<Slot>().item = itemObject;
                    slot[i].GetComponent<Slot>().ID = itemID;
                    slot[i].GetComponent<Slot>().type = itemType;
                    slot[i].GetComponent<Slot>().number = itemNumber;
                    slot[i].GetComponent<Slot>().description = itemDescripton;
                    slot[i].GetComponent<Slot>().amount += itemAmount;
                    slot[i].GetComponent<Slot>().icon = itemIcon;

                    itemObject.transform.parent = slot[i].transform;
                    itemObject.SetActive(false);

                    slot[i].GetComponent<Slot>().empty = false;
                    slot[i].GetComponent<Slot>().UpdateSlot();

                    return;
                }
            }
        }
        if (potKontrol == false)
        {
            for (int i = 0; i < allSlots; i++)
            {

                if (slot[i].GetComponent<Slot>().empty == true)
                {
                    itemObject.GetComponent<item>().pickedUp = true;
                    //Debug.Log("Boş slottasın");

                    slot[i].GetComponent<Slot>().item = itemObject;
                    slot[i].GetComponent<Slot>().ID = itemID;
                    slot[i].GetComponent<Slot>().type = itemType;
                    slot[i].GetComponent<Slot>().number = itemNumber;
                    slot[i].GetComponent<Slot>().description = itemDescripton;
                    slot[i].GetComponent<Slot>().amount = itemAmount;
                    slot[i].GetComponent<Slot>().icon = itemIcon;

                    itemObject.transform.parent = slot[i].transform;
                    itemObject.SetActive(false);

                    slot[i].GetComponent<Slot>().empty = false;
                    slot[i].GetComponent<Slot>().UpdateSlot();

                    return;
                }

            }
        }
        
    }

    /*
    public void RemoveItemAtSlot()
    {
        Debug.Log("RemoveItemAtSlot dasın Player.cs");
        slot[1].GetComponent<Slot>().item = null;
        slot[1].GetComponent<Slot>().ID = 0;
        slot[1].GetComponent<Slot>().type = null;
        slot[1].GetComponent<Slot>().number = 0;
        slot[1].GetComponent<Slot>().description = null;
        slot[1].GetComponent<Slot>().amount = 0;
        slot[1].GetComponent<Slot>().icon = null;

        slot[1].GetComponent<Slot>().empty = true;
        slot[1].GetComponent<Slot>().EmptySlot();
    }
    */
}
