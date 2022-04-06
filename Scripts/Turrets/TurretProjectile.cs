using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] protected float delayBtwAttacks = 2f;

    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected Turret _turret;
    /// <summary>
    /// we load it to our project
    /// </summary>
    private Projectile _currentProjectileLoaded;
    


    private void Start() {
        _turret = GetComponent<Turret>();
        _pooler = GetComponent<ObjectPooler>();

        LoadProjectile();
    }

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            LoadProjectile();
        }

        if(IsTurrentEmpty())
        {
            LoadProjectile();
        }

        if(Time.time > _nextAttackTime)
        {
            // if our Target has enemy & we have a projectile loaded & Enemy is Alive
            if(_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null && 
                                     _turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
            {
            _currentProjectileLoaded.transform.parent = null;
            _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);
            }  
            _nextAttackTime = Time.time + delayBtwAttacks;
        }
        
        
    }
    
    protected virtual void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPosition.position ;
        newInstance.transform.SetParent(projectileSpawnPosition);


        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        _currentProjectileLoaded.TurretOwner = this;
        _currentProjectileLoaded.ResetProjectile();
        newInstance.SetActive(true);

    }
    private bool IsTurrentEmpty()
    {
        return _currentProjectileLoaded == null;
    }

    public void ResetTurretProjectile()
    {
        _currentProjectileLoaded = null ;
    }
}
