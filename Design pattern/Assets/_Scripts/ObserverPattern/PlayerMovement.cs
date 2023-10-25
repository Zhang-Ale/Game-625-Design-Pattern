using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : Observable
{
    public GameObject particle, PUparticle; 
    public bool playerDead;
    GameObject[] Enemies;
    GameObject[] PowerUps;

    public GameObject pistol;

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        if (menu.gameStarted)
        {
            spawner.GetComponent<Spawner>().enabled = true;
            RB.constraints = RigidbodyConstraints.None;
            RB.constraints = RigidbodyConstraints.FreezePositionY;
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 motion = move * Time.deltaTime * moveSpeed;
            CC.Move(motion);
        }

        if (GetComponent<Rigidbody>().velocity.magnitude != 0 && !playerDead)
        {
            InstantiateParticle(particle);
        }

        if (poweredUp)
        {
           StartCoroutine(PowerUpTime(PUparticle)); 
        }
        else
        {
            StopCoroutine(PowerUpTime(PUparticle));
        }

        if (playerDead)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in Enemies)
            {
                enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                enemy.GetComponent<NavMeshAgent>().speed = 0; 
            }

            PowerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (GameObject powerUp in PowerUps)
            {
                powerUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                powerUp.GetComponent<NavMeshAgent>().speed = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if (!poweredUp)
            {
                playerDead = true;
                menu.gameStarted = false;
                Destroy(spawner);
                menu.StopGame(); 
            }
            else
            {
                playerDead = false;
                Destroy(other.gameObject);
                menu.AddPoint(); 
            }
        }

        if(other.tag == "PowerUp" && !playerDead)
        {
            poweredUp = true;
            Notify(this.gameObject, Action.OnPowerUpCollect);
            Destroy(other.gameObject);
            spawner.Invoke("QuickSpawn", 5);
        }
    }

    private void Awake()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        AddObserver(gm);
    }

    private void OnDisable()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        RemoveObserver(gm);
    }
}
