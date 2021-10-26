using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicLight2D;

public class LightOptimizer : MonoBehaviour
{
    public bool staticLight = true;

    Transform cam;

    public DynamicLight light;
    GameObject lightGameObject;


    // Start is called before the first frame update
    void Start()
    {
        if (staticLight)
            StartCoroutine(BakeStaticLight());

        if (!staticLight)
        {
            cam = Camera.main.transform;
            lightGameObject = light.gameObject;
            lightGameObject.SetActive(false);
        }
    }

    IEnumerator BakeStaticLight()
    {
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(0.5f);
        
        light.enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (staticLight) return;

        float distance = Vector2.Distance(transform.position, cam.position);

        if (distance < 22 && !lightGameObject.activeSelf)
        {
            lightGameObject.SetActive(true);
        }

        else if (distance > 26 && lightGameObject.activeSelf)
        {
            lightGameObject.SetActive(false);

        }

    }
}
