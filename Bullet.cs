using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float id,myLifeTime,myMaxTime,hasar;
    public string hasarTürü;
    private Rigidbody rgb;
    public GameObject gunn;
    void Start()
    {
        gunn = GameObject.Find("R-Gun");

        if (id == 1)
        {
            myLifeTime = myMaxTime = 0.4f;
            hasar = 10;
            hasarTürü = "";
        }
        if (id == 31)
        {
            myLifeTime = myMaxTime = 1f;
            hasar = 10;
            hasarTürü = "utandırma";
        }
        rgb = this.gameObject.GetComponent<Rigidbody>();
        rgb.AddForce(transform.forward * gunn.GetComponent<Gun>().benimGucum);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyTag")
        {
            other.GetComponent<Enemy>().receiveDamage(hasar);
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        if (myLifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            myLifeTime -= 1 * Time.deltaTime;
        }

        //this.gameObject.transform.Translate(0, 0, 0.5f);
    }
}
