using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter (Collider hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }

}
