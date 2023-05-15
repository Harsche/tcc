using Cinemachine;
using UnityEngine;

public class CameraBounds : MonoBehaviour{
    private void Start(){
        GameObject virtualCameraObject = Player.Instance.PlayerVirtualCamera.gameObject;
        var confiner2D = virtualCameraObject.GetComponent<CinemachineConfiner2D>();
        confiner2D.m_BoundingShape2D = GetComponent<CompositeCollider2D>();
    }
}