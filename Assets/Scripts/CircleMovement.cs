using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float speed;
    private Rigidbody2D rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        speed = baseSpeed;

        Debug.Log($"{gameObject.name} initialized.");
    }

    void FixedUpdate()
    {
        if (gameManager.IsGameStarted())
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void ResetPositionAndSpeed(int level)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Set the position to be closer to the middle (e.g., within a range of -2 to 2 units)
        transform.position = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        speed = baseSpeed + level; // Increase speed based on level
        rb.velocity = Random.insideUnitCircle.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && gameManager.IsGameStarted())
        {
            Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            rb.velocity = reflectDirection * speed;

            Debug.Log($"{gameObject.name} bounced off a wall with new velocity {rb.velocity}");
        }
    }
}
