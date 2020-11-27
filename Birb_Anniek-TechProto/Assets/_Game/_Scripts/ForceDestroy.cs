using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDestroy : MonoBehaviour
{
    public GameObject onDestroyInstantiateObject;
    // The minimum amount of force needed on a 2d rigidbody to make it destroy itselfs
    public float minimumForce = 3f;
    private Rigidbody2D rb2d;
    void Start() 
    {
        if (GetComponent<Rigidbody2D>() != null)
            rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normalImpulse >= minimumForce)
        {
            Instantiate(onDestroyInstantiateObject, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
