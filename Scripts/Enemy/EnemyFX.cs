using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    /// <summary>
    /// this class is to respond that new event is by getting an instance of an animated text
    /// which is inside the Pooler in our DamageTextManager
    /// Once we have that instance here , to set the damage value in our text
    /// spawn a new animated text 
    /// </summary>
    [SerializeField] private Transform textDamageSpawnPosition;

    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void EnemyHit(Enemy enemy, float damage)
    {
        if (_enemy == enemy)
        {
            GameObject newInstance = DamageTextManager.Instance.Pooler.GetInstanceFromPool();
            TextMeshProUGUI damageText = newInstance.GetComponent<DamageText>().DmgText;
            damageText.text = damage.ToString();
            
            newInstance.transform.SetParent(textDamageSpawnPosition);
            newInstance.transform.position = textDamageSpawnPosition.position;
            newInstance.SetActive(true);
        }
    }
    
    private void OnEnable()
    {
        Projectile.OnEnemyHit += EnemyHit;
    }

    private void OnDisable()
    {
        Projectile.OnEnemyHit -= EnemyHit;
    }
}
