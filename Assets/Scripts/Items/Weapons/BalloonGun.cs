using UnityEngine;

public class BalloonGun : MonoBehaviour, IWeapon
{
    [Header("Options")]
    [SerializeField] private float cooldown = 0.5f;
    private float lastShootTime = 0.0f;

    [Header("Components")]
    [SerializeField] private GameObject balloonSpawnPoint = null;
    [SerializeField] private GameObject balloon = null;

    private AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (Time.time - lastShootTime >= cooldown)
        {
            lastShootTime = Time.time;

            GameObject balloonInstance = Object.Instantiate(balloon, balloonSpawnPoint.transform.position, balloonSpawnPoint.transform.rotation);
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            Balloon balloonScript = balloonInstance.GetComponent<Balloon>();
            if (balloonScript != null)
            {
                balloonScript.Initialize(gameManager);
                audioSource.Play();
            }
        }
    }
}
