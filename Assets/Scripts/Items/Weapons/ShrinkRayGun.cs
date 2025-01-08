using UnityEngine;

public class ShrinkRayGun : MonoBehaviour, IWeapon
{
    [Header("Options")]
    [SerializeField] private float interval = 0.15f;
    private float lastShootTime = 0.0f;

    public void Shoot()
    {
        if (Time.time - lastShootTime >= interval)
        {
            lastShootTime = Time.time;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.ApplyDamage(10.0f);
                        enemy.transform.localScale = enemy.transform.localScale * 0.95f;
                        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                        if (gameManager != null)
                        {
                            gameManager.AddPoints(10);
                        }
                    }
                }
            }
        }
    }
}
