using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    float timerMax = 5;
    public float aggroTimer;
    public bool aggro = false;
    float staggerMax = 3;
    public float staggerTimer;
    public bool staggered = false;
    float speed = 10;

    public int health;
    public Material hitMaterial;
    public Material stagMaterial;
    Material originalMaterial;

    void Start()
    {
        aggroTimer = timerMax;
        staggerTimer = staggerMax;
        health = 5;
        originalMaterial = GetComponent<MeshRenderer>().material;
    }

    void FixedUpdate()
    {
        if (health <= 2) staggered = true;

        if (!staggered)
        {
            if (aggroTimer >= 0) aggroTimer -= Time.fixedDeltaTime;
            else
            {
                aggroTimer = timerMax;
                aggro = !aggro;
            }
            if (aggro)
            {
                GetComponent<MeshRenderer>().material = originalMaterial;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.LookAt(player.transform);
                transform.position += transform.forward * speed * Time.fixedDeltaTime;
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<MeshRenderer>().material = stagMaterial;
            staggerTimer -= Time.fixedDeltaTime;
            if (staggerTimer <= 0)
            {
                staggered = false;
                health = 4;
                staggerTimer = staggerMax;
            }
        }

        if (health <= 0) Destroy(this.gameObject);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z); // Stay grounded.
    }

    public void HurtAnimation()
    {
        GetComponent<MeshRenderer>().material = hitMaterial;
    }
}
