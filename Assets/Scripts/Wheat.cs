using UnityEngine;

public class Wheat : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError(" Animator НЕ найден на объекте: " + gameObject.name);
        }
        else
        {
            Debug.Log(" Animator найден у объекта: " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(" OnTriggerEnter2D: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log(" Игрок коснулся пшеницы — включаю isTouched = true");
            if (anim != null) anim.SetBool("isTouched", true);
        }
        else
        {
            Debug.Log(" Но это НЕ игрок, тег: " + other.tag);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(" OnTriggerStay2D: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log(" Игрок продолжает касаться — isTouched = true");
            if (anim != null) anim.SetBool("isTouched", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(" OnTriggerExit2D: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log(" Игрок ушёл — isTouched = false");
            if (anim != null) anim.SetBool("isTouched", false);
        }
    }
}
