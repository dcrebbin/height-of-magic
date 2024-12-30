using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    public bool isCharging = false;

    [SerializeField] private float spellSpeed = 7f;

    private movement movement;

    [SerializeField] private float chargeTime = 0f;

    [SerializeField] private float omegaSpellChargeTime = 3f;

    public GameObject omegaSpell;

    public Vector3 direction = new Vector3(1, 0, 0);


    public Transform spellCircle;

    public bool isDefending = false;

    public bool isAbovePlayer = false;
    public bool isBelowPlayer = false;

    public SoundManager soundManager;

    [SerializeField] private GameObject spellPrefab;
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<movement>();
        soundManager = FindObjectOfType<SoundManager>();
    }


    IEnumerator ChargeSpell()
    {
        chargeTime = 0f;
        while (isCharging)
        {
            chargeTime += Time.deltaTime;

            if (chargeTime > omegaSpellChargeTime)
            {
                soundManager.playDashRenewSound();
            }
            yield return null;
        }
    }

    void LateUpdate()
    {
        animator.SetBool("isDefending", isDefending);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isDefending = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isDefending = false;
        }

        if (isDefending)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //get mouse position
            spellCircle.rotation = Quaternion.Euler(0, 0, 0);
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isCharging = true;
            Debug.Log("Mouse position: " + mousePosition);
            Debug.Log("Player position: " + transform.position);
            isAbovePlayer = (mousePosition.y > transform.position.y + 1);
            isBelowPlayer = (mousePosition.y < transform.position.y - 1);

            if (isAbovePlayer)
            {
                spellCircle.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? 90 : -90);
                Debug.Log("Is above player");
            }
            else if (isBelowPlayer)
            {
                Debug.Log("Is below player");
                spellCircle.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? -90 : 90);
            }


            animator.SetBool("isCharging", true);
            StartCoroutine(ChargeSpell());
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            float originalSpeed = spellSpeed;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isAbovePlayer = (mousePosition.y > transform.position.y + 1);
            isBelowPlayer = (mousePosition.y < transform.position.y - 1);

            if (chargeTime > omegaSpellChargeTime)
            {
                StartCoroutine(Lazor());
                StopCoroutine(ChargeSpell());
                isCharging = false;
                animator.SetBool("isCharging", false);
                return;
            }
            StopCoroutine(ChargeSpell());
            isCharging = false;
            animator.SetBool("isCharging", false);
            var spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
            if (isAbovePlayer)
            {

                spell.transform.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? 90 : 90);
                spell.GetComponent<Rigidbody2D>().velocity = transform.up * spellSpeed;
            }
            else if (isBelowPlayer)
            {
                spell.transform.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? -90 : -90);
                spell.GetComponent<Rigidbody2D>().velocity = -transform.up * spellSpeed;
            }
            else
            {
                spell.transform.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? 0 : 180);
                spell.transform.position += movement.isFacingRight ?
                transform.right + new Vector3(0.5f, 0, 0) :
                -transform.right + new Vector3(-0.5f, 0, 0);
                spell.GetComponent<Rigidbody2D>().velocity = movement.isFacingRight ?
                transform.right * spellSpeed :
                -transform.right * spellSpeed;
            }
        }
    }

    IEnumerator ShakeSpell()
    {
        while (true)
        {
            omegaSpell.transform.position = new Vector3(omegaSpell.transform.position.x, omegaSpell.transform.position.y + Random.Range(-0.01f, 0.01f), omegaSpell.transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator Lazor()
    {
        var shots = 0;
        omegaSpell.SetActive(true);
        var shakeCoroutine = StartCoroutine(ShakeSpell());

        while (shots < 20)
        {
            shots++;
            var spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);

            if (isAbovePlayer)
            {
                spell.transform.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? 90 : 90);
                spell.GetComponent<Rigidbody2D>().velocity = transform.up * spellSpeed;
            }
            else if (isBelowPlayer)
            {
                spell.transform.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? -90 : -90);
                spell.GetComponent<Rigidbody2D>().velocity = -transform.up * spellSpeed;
            }
            else
            {
                spell.transform.rotation = Quaternion.Euler(0, 0, movement.isFacingRight ? 0 : 180);
                spell.GetComponent<Rigidbody2D>().velocity = movement.isFacingRight ?
                transform.right * spellSpeed :
                -transform.right * spellSpeed;
                spell.transform.position += movement.isFacingRight ?
                transform.right + new Vector3(0.5f, 0, 0) :
                -transform.right + new Vector3(-0.5f, 0, 0);
            }


            yield return new WaitForSeconds(0.01f);
        }
        StopCoroutine(shakeCoroutine);
        omegaSpell.transform.position = transform.position;
        omegaSpell.SetActive(false);
    }
}
