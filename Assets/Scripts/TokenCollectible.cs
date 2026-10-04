using UnityEngine;

public class TokenCollectible : MonoBehaviour
{
    [SerializeField] float bobSpeed = 2f;
    [SerializeField] float bobHeight = 0.5f;

    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startingPosition + Vector3.up * yOffset;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponentInParent<PlayerManager>();

        if (player != null && player.AddToken())
        {
            PlayerAudio playerAudio = player.GetComponent<PlayerAudio>();

            if (playerAudio != null)
            {
                playerAudio.PlayTokenCollected();
            }

            Destroy(gameObject);
        }
    }
}
