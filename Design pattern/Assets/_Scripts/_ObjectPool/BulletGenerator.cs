using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Player
{
    public class BulletGenerator : MonoBehaviour
    {

        public GameObject bulletPrefab;
        public Transform firePoint;
        //public int numberOfBullets = 50;
        public float radius = 1.0f;
        public BulletPool bulletPool;

        float Time;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //testing if ReturnBullet works
            if (Input.GetKeyDown(KeyCode.B))
            {
                for (int i = 0; i < 20; i++)
                {
                    bulletPool.ReturnBullet();
                }
            }

            //Player shoots
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBurst();
                
            }

            
        }

        void SingleShot()
        {
            GameObject newBullet = bulletPool.GetBullet();
        }
        void FireBurst()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject newBullet = bulletPool.GetBullet();
                newBullet.transform.position = firePoint.position;
                Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
                float angleIncrement = 360f / Random.Range(-90, 90);

                if (angleIncrement != 0) //to make sure angle is never i*0
                {
                    float angle = i * angleIncrement;
                    Vector3 spawnPosition = firePoint.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
                    Vector3 bulletDirection = (spawnPosition - firePoint.position).normalized;
                    bulletRigidbody.AddForce(bulletDirection * 10f, ForceMode.VelocityChange);
                }
            }

            return;

        }

    }
}
