using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    public static Action <Enemy>OnEnemyKilled;
    public static Action <Enemy>OnEnemyHit ;
    

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;

    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private float maxHealth = 10f;

    public float CurrentHealth { get; set; }

    private Image _healthBar;
    private Enemy _enemy;


    void Start()
    {
        CreateHealthBar();
        CurrentHealth = initialHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealDamage(5f);
        }

        _healthBar.fillAmount = Mathf.Lerp(
            _healthBar.fillAmount, CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
            newBar.transform.SetParent(transform);

        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
            _healthBar = container.FillAmountImage;
    }

    public void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;
        if(CurrentHealth <= 0)
        { //When the enemy has Health
            CurrentHealth = 0;
            Die();
        } else
        { //this enemy is not dead ,we're going to fire our own
        //Enemy hit event ,if this is not null, let's invoke it
            OnEnemyHit?.Invoke(_enemy);
        }
    }

    public void ResetHealth()
    { //when the enemy reaches the final position
    // it has received some damage we need to call this
    //method to reset the health of the enemy
        CurrentHealth = initialHealth;
        _healthBar.fillAmount = 1f;
    }

    private void Die()
    {
        // ResetHealth();
        //OnEnemyKilled if this is not now , let's Invoke
        OnEnemyKilled?.Invoke(_enemy);
        // ObjectPooler.ReturnToPool(gameObject);
    }

}
