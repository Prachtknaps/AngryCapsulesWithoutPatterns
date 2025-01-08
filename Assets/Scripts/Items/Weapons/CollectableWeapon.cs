using UnityEngine;

public class CollectableWeapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IWeapon weapon = GetComponent<IWeapon>();
            if (weapon != null)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.SetWeapon(weapon);

                    Transform weaponHolder = other.transform.Find("Head/Weapon");
                    if (weaponHolder != null)
                    {
                        foreach (Transform child in weaponHolder)
                        {
                            Destroy(child.gameObject);
                        }

                        transform.SetParent(weaponHolder);
                        transform.localPosition = Vector3.zero;
                        transform.localRotation = Quaternion.identity;
                    }

                    Collider collider = GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }
    }
}
