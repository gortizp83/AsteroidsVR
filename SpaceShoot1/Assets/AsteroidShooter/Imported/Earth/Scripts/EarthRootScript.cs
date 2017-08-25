using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.Earth
{
    public class EarthRootScript : MonoBehaviour
    {
        [SerializeField] GameObject m_collisionEffect;
        [SerializeField] GameObject m_earth;

        private void OnTriggerEnter(Collider other)
        {
            Instantiate(m_collisionEffect, other.transform.position, Quaternion.identity);
        }

        //void OnCollisionEnter(Collision colInfo)
        //{
        //    Instantiate(m_collisionEffect, colInfo.contacts[0].point, Quaternion.LookRotation(colInfo.contacts[0].normal));
        //}
    }
}
