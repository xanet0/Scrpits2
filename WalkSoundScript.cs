using UnityEngine;

public class WalkSoundScript : MonoBehaviour
{
    public AudioClip[] StepSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void onStep()
    {
        audioSource.PlayOneShot(StepSound[Random.Range(0, StepSound.Length)]);
    }
}
