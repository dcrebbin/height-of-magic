using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudRelocator : MonoBehaviour
{
    public GameObject startPoint;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cloud"))
        {
            other.transform.position = new Vector3(startPoint.transform.position.x, other.transform.position.y, other.transform.position.z);
        }
    }
}
