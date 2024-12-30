using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMoving : MonoBehaviour
{

    public float cloudSpeed = 1f;

    void Start()
    {
        StartCoroutine(MoveCloud());
    }

    IEnumerator MoveCloud()
    {
        while (true)
        {
            transform.Translate(Vector3.right * cloudSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
