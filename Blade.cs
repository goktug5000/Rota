using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blade : MonoBehaviour
{
    public Camera cam;

    public GameObject fovStartPoint, player;
    public ForCanvas ForCanvasScript;
    private Vector3 enemyDir;

    public NavMeshAgent agent;
    public GameObject selectedObject;

    public Enemy EnemyScript;
    public Resource ResourceScript;
    private Quaternion targetRotation;

    public bool atCombatB,hizlanabilir;

    public float playerY , distance, attackSpeed = 0.5f, attackSpeedCD, Damage=1;

    public string giyilenEsya;//kilic, balta

    void Update()
    {
        playerY = player.transform.eulerAngles.y;
        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.tag == "EnemyTag")
                {
                    selectedObject = hit.transform.gameObject;
                    EnemyScript = selectedObject.transform.GetComponent<Enemy>();
                }
                if (hit.transform.tag == "Resource"&&giyilenEsya=="balta")
                {
                    selectedObject = hit.transform.gameObject;
                    ResourceScript = selectedObject.transform.GetComponent<Resource>();
                }


                if (hit.transform.tag != "EnemyTag" && hit.transform.tag != "Resource")
                {
                    selectedObject = null;
                    EnemyScript = null;
                    ResourceScript = null;
                    targetRotation = Quaternion.Euler(0, 0, 0);
                    transform.rotation = Quaternion.Euler(270, 0, playerY);
                }
            }
        }


        if (selectedObject != null)
        {
            enemyDir = selectedObject.transform.position - transform.position;
            if (EnemyInFieldOfView(fovStartPoint))
            {
                if (attackSpeedCD>=0 || attackSpeedCD<= (0.25f / 0.5f))
                {
                    targetRotation = Quaternion.LookRotation(enemyDir);
                    Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, 2000f);
                    transform.rotation = lookAt;
                }
                if (attackSpeedCD > (0.25f/0.5f))
                {
                    transform.rotation = Quaternion.Euler(270, playerY,0);
                }
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Euler(90, playerY,0);
            }
        }
        else
        {
            if (atCombatB == false)
            {
                targetRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Euler(90, playerY,0);
            }
            
        }



        try
        {
            if (selectedObject.tag == "EnemyTag")
            {
                distance = Vector3.Distance(transform.position, selectedObject.transform.position);
                if (distance < 2.5f)
                {
                    player.GetComponent<NavMeshAgent>().speed = 0;
                    hizlanabilir = false;
                    if (attackSpeedCD <= 0)
                    {
                        //Debug.Log("vuruyon");
                        attackSpeedCD = (1 / attackSpeed);
                        BasicAttack();
                        ForCanvasScript.intoToWar(7);
                    }
                    setRotationPlayer();
                }
            }
            if (selectedObject.tag == "Resource")
            {
                distance = Vector3.Distance(transform.position, selectedObject.transform.position);
                if (distance < 1.8f)
                {
                    player.GetComponent<NavMeshAgent>().speed = 0;
                    hizlanabilir = false;
                    if (attackSpeedCD <= 0)
                    {
                        //Debug.Log("vuruyon");
                        attackSpeedCD = (1 / attackSpeed);
                        BasicAttack();
                    }
                    setRotationPlayer();
                }
            }
        }
        catch
        {

        }
        if (selectedObject == null)
        {
            hizlanabilir = true;
        }


        if (attackSpeedCD > 0)
        {
            attackSpeedCD -= Time.deltaTime;
        }
    }
//SET PLAYER ROTATION
    public void setRotationPlayer()
    {
        
        Vector3 targetPostition = new Vector3(selectedObject.transform.position.x,
                                        player.transform.position.y,
                                        selectedObject.transform.position.z);
        player.transform.LookAt(targetPostition);
    }


//HASAR ÖLÇER
    public void Hasarim(float myDamage,float myAttackSpeed)
    {
        Damage = myDamage;
        attackSpeed = myAttackSpeed;
    }

//HASAR VER
    void BasicAttack()
    {
        if (EnemyScript != null)
        {
            EnemyScript.receiveDamage(Damage);
        }
        else if(ResourceScript != null)
        {
            ResourceScript.receiveDamage(Damage);
        }
        
    }

    bool EnemyInFieldOfView(GameObject looker)
    {
        float anglee = Vector3.Angle(enemyDir, looker.transform.forward);
        if (anglee < 70)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
