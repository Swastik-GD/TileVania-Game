using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D enemyRigidbody2D;

    void Start()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyRigidbody2D.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFace();
    }

    void FlipEnemyFace()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody2D.linearVelocity.x)), 1f);
    }
}
