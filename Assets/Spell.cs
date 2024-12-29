using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public void Awake()
    {
        Destroy(gameObject, 5f);
    }

   public void OnTriggerEnter2D(Collider2D other)
   {
        if(other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
   }
}