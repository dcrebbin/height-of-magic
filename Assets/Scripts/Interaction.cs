using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public SpriteRenderer mainBody;
    public Material hitMaterial;
    private Material originalMaterial;

    private ScreenShake screenShake;

    private Attack attack;
    private movement movement;


    [SerializeField]
    private GameObject[] shieldPieces;
    void Start()
    {
        originalMaterial = mainBody.material;
        screenShake = FindObjectOfType<ScreenShake>();
        attack = GetComponent<Attack>();
        movement = GetComponent<movement>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemySpell"))
        {
            if (attack.isDefending || movement.isSleeping)
            {
                if (shieldPieces.Length > 0)
                {
                    screenShake.TriggerShake(0.05f, 0.05f);
                    int randomIndex = Random.Range(0, shieldPieces.Length);
                    Destroy(shieldPieces[randomIndex]);
                    shieldPieces = RemoveAt(shieldPieces, randomIndex);
                }
                Destroy(other.gameObject);
                return;
            }
            mainBody.material = hitMaterial;
            StartCoroutine(ResetMaterial());
            screenShake.TriggerShake(0.3f, 0.1f);

            Destroy(other.gameObject);
        }
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
