using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipTrigger : MonoBehaviour
{
    public string tip;
    Animator anim;
    Text tipText;

    private void Awake()
    {
        GameObject tipGUI = GameObject.Find("Tip GUI");
        anim = tipGUI.GetComponent<Animator>();
        tipText = tipGUI.transform.Find("Tip").GetComponent<Text>();

        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tipText.text = tip;
            anim.SetTrigger("show");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("hide");
        }
    }
}
