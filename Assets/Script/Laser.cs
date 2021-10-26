using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collision.gameObject.layer != 9)
        {
            if(!collision.gameObject.GetComponent<Player>().dead)
                collision.gameObject.GetComponent<Player>().Die();
        }
    }
}
