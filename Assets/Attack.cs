using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private bool isCharging = false;

    [SerializeField] private float spellSpeed = 10f;

    private movement movement;

    [SerializeField] private float chargeTime = 0f;

    public GameObject omegaSpell;

    public bool isDefending = false;

    [SerializeField] private GameObject spellPrefab;
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<movement>();
    }


    IEnumerator ChargeSpell()
    {
        chargeTime = 0f;
        while(isCharging)
        {
            chargeTime += Time.deltaTime;
            yield return null;
        }
    }

    void LateUpdate(){
        animator.SetBool("isDefending", isDefending);
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse1)){
            isDefending = true;
        }
        if(Input.GetKeyUp(KeyCode.Mouse1)){
            isDefending = false;
        }

        if(isDefending){
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            isCharging = true;
            animator.SetBool("isCharging", true);
            StartCoroutine(ChargeSpell());
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            float originalSpeed = spellSpeed;
            if(chargeTime > 1f)
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
            spell.GetComponent<Rigidbody2D>().velocity = movement.isFacingRight ? transform.right * spellSpeed : -transform.right * spellSpeed;
        }
    }

    IEnumerator ShakeSpell(){
        while(true){
            omegaSpell.transform.position = new Vector3(omegaSpell.transform.position.x, omegaSpell.transform.position.y + Random.Range(-0.01f, 0.01f), omegaSpell.transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator Lazor(){
        var shots = 0;
        omegaSpell.SetActive(true);
        var shakeCoroutine = StartCoroutine(ShakeSpell());
        
        while(shots < 20) 
        {
            shots++;
            var spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
            spell.GetComponent<Rigidbody2D>().velocity = movement.isFacingRight ? 
                transform.right * spellSpeed : 
                -transform.right * spellSpeed;            
            spell.transform.position += movement.isFacingRight ? 
                    transform.right : 
                    -transform.right;
            
            yield return new WaitForSeconds(0.01f);
        }
        StopCoroutine(shakeCoroutine);
        omegaSpell.transform.position = transform.position;
        omegaSpell.SetActive(false);
    }
}
