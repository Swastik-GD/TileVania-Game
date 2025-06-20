using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinPickupAmount = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (collision.tag == "Player" && !wasCollected)
            {
                wasCollected = true;
                FindObjectOfType<GameSession>().AddScore(coinPickupAmount);
                AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
                Destroy(gameObject);
            }
        }
    }

}
