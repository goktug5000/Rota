using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public GameObject player,myHP,CanvasController,enemySpawner;
    public GameObject coin;//KESİNCE DÜŞENLER
    public ForCanvas forCanvasScript;
    public Player PlayerScript;
    public NavMeshAgent _agent;
    public float distance,normalDistance,degiskenDistance,öfkeliDistance,HP=50,MaxHP=50,attackCD;
    public EnemySpawn enemySpawnCode;

    public string myStat;//beyaz,sari,kirmizi


    private void Start()
    {
        player = GameObject.Find("Player");
        enemySpawner = GameObject.Find("Spawn Enemy");
        this.gameObject.transform.parent = enemySpawner.transform;

        PlayerScript = player.transform.GetComponent<Player>();
        CanvasController = GameObject.Find("GameCanvasController");
        forCanvasScript = CanvasController.transform.GetComponent<ForCanvas>();
        enemySpawnCode = enemySpawner.transform.GetComponent<EnemySpawn>();

        if (normalDistance == 0)
        {
            normalDistance =degiskenDistance = 6;
            öfkeliDistance = normalDistance * 1.8f;
        }
        
    }
    void Update()
    {

//BENİM HERİFE DOĞRU KOŞMASI İÇİN
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < degiskenDistance)
        {
            _agent.SetDestination(player.transform.position);
        }

        if (distance < 1.5)
        {
            _agent.speed = 0;
            PlayerScript.distanceBetweenEnemy(this.gameObject);
        }
        else
        {
            _agent.speed = 2;
        }
//BANA KOŞERKEN ATEŞ EDİNCE DAHA UZAKTAN BENİ GÖRMESİ
        if(normalDistance < degiskenDistance)
        {
            degiskenDistance -= 1 * Time.deltaTime;
        }


//SALDIRI
        if (distance <= 1.6 && attackCD <= 0)
        {
            PlayerScript.receiveDamagePlayer(10);
            attackCD = 2;
        }
        if (attackCD >=0) {
            attackCD -= Time.deltaTime;
        }
        myHP.GetComponent<TextMeshPro>().text = HP.ToString();
        

//ÖL ve Exp ver
        if (HP <= 0)
        {
            int myGörevNum = 0;
            if (myStat == "beyaz")
            {
                myGörevNum = 1;
            }
            PlayerScript.checkQuests(myGörevNum);
            PlayerScript.gainExp(5);
            forCanvasScript.intoToWar(0);
            forCanvasScript.atCombatCD = 3/5f;

            try
            {
                if (myStat == "beyaz")
                {
                    Debug.Log("eşya düşürme Enemy.cs");
                    int kullanAt = Random.Range(0, 3);
                    if (kullanAt > 0)
                    {
                        var newObj = Instantiate(coin, this.gameObject.transform.position, this.gameObject.transform.rotation);
                        newObj.GetComponent<item>().amount = kullanAt;
                    }
                }
            }
            catch
            {

            }

            enemySpawnCode.SpawnEnemy01(1);
            Destroy(this.gameObject);
        }
    }

//RECEIVE DAMAGE
    public void receiveDamage(float dmg)
    {
        HP -= dmg;
        degiskenDistance = öfkeliDistance;
        //Debug.Log("Enemy HP: "+HP);
    }

}
