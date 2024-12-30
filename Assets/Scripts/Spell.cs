using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public bool isPlayerSpell = false;

    private SoundManager soundManager;

    public GameObject splashPrefab;

    public void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        Destroy(gameObject, 5f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerSpell") && !isPlayerSpell)
        {
            Destroy(other.gameObject);
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * 0.7f;
            soundManager.playHitSound();
            return;
        }

        if (other.gameObject.CompareTag("Destructible"))
        {
            Destroy(gameObject);
            var animator = other.gameObject.GetComponent<Animator>();
            var boxCollider = other.gameObject.GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            animator.SetTrigger("explode");
            soundManager.playBigHitSound();
            var splash = Instantiate(splashPrefab, other.gameObject.transform.position, Quaternion.identity);

            splash.transform.rotation = transform.rotation;
            return;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            soundManager.playLilHitSound();
            Destroy(gameObject);
        }
    }

}
