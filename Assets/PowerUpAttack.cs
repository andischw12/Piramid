using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAttack : PowerUp
{
     
    // Start is called before the first frame update
    public override IEnumerator UseProcess(float powerUpVal)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        EnemyControl[] enemyArr = FindObjectsOfType<EnemyControl>();
        foreach (EnemyControl E in enemyArr) 
        {
            if (Vector3.Distance(Player.transform.position, E.transform.position) < 5) 
            {
                E.TakeDamage((int)powerUpVal);
            }
        }
        yield return null;
    }
}
