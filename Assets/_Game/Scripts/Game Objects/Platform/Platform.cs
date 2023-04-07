using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour{
    [SerializeField] private bool move;
    [SerializeField] private bool stopBetweenPoints;
    [SerializeField] private bool stopAtEdges;
    [SerializeField] private float stopTime = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private PlatformType platformType;
    [SerializeField] private List<Vector3> waypoints = new();
    [SerializeField] private ContactFilter2D playerContactFilter;
    [SerializeField] private Rigidbody2D myRigidbody2D;

#if UNITY_EDITOR
    public bool showHandles = true;
#endif
    private readonly Collider2D[] colliders = new Collider2D[1];
    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private bool reverse;
    private Coroutine moveCoroutine;

    private PlayerMovement playerMovement;

    [field: SerializeField] public int Number{ get; private set; }

    public PlatformType PlatformType => platformType;
    public List<Vector3> Waypoints => waypoints;

    private void Awake(){
        myRigidbody2D = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        if (!move){ return; }
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    private void Start(){
        playerMovement = Player.Instance.GetComponent<PlayerMovement>();
    }

    private void FixedUpdate(){
        currentPosition = transform.position;
        if (CheckPlayer()){
            Vector2 direction = (currentPosition - lastPosition) / Time.fixedDeltaTime;
        }
        lastPosition = currentPosition;
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (!col.gameObject.CompareTag("Player") || !CheckPlayer()){ return; }
        col.transform.SetParent(transform);
        playerMovement.onPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D col){
        if (!col.gameObject.CompareTag("Player")){ return; }
        col.transform.SetParent(null);
        playerMovement.onPlatform = false;
    }

    private void OnValidate(){
        if (waypoints.Count == 0){
            waypoints.Add(transform.position);
            return;
        }
        waypoints[0] = transform.position;
    }

    private bool CheckPlayer(){
        int collisions = myRigidbody2D.GetContacts(playerContactFilter, colliders);
        return collisions > 0;
    }

    private IEnumerator MoveCoroutine(){
        var path = new List<Vector3>(waypoints);
        while (true){
            for (int index = 0; index < path.Count; index++){
                Vector3 waypoint = path[index];
                Tweener tweener = transform.DOMove(waypoint, maxSpeed)
                    .SetSpeedBased()
                    .SetEase(ease)
                    .SetLink(gameObject);

                yield return tweener.WaitForCompletion();
                path.Clear();
                path.AddRange(waypoints);
                if (reverse){ path.Reverse(); }
            }

            reverse = !reverse;
            path.Reverse();
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void AddWaypoint(EventBase eventBase){
        if (waypoints.Count < 2){
            waypoints.Add(transform.position + (Vector3) Vector2.right * 1f);
            return;
        }
        waypoints.Add(waypoints[^1] + (Vector3) Vector2.right * 1f);
    }

    public void RemoveWaypoint(EventBase eventBase){
        if (waypoints.Count > 1){ waypoints.Remove(waypoints[^1]); }
    }
}