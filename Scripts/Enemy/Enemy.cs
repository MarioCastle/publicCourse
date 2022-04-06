using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public static Action <Enemy>OnEndReached;
	/// <summary>	
    /// Move speed of our enemy	
    /// </summary>
    [SerializeField] private float moveSpeed = 3f;
    public float MoveSpeed { get; set; }
    
    /// <summary>	
    /// The waypoint reference	
    /// </summary>	
    public Waypoint Waypoint { get; set; }
    
	/// <summary>	
    /// Returns the current Point Position where this enemy needs to go	
    /// </summary>
    public Vector3 CurrentPointPosition => Waypoint.GetWaypointPosition(_currentWaypointIndex);

    private int _currentWaypointIndex;
    private Vector3 _lastPointPosition;	
    
    private EnemyHealth _enemyHealth;
    private SpriteRenderer _spriteRenderer;	
    
    public EnemyHealth EnemyHealth { get; set; }

    private void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHealth = GetComponent<EnemyHealth>();

        _currentWaypointIndex = 0; //1
        MoveSpeed = moveSpeed;
        _lastPointPosition = transform.position;
    }

    private void Update()
    {
        Move();
        Rotate();

        if(CurrentPointPositionReached())
        {
           // _currentWayPointIndex++;
           UpdateCurrentPointIndex();

        }
    }

    public void StopMovement()
    {
        MoveSpeed = 0f;
    }

    public void ResumeMovement()
    {
        MoveSpeed = moveSpeed;
    }

    private void Rotate()	
    {	
        if (CurrentPointPosition.x > _lastPointPosition.x)	
        {	
            _spriteRenderer.flipX = false;	
        }	
        else	
        {	
            _spriteRenderer.flipX = true;	
        }	
    }

    private void Move()
    {
        //Vector3 currentPosition = waypoint.GetWaypointPosition(_currentWayPointIndex);
        transform.position = Vector3.MoveTowards(transform.position, 
                    CurrentPointPosition, MoveSpeed * Time.deltaTime);
        
        
    }

    private bool CurrentPointPositionReached()
    { // go next point position
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if(distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
        
    }

    private void UpdateCurrentPointIndex()	
    {	
        int lastWaypointIndex = Waypoint.Points.Length - 1;	
        if (_currentWaypointIndex < lastWaypointIndex)	
        {	//if we are not in the last waypoint index
            _currentWaypointIndex++;	
        }	
        else // but if we are IN THE Last Position our waypoint
        {     // let return this enemy back to the pool	
            EndPointReached();	
        }	


    }
/// <summary>
///  Before we return this enemy to the pool (ObjectPooler.ReturnPool)
///     
//Simple Way  :   OnEndReached?.Invoke();
// if(OnEndReached !=null)
// {
//     OnEndReached.Invoke();
// }

/// </summary>

    private void EndPointReached()
    {
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void ResetEnemy()
    {
        _currentWaypointIndex = 0;
    }
}

