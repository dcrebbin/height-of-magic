using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qual : MonoBehaviour
{

    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x < transform.position.x){
            transform.localScale = new Vector3(-1, 1, 1);
        }else{
            transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
}
