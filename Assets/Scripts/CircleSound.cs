using UnityEngine;

public class CircleSound : MonoBehaviour
{
    public AudioClip[] touchSounds;
    private SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene.");
        }
    }

    public AudioClip PlayRandomSound()
    {
        if (soundManager == null)
        {
            Debug.LogError($"{gameObject.name} has no SoundManager component.");
            return null;
        }

        if (touchSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, touchSounds.Length);
            AudioClip clipToPlay = touchSounds[randomIndex];
            if (clipToPlay != null)
            {
                soundManager.PlaySound(clipToPlay);
                return clipToPlay;
            }
            else
            {
                Debug.LogError($"{gameObject.name} has a null audio clip at index {randomIndex}.");
                return null;
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name} has no audio clips assigned.");
            return null;
        }
    }
}
