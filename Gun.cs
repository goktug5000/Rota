using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gun : MonoBehaviour
{
    public Camera cam;

    public GameObject fovStartPoint,player,mermi;
    private Vector3 enemyDir;

    public NavMeshAgent agent;
    public GameObject selectedObject;

    public Enemy EnemyScript;
    private Quaternion targetRotation;

    public float playerY,benimGucum;

    public string yön;

    private void Start()
    {
        benimGucum = 100;
    }
    void Update()
    {
        playerY = player.transform.eulerAngles.y;
        if (Input.GetMouseButton(0))
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


                if (hit.transform.tag != "EnemyTag")
                {
                    selectedObject = null;
                    EnemyScript = null;
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
                targetRotation = Quaternion.LookRotation(enemyDir);
                Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, 200f);
                transform.rotation = lookAt;
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Euler(270, 0, playerY);
            }
        }
        else
        {
            targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Euler(270, 0, playerY);
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z)
            var mermii = Instantiate(mermi, this.gameObject.transform.position, this.gameObject.transform.rotation);

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
