using Unity.AI.Navigation.LowLevel;
using Unity.Cinemachine;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D rigidbody;
    PlayerMove player;
    float xSpeed;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerMove>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
