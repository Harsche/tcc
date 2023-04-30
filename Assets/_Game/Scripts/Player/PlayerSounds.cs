using UnityEngine;

public class PlayerSounds : MonoBehaviour{
    [SerializeField] private AudioClip[] grassSteps;

    private AudioSource audioSource;

    private void Start(){
        audioSource = GetComponent<AudioSource>();
        Player.Instance.PlayerAnimation.OnStep += PlayStepSound;
    }

    private void PlayStepSound(){
        int sound = Random.Range(0, grassSteps.Length);
        audioSource.PlayOneShot(grassSteps[sound]);
    }
}