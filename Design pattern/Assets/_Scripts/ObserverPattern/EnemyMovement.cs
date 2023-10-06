using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : Observable
{
    GameObject player;
    public GameObject part, PUparticle;
    EnemyHealth EH;

    void Start()
    {
        SetUp();
        player = GameObject.FindGameObjectWithTag("Player");
        EH = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (menu.gameStarted && !EH.notified)
        {
            agent.speed = 30;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.speed = 0;
            agent.SetDestination(this.gameObject.transform.position);
        }

        
        if(GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            InstantiateParticle(part);
        }

        if (poweredUp)
        {
            StartCoroutine(PowerUpTime(PUparticle));
        }
        else
        {
            StopCoroutine(PowerUpTime(PUparticle));
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            poweredUp = true;
            Destroy(other.gameObject);
            spawner.Invoke("QuickSpawn", 5);
        }
    }
}
