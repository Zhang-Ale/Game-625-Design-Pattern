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
    public float _fireRate = 0;
    public Transform shootPosition;
    public GameObject _bulletPrefab;
    public float forceMultiplicator = 10;
    public GameObject pistol;
    public Animator pistolAnim;

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

        Vector3 mouseInWorldSpace = Camera.main.ScreenToWorldPoint
            (new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.y));
        Vector3 direction = mouseInWorldSpace - transform.position;

        float angle = Mathf.Acos(Vector3.Dot(Vector3.forward, direction.normalized)) * Mathf.Rad2Deg;
        pistol.transform.rotation = Quaternion.LookRotation(direction);
        if (Input.GetMouseButtonDown(0) && _fireRate < Time.time)
        {
            ShootBullet(direction);
            Notify(this.gameObject, Action.OnPlayerShoot);
            _fireRate = Time.time + 0.5f;
        }
    }
    void ShootBullet(Vector3 direction)
    {
        pistolAnim.SetTrigger("Shoot");
        GameObject bullet = Instantiate(_bulletPrefab, shootPosition.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = direction * forceMultiplicator;
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
