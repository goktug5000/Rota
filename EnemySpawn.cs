using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject Enemy01;

    void Start()
    {
        //SpawnEnemy01(3);
    }

    void Update()
    {
        
    }
    public void SpawnEnemy01(int a)
    {
        for(int i = 1; i <= a; a--)
        {
            Instantiate(Enemy01, new Vector3(Random.Range(96, 172), 0, Random.Range(-160, -95)), Quaternion.Euler(0,Random.Range(0,360),0));
        }
        
    }

}
