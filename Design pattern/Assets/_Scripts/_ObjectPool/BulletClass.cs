using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Player
{
    public class BulletClass : MonoBehaviour
    {
        [System.NonSerialized] public BulletClass next;

        //private float bulletSpeed = 10f;

        //private float deactivationDistance = 30f;

        public BulletPool objectPool;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
          
        }
        
        
    }
}
