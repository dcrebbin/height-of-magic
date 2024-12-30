using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public SpriteRenderer mainBody;
    public Material hitMaterial;
    private Material originalMaterial;

    private ScreenShake screenShake;
    private UxController uxController;
    private Attack attack;
    private movement movement;
    private SoundManager soundManager;

    private qual qual;

    public GameObject[] hearts;


    [SerializeField]
    private GameObject[] shieldPieces;

    private Animator animator;
    void Start()
    {
        originalMaterial = mainBody.material;
        screenShake = FindObjectOfType<ScreenShake>();
        attack = GetComponent<Attack>();
        movement = GetComponent<movement>();
        animator = GetComponent<Animator>();
        qual = FindObjectOfType<qual>();
        uxController = FindObjectOfType<UxController>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemySpell"))
        {
            if (attack.isDefending || movement.isSleeping)
            {
                if (shieldPieces.Length > 0)
                {
                    soundManager.playLilHitSound();
                    screenShake.TriggerShake(0.05f, 0.05f);
                    int randomIndex = Random.Range(0, shieldPieces.Length);
                    Destroy(shieldPieces[randomIndex]);
                    shieldPieces = RemoveAt(shieldPieces, randomIndex);
                }
                Destroy(other.gameObject);
                return;
            }


            if (hearts.Length > 0)
            {
                Destroy(hearts[0]);
                hearts = RemoveAt(hearts, 0);
                if (hearts.Length == 0)
                {
                    StartCoroutine(Die());
                }
            }

            soundManager.playHitSound();

            mainBody.material = hitMaterial;
            StartCoroutine(ResetMaterial());
            screenShake.TriggerShake(0.3f, 0.1f);

            Destroy(other.gameObject);
        }
    }

    IEnumerator Die()
    {
        var rb = GetComponent<Rigidbody2D>();
        animator.SetTrigger("dead");
        enabled = false;
        attack.enabled = false;
        movement.enabled = false;
        qual.StopShooting();
        uxController.ShowDefeatScreen();
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    private GameObject[] RemoveAt(GameObject[] array, int index)
    {
        if (array == null || index < 0 || index >= array.Length)
        {
            return array;
        }

        List<GameObject> list = new List<GameObject>(array);
        list.RemoveAt(index);
        return list.ToArray();
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        mainBody.material = originalMaterial;
    }
}
