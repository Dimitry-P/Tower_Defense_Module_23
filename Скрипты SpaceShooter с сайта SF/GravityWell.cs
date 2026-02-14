using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class GravityWell : MonoBehaviour
{
    [SerializeField] private float m_Force;
    [SerializeField] private float m_Radius;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.attachedRigidbody == null)
            return;

        Vector2 delta = transform.position - other.transform.position;

        float distance = delta.magnitude;

        if (distance < m_Radius)
        {
            Vector2 force = delta.normalized * m_Force * (distance / m_Radius);
            other.attachedRigidbody.AddForce(force, ForceMode2D.Force);
        }
    }

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().radius = m_Radius;
    }
}
