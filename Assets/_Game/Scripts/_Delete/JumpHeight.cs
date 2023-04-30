using System.Collections;
using UnityEngine;

public class JumpHeight : MonoBehaviour{
    private float maxHeight;
    private PlayerMovement playerMovement;
    private float startHeight;


    private void Start(){
        // myRigidbody2D = GetComponent<Rigidbody2D>();
        playerMovement = Player.Instance.GetComponent<PlayerMovement>();
        
        PlayerInput.OnJumpInput += value => {
            if (!value || !PlayerMovement.Grounded) return;
            startHeight = playerMovement.transform.position.y;
            maxHeight = startHeight;
            StartCoroutine(CheckJumpHeight());
        };
    }

    private IEnumerator CheckJumpHeight(){
        yield return new WaitForSeconds(1f / 3f);
        while (!PlayerMovement.Grounded){
            if (playerMovement.transform.position.y > maxHeight){ maxHeight = playerMovement.transform.position.y; }
            yield return null;
        }
        Debug.Log($"JUMP HEIGHT: {maxHeight - startHeight}");
    }
}