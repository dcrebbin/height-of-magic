using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class qual : MonoBehaviour
{

    private GameObject player;


    private UxController uxController;
    public bool isFacingRight = false;
    public float speed = 10f;
    public GameObject spell;

    public SpriteRenderer mainBody;

    public SpriteRenderer shadow;

    public Image healthBar;

    public Material hitMaterial;

    private Material originalMaterial;

    [SerializeField]
    private float health = 100f;

    public bool canShoot = true;

    private Coroutine shootCoroutine;

    private SoundManager soundManager;

    private ScreenShake screenShake;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        uxController = FindObjectOfType<UxController>();
        shootCoroutine = StartCoroutine(ShootCoroutine());
        originalMaterial = mainBody.material;
        screenShake = FindObjectOfType<ScreenShake>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            var shootingDelay = Random.Range(3f, 5f);
            yield return new WaitForSeconds(shootingDelay);
            Shoot();
        }
    }

    void Update()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }

    }

    public void Shoot()
    {
        Vector3 spawnPosition = transform.position + ((player.transform.position - transform.position).normalized * 1.75f);
        var instance = Instantiate(spell, spawnPosition, transform.rotation);
        var spellRb = instance.GetComponent<Rigidbody2D>();

        spellRb.velocity = (player.transform.position - spawnPosition).normalized * speed;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerSpell"))
        {
            mainBody.material = hitMaterial;
            StartCoroutine(ResetMaterial());
            screenShake.TriggerShake(0.1f, 0.05f);
            soundManager.playHitSound();
            Destroy(other.gameObject);
            health -= 0.5f;
            healthBar.fillAmount = health / 100f;

            if (health <= 0)
            {
                mainBody.enabled = false;
                shadow.enabled = false;
                screenShake.TriggerShake(0.5f, 0.5f);
                uxController.ShowVictoryScreen();
                StartCoroutine(Die());
            }
        }
    }

    public void StopShooting()
    {
        StopCoroutine(shootCoroutine);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);
        Destroy(healthBar.transform.parent.gameObject);
        Destroy(gameObject);
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.1f);
        mainBody.material = originalMaterial;
    }
}
