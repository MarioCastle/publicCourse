using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static Action <Enemy>OnEndReached;

    [SerializeField] private float moveSpeed = 3f;
    public float MoveSpeed { get; set; }
    
    // [SerializeField] private Waypoint waypoint;
    public Waypoint Waypoint{get;set;}
    
    /// <summary>
    /// Return the Current Point Position where this Enemy Needs to go
    /// </summary>
    /// <returns></returns>
    public Vector3 CurrentPointPosition => Waypoint.GetWaypointPosition(_currentWayPointIndex);

    private int _currentWayPointIndex;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        MoveSpeed = moveSpeed;
        _currentWayPointIndex = 0; //1
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        Move();
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
            return true;
        }
        return false;
        
    }
    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = Waypoint.Points.Length -1;
        if(_currentWayPointIndex <lastWaypointIndex)
        {  //if we are not in the last waypoint index
            _currentWayPointIndex++;
        }else // but if we are IN THE Last Position our waypoint
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
    { // return Enemy back to Pool
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void ResetEnemy()
    {
        _currentWayPointIndex = 0;
    }

    


}

