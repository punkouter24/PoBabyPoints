using UnityEngine;
using System.Collections;

public class CircleInteraction : MonoBehaviour
{
    private GameManager gameManager;
    private CircleSound circleSound;
    private SpriteRenderer spriteRenderer;
    private bool isTouched = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        circleSound = GetComponent<CircleSound>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
        if (circleSound == null)
        {
            Debug.LogError("CircleSound component not found on the circle!");
        }
    }

    void OnMouseDown()
    {
        if (!isTouched && gameManager.IsGameStarted())
        {
            isTouched = true;
            gameManager.CircleTouched(GetComponent<CircleMovement>());
            if (circleSound != null)
            {
                AudioClip clip = circleSound.PlayRandomSound();
                StartCoroutine(FadeAndGrow(clip != null ? clip.length : 0));
            }
            else
            {
                StartCoroutine(FadeAndGrow(0));
            }
            Debug.Log($"{gameObject.name} was touched.");
        }
    }

    private IEnumerator FadeAndGrow(float delay)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            float scale = Mathf.Lerp(1f, 1.2f, elapsed / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
