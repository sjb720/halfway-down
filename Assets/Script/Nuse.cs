using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuse : MonoBehaviour
{
    public int length = 7;

    public GameObject rope, slipKnot;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D previousRigidBody = GetComponent<Rigidbody2D>();

        for(int i = 0; i < length; i++)
        {

            

            GameObject r1 = Instantiate(rope, transform);
            r1.transform.position = transform.position + Vector3.down * i *.7f;

            HingeJoint2D hj = r1.GetComponent<HingeJoint2D>();

            hj.anchor = new Vector2(0, 0.4f);
            hj.connectedAnchor = new Vector2(0, -0.33f);
            hj.connectedBody = previousRigidBody;

            previousRigidBody = r1.GetComponent<Rigidbody2D>();
        }

        GameObject sk = Instantiate(slipKnot, transform);
        sk.transform.position = transform.position + Vector3.down * length * .7f;

        HingeJoint2D skhj = sk.GetComponent<HingeJoint2D>();

        skhj.anchor = new Vector2(-.15f, 0.88f);
        skhj.connectedAnchor = new Vector2(0, -0.33f);
        skhj.connectedBody = previousRigidBody;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
