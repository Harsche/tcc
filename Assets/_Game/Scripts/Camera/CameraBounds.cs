using System;
using Cinemachine;
using UnityEngine;

public class CameraBounds : MonoBehaviour{
    [SerializeField] private bool showBoundsAsGizmos;
    [SerializeField] private BoxCollider2D boxCollider2D;

    private void Start(){
        GameObject virtualCameraObject = Player.Instance.PlayerVirtualCamera.gameObject;
        var confiner2D = virtualCameraObject.GetComponent<CinemachineConfiner2D>();
        confiner2D.m_BoundingShape2D = GetComponent<CompositeCollider2D>();
    }

    private void OnDrawGizmos(){
        if (!showBoundsAsGizmos){ return; }
        Gizmos.color = Color.yellow * new Vector4(1f, 1f, 1f, 0.1f);
        Vector3 position = (Vector2) transform.position + boxCollider2D.offset;
        Vector3 size = boxCollider2D.size;
        size.z = 0.01f;
        Gizmos.DrawCube(position, size);
    }

#if UNITY_EDITOR
    private void OnValidate(){
        if (!boxCollider2D){ boxCollider2D = GetComponent<BoxCollider2D>(); }
    }
#endif
}