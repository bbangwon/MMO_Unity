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
        //Vector3 look = transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);


        //RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        //foreach (var hit in hits)
        //{
        //    Debug.Log($"Raycast {hit.collider.gameObject.name}");
        //}

        //스크린 좌표계 Input.mousePosition
        //y+
        //|
        //*-- x+
        //Debug.Log(Input.mousePosition);

        //Viewport 좌표계  Screen좌표계의 0.0 ~ 1.0
        //Debug.Log(Camera.main.ScreenToViewportPoint( Input.mousePosition ));

        //Screen To WorldPosition
        if(Input.GetMouseButtonDown(0))
        {
            //Camera Ray를 바로 가져오는 방법
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.0f);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.Log($"Raycast Camera {hit.collider.gameObject.name}");
            }

            //Ray를 직접 구하는 방법
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            //Vector3 dir = mousePos - Camera.main.transform.position;
            //dir = dir.normalized;

            //Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 1.0f);
            //if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hit, 100f))
            //{
            //    Debug.Log($"Raycast Camera {hit.collider.gameObject.name}");
            //}
        }
    }
}
