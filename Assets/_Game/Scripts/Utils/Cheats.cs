using UnityEngine;

namespace Utils{
    public class Cheats : MonoBehaviour{
        public void ActivateCheats(){
            Player.Instance.PlayerParry.EnableParry = true;
            Player.Instance.PlayerParry.enableGreen = true;
            Player.Instance.PlayerParry.enableBlue = true;
            Player.Instance.PlayerParry.enableRed = true;
            Player.Instance.PlayerMovement.enableDash = true;
            Player.Instance.PlayerShield.unlocked = true;
            Player.Instance.invulnerable = true;
        }
    }
}