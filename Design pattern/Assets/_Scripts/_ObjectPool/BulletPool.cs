using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObjectPool.Player
{

    public class BulletPool : ObjectPool
    {
        public GameObject bulletPrefab;

        private List<GameObject> bulletsInUse = new List<GameObject>();
        private List<GameObject> bulletsNotInUse = new List<GameObject>();
        

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < INITIAL_POOL_SIZE; i++)
            {
                GenerateBullet();
            }

        }

       
        private void GenerateBullet()
        {
            GameObject newBullet = Instantiate(bulletPrefab.gameObject, transform);
            newBullet.SetActive(false);
            bulletsNotInUse.Add(newBullet);
        }

        public GameObject GetBullet()
        {
            //int totalNotInUse = bulletsNotInUse.Count;

            //Try to find an inactive bullet
            foreach (GameObject bullet in bulletsNotInUse)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.SetActive(true);
                    bulletsNotInUse.Remove(bullet);
                    bulletsInUse.Add(bullet);
                    
                    return bullet;
                }
            }
            return null;
            /*if (bulletsNotInUse.Count < MAX_POOL_SIZE)
            {
                GenerateBullet();

                //The new bullet is last in the list so get it
                GameObject lastBullet = bulletsInUse[^1];

                lastBullet.SetActive(true);

                return lastBullet;
            }
            
            return null;*/
            
        }
        
        public GameObject ReturnBullet()
        {

                foreach (GameObject bullet in bulletsInUse)
                {
                    if (bullet.activeInHierarchy)
                    {
                        bullet.SetActive(false);
                        bulletsInUse.Remove(bullet);
                        bulletsNotInUse.Add(bullet);
                        return bullet;
                    }
                }
            
            return null;
        }
    }
}
