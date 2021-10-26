using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicLight2D;

public class CageLight : MonoBehaviour
{

    public bool staticLight = true;

    // Start is called before the first frame update
    void Start()
    {
        if(staticLight)
        {
            StartCoroutine(StaticLight());
        }
    }

    IEnumerator StaticLight()
    {
        yield return new WaitForEndOfFrame();
        transform.Find("2DDLight").GetComponent<DynamicLight>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
