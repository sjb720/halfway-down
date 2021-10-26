using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class RunAwayTrigger : MonoBehaviour
{
    public float teleportX = 100;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.transform.position = new Vector3(transform.position.x + teleportX, collision.transform.position.y, collision.transform.position.z);
            Camera.main.GetComponent<ProCamera2D>().VerticalFollowSmoothness = 0;
            Camera.main.GetComponent<ProCamera2D>().HorizontalFollowSmoothness = 0;

            StartCoroutine(AddSmoothnessBack());
        }
    }

    IEnumerator AddSmoothnessBack()
    {
        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<ProCamera2D>().VerticalFollowSmoothness = 0.15f;
        Camera.main.GetComponent<ProCamera2D>().HorizontalFollowSmoothness = 0.15f;
    }
}
