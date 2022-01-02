using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConfig : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    [SerializeField] float m_Thrust = 20f;
    [SerializeField] GameObject hitEffect;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
            m_Rigidbody.AddForce(transform.up * m_Thrust, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitVFX = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(hitVFX, 1f);
        Destroy(gameObject);
    }

}
