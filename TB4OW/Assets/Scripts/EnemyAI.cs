using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// from https://www.youtube.com/watch?v=sWqRfygpl4I

[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private  float activateDistance = 50f;
    [SerializeField] private  float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    [SerializeField] private  float nextWaypointDistance = 3f;
    [SerializeField] private  float jumpNodeHeightRequirement = 0.8f;

    [Header("Custom Behavior")]
    [SerializeField] private  bool followEnabled = true;
    [SerializeField] private  bool jumpEnabled = true;
    [SerializeField] private  PlayerController selfPlayerRef = null;
    [SerializeField] private  GameObject otherPlayerGO = null;
    [SerializeField] private float minDistance = .2f;
    [SerializeField] private float rangedAttackHeightThreshold = .1f;
    [SerializeField] private float itemPickupDistThreshold = .21f;
    [SerializeField] private float attackRangeModifier = .1f;

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

        if(itemPickupDistThreshold < minDistance)
        {
            Debug.LogWarning("ItemPickupDistance is less than MinMoveDistance therefore the AI will never pickup an item because it will stop moving before it gets in range to pick up said item");
        }

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        // TurnTowardsPlayer();

        if(selfPlayerRef.curWeapon == null){
            GameObject nearestWeapon = GetNearestWeapon();

            // there is at least one weapon available to be picked up
            if(nearestWeapon != null){
                target = nearestWeapon.transform;
                if (Vector2.Distance(transform.position, target.transform.position) < itemPickupDistThreshold)
                {
                    //Debug.Log("Picking up weapon");
                    MoveToTarget();
                    selfPlayerRef.WeaponInteract();
                }
                else
                {
                    // Debug.Log(Vector2.Distance(transform.position, target.transform.position));
                    //Debug.Log("Moving to weapon");
                    MoveToTarget();
                }
            }
            else{
                if(otherPlayerRef.curWeapon != null){
                    // run away (not implemented yet)
                }
                else{
                    // shoving match
                    target = otherPlayerRef.aiTargetPos;
                    MoveToTarget();
                }
            }
        }
        else{
            target = otherPlayerRef.aiTargetPos;

            float weaponRange = GetRangeOfWeapon(selfPlayerRef.curWeapon);

            if (selfPlayerRef.curWeapon.label.Equals("fish bucket"))
            {
                if (Mathf.Abs(otherPlayerGO.transform.position.y - transform.position.y) < rangedAttackHeightThreshold)
                {
                    //Debug.Log("Fire");
                    //RangedController tmp = (RangedController)selfPlayerRef.curWeapon;

                    //tmp.UseWeapon();
                    selfPlayerRef.curWeapon.Attack();

                }
                else
                {
                    MoveToTarget();
                }
            }
            else if (Vector2.Distance(transform.position, target.transform.position) < weaponRange)
            {
                //Debug.Log("Attack Player");
                MoveToTarget();
                selfPlayerRef.curWeapon.Attack();
            }
            else
            {
                //Debug.Log("Move To Player");
                MoveToTarget();
            }
    
        }
    }


    private float GetRangeOfWeapon(WeaponController weapon){
        Vector2 size = weapon.spriteRendererRef.bounds.size;
        return size.magnitude + attackRangeModifier;
    }

    private GameObject GetNearestWeapon() {

        GameObject nearestWeapon = null;

        // get all weapons in scene
        GameObject[] sceneWeapons = GameObject.FindGameObjectsWithTag("weapon");
        // Debug.Log(sceneWeapons.Length);
        float minDist = float.MaxValue;

        // iterate through each one
        foreach (GameObject weapon in sceneWeapons)
        {
            // calculate the distance to it
            // Debug.Log("Distance to " + weapon.GetComponent<WeaponController>().label + ": " + Vector2.Distance(weapon.transform.position, transform.position));
            float dist = Vector2.Distance(weapon.transform.position, transform.position);
            if (dist < minDist &&
                // if the player is not holding said weapon
                weapon.GetComponent<WeaponController>() != otherPlayerRef.curWeapon)
            {
                nearestWeapon = weapon;
                minDist = dist;
            }
        }

        return nearestWeapon; 
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
        if (path == null || Vector2.Distance(transform.position, target.transform.position) < minDistance)// || target == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        bool jump = false;
        // Jump
        if (jumpEnabled)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                //Debug.Log("Jump");
                jump = true;
            }
        }

        // Movement
        float moveVal = direction.x * Time.fixedDeltaTime * selfPlayerRef.movementSpeed;
        // Debug.Log(moveVal);
        selfPlayerRef.controller.Move(moveVal, false, jump);
        // TurnTowardsPlayer(moveVal);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // private void TurnTowardsPlayer(float moveVal){
    //     Debug.Log(moveVal);
    //     Debug.Log(transform.localScale.x);
    //     if(moveVal < 0 && transform.localScale.x > 0){
    //         Vector3 theScale = transform.localScale;
	// 	    theScale.x *= -1;
	// 	    transform.localScale = theScale;
    //     }
    //     else if(moveVal > 0 && transform.localScale.x < 0){
    //         Vector3 theScale = transform.localScale;
	// 	    theScale.x *= -1;
	// 	    transform.localScale = theScale;  
    //     }
    // }

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
