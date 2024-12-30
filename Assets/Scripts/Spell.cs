using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public bool isPlayerSpell = false;

    public void Awake()
    {
        Destroy(gameObject, 5f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Destructible"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Wall") || (other.gameObject.CompareTag("PlayerSpell") && !isPlayerSpell) || (other.gameObject.CompareTag("EnemySpell") && isPlayerSpell))
        {

            Destroy(gameObject);
        }
    }

}
