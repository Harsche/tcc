using JSAM;
using UnityEngine;

public class Configurations : MonoBehaviour
{
    public void UpdateMusicVolume(float value){
        AudioManager.SetMusicVolume(value);
    }
    
    public void UpdateSoundVolume(float value){
        AudioManager.SetSoundVolume(value);
    }
}   
