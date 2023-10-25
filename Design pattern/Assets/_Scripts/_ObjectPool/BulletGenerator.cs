using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Player
{
    public class BulletGenerator : Observable
    {
        //public int numberOfBullets = 50;
        public float radius = 1.0f;
        public BulletPool bulletPool;
        public GameObject pistol;
        public Animator pistolAnim;
        public float _fireRate = 0;
        public Transform shootPosition;
        public float forceMultiplicator = 10;
        public GameObject _bulletPrefab;
        [SerializeField] float skillTimer;
        public bool shotGunShot; 

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

        private void Start()
        {
            SetUp();
        }

        void Update()
        {
            if (menu.gameStarted)
            {
                skillTimer = 10;
                skillTimer -= Time.time;
            }
            Vector3 mouseInWorldSpace = Camera.main.ScreenToWorldPoint
                (new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.y));
            Vector3 direction = mouseInWorldSpace - transform.position;

            float angle = Mathf.Acos(Vector3.Dot(Vector3.forward, direction.normalized)) * Mathf.Rad2Deg;
            pistol.transform.rotation = Quaternion.LookRotation(direction);

            if (Input.GetMouseButtonDown(0) && !shotGunShot)
            {
                FireBurst(direction);
                Notify(this.gameObject, Action.OnPlayerShoot);
                shotGunShot = true; 
            }

            if (Input.GetKeyDown(KeyCode.R) && shotGunShot)
            {
                for (int i = 0; i < 20; i++)
                {
                    bulletPool.ReturnBullet();
                }
                shotGunShot = false; 
            }

            if (skillTimer <= 0)
            {
                skillTimer += Time.time;
                if (Input.GetMouseButtonDown(0) && _fireRate < Time.time)
                {
                    ShootBullet(direction);
                    Notify(this.gameObject, Action.OnPlayerShoot);
                    _fireRate = Time.time + 0.5f;
                }
            }
        }
        void ShootBullet(Vector3 direction)
        {
            pistolAnim.SetTrigger("Shoot");
            GameObject bullet = Instantiate(_bulletPrefab, shootPosition.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * forceMultiplicator;
        }

        void SingleShot()
        {
            GameObject newBullet = bulletPool.GetBullet();
        }
        void FireBurst(Vector3 direction)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject newBullet = bulletPool.GetBullet();
                newBullet.transform.position = shootPosition.transform.position;
                Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();

                float angleIncrement = 360f / Random.Range(-90, 90);

                if (angleIncrement != 0) //to make sure angle is never i*0
                {
                    float angle = i * angleIncrement;
                    Vector3 spawnPosition = shootPosition.transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
                    Vector3 bulletDirection = (spawnPosition - shootPosition.transform.position).normalized;
                    bulletRigidbody.AddForce(bulletDirection * 10f, ForceMode.VelocityChange);
                }
            }
            return;
        }
    }   
}
