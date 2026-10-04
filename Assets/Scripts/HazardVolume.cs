using UnityEngine;

public class HazardVolume : MonoBehaviour
{
    [SerializeField] bool instantKill;
    [SerializeField] float damagePerSecond = 20f;

    void OnTriggerEnter(Collider other)
    {
        if (!instantKill)
        {
            return;
        }

        PlayerManager player = other.GetComponentInParent<PlayerManager>();

        if (player != null)
        {
            player.Kill();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (instantKill)
        {
            return;
        }

        PlayerManager player = other.GetComponentInParent<PlayerManager>();

        if (player != null)
        {
            player.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
        }
    }
}