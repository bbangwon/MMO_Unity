using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision @ {collision.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {        
        Debug.Log($"Trigger @ {other.gameObject.name}");
    }

    private void Update()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);


        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        foreach (var hit in hits)
        {
            Debug.Log($"Raycast {hit.collider.gameObject.name}");
        }
    }
}
