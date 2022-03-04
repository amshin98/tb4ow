using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// from https://www.youtube.com/watch?v=sWqRfygpl4I

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private  float activateDistance = 50f;
    [SerializeField] private  float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    [SerializeField] private  float nextWaypointDistance = 3f;
    [SerializeField] private  float jumpNodeHeightRequirement = 0.8f;
    [SerializeField] private  float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    [SerializeField] private  bool followEnabled = true;
    [SerializeField] private  bool jumpEnabled = true;
    [SerializeField] private  bool directionLookEnabled = true;
    [SerializeField] private  PlayerController selfPlayerRef = null;
    [SerializeField] private  GameObject otherPlayerGO = null;
    [SerializeField] private  List<GameObject> sceneWeapons = new List<GameObject>();
    [SerializeField] private float minDistance = .2f;

    private Path path;
    private int currentWaypoint = 0;
    private RaycastHit2D isGrounded;
    private Seeker seeker;
    private Rigidbody2D rb;
    private PlayerController otherPlayerRef;

    public void Start()
    {
        if (selfPlayerRef == null)
            selfPlayerRef = GetComponent<PlayerController>();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        selfPlayerRef.isAI = true;
        otherPlayerRef = otherPlayerGO.GetComponent<PlayerController>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        // use max dimension of current weapon to calculate range of attacks

        if(selfPlayerRef.curWeapon == null){
            int weaponsInUse = 0;

            if(otherPlayerRef.curWeapon != null){
                weaponsInUse += 1;
            }

            // there is at least one weapon available to be picked up
            if(sceneWeapons.Count > weaponsInUse){
                GameObject nearestWeapon = GetNearestWeapon();
                target = nearestWeapon.transform;
                MoveToTarget();
            }
            else{
                if(otherPlayerRef.curWeapon != null){
                    // run away
                }
                else{
                    // shoving match
                    target = otherPlayerGO.transform;
                    MoveToTarget();
                }
            }
        }
        else{
            target = otherPlayerGO.transform;
            float weaponRange = GetRangeOfWeapon(selfPlayerRef.curWeapon);

            // if in range, attack
            // if not in range, move towards target
        }
    }

    private GameObject GetNearestWeapon(){
        return null;
    }

    private float GetRangeOfWeapon(WeaponController weapon){
        return 0;
    }   

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void MoveToTarget()
    {
        if (path == null || Vector2.Distance(transform.position, target.transform.position) < minDistance)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        bool jump = false;
        // Jump
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                jump = true;
            }
        }

        // Movement
        selfPlayerRef.MovePlayer(direction.normalized.x, jump);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
