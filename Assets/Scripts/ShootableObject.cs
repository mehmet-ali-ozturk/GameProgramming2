using UnityEngine;

public class ShootableObject : MonoBehaviour, IShootable
{
    [SerializeField] TokenCollectible tokenPrefab;
    [SerializeField] Transform tokenSpawnPoint;
    [SerializeField] AudioClip shotSound;
    [SerializeField] float soundVolume = 1f;

    bool wasShot;

    public void OnShot()
    {
        if (wasShot)
        {
            return;
        }

        wasShot = true;

        Vector3 spawnPosition = tokenSpawnPoint != null
            ? tokenSpawnPoint.position
            : transform.position;

        Quaternion spawnRotation = tokenSpawnPoint != null
            ? tokenSpawnPoint.rotation
            : tokenPrefab != null ? tokenPrefab.transform.rotation : Quaternion.identity;

        if (tokenPrefab != null)
        {
            Instantiate(tokenPrefab, spawnPosition, spawnRotation);
        }

        if (shotSound != null)
        {
            AudioSource.PlayClipAtPoint(shotSound, spawnPosition, soundVolume);
        }

        Destroy(gameObject);
    }
}
