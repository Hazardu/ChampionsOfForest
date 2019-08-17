using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public class ProjectileIgnoreCollision : MonoBehaviour
    {
        public Collider col;
        void Awake()
        {
            col = GetComponent<Collider>();
        }
        void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("projectile"))
            {
                Physics.IgnoreCollision(other.collider, col, true);
                Debug.Log("Colliding & ignoring");
            }
            }
    }
}
