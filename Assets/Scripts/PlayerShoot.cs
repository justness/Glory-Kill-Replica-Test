using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Camera cam;
    float range = 50;
    float knockback = 100;

    public GameObject healthOrb;
    LineRenderer lr;
    public Material lrMaterial;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range))
            {
                //For creating line renderer object
                lr = new GameObject("Line").AddComponent<LineRenderer>();
                lr.material = lrMaterial;
                lr.startWidth = 0.01f;
                lr.endWidth = 0.01f;
                lr.positionCount = 2;
                lr.useWorldSpace = true;
                //For drawing line in the world space, provide the x,y,z values
                lr.SetPosition(0, cam.transform.GetChild(0).transform.position); //x,y and z position of the starting point of the line
                lr.SetPosition(1, hit.point); //x,y and z position of the starting point of the line
                Destroy(lr.gameObject, 0.5f);

                if (hit.rigidbody != null)
                {
                    EnemyController ec = hit.collider.gameObject.GetComponent<EnemyController>();
                    ec.health--;
                    ec.aggro = false;
                    ec.aggroTimer = 0.75f; // Shooting an un-aggro'd enemy makes it aggro.
                    hit.rigidbody.AddForce(-hit.normal * knockback);
                    ec.HurtAnimation();
                }
            }
            else
            {
                Debug.Log("Missed.");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range))
            {
                if (hit.rigidbody != null)
                {
                    EnemyController ec = hit.collider.gameObject.GetComponent<EnemyController>();
                    if (ec.staggered)
                    {
                        StartCoroutine(glory(hit.collider.gameObject));
                    }
                }
            }
        }
    }

    IEnumerator glory(GameObject hitObject)
    {
        transform.position = hitObject.transform.position - 1.5f * Vector3.Normalize((hitObject.transform.position - transform.position)/2);
        GameObject[] healthOrbs = new GameObject[5];
        for (int i = 0; i < healthOrbs.Length; i++)
        {
            healthOrbs[i] = Instantiate(healthOrb, hitObject.transform.position, Quaternion.identity);
            healthOrbs[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 25, 0));
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(hitObject);
    }
}
