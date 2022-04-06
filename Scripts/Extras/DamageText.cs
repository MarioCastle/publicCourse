using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI DmgText => GetComponent<TextMeshProUGUI>();

    public void ReturnTextPool()
    {//it follows the enemy position 
        transform.SetParent(null);
        //if Parent = null it will return back to Pool
        ObjectPooler.ReturnToPool(gameObject);
    }
}
