using UnityEngine;

public class PlayerSounds : MonoBehaviour{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private AudioClip[] grassSteps;

    private AudioSource audioSource;

    private void Awake(){
        audioSource = GetComponent<AudioSource>();
        playerAnimation.OnStep += PlayStepSound;
    }

    private void PlayStepSound(){
        int sound = Random.Range(0, grassSteps.Length);
        audioSource.PlayOneShot(grassSteps[sound]);
    }
}