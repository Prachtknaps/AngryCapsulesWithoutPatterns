using UnityEngine;

public class Balloon : MonoBehaviour
{
    private GameManager gameManager = null;
    private new Rigidbody rigidbody = null;
    private float lifetime = 5.0f;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * 10.0f, ForceMode.Impulse);
        Destroy(gameObject, lifetime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyDamage(20.0f);
                gameManager.AddPoints(20);
            }

            Destroy(gameObject);
        }
    }
}
