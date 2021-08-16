using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePick : MonoBehaviour
{
    GameObject player;
    public float speed = 10f;
    public Rigidbody2D rb;
    Vector3 moveDir = Vector3.zero;

    public float knockbackX = 1f;
    public float knockbackY = 2f;
    public float knockbackDuration = 0.5f;
    public int damage = 20;

    public int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            transform.right = player.transform.position - transform.position;
            moveDir = player.transform.position - transform.position;
            moveDir = moveDir.normalized;

            if (moveDir.x < 0)
            {
                direction = -1;
            }
        }

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveDir * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //apply X knockback
            other.GetComponent<PlayerInput>().modX = knockbackX * direction;
            other.GetComponent<PlayerInput>().modTimer = knockbackDuration;

            other.GetComponent<PlayerHealth>().takeDamage(damage);

            //apply Y knockback
            other.attachedRigidbody.velocity = new Vector2(other.attachedRigidbody.velocity.x, knockbackY);
        }
    }
}
