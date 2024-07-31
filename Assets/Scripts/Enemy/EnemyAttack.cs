using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : GameBehaviour
{
    public EnemyPatrol enemyPatrol;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public IEnumerator FishAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Fish Attack");
        enemyPatrol.ChangeSpeed(20);
        yield return new WaitForSeconds(1);
        //PlayAnimation("Attack");
        enemyPatrol.ChangeSpeed(0);
        yield return new WaitForSeconds(3);
        enemyPatrol.ChangeSpeed(enemyPatrol.mySpeed);
        enemyPatrol.myPatrol = PatrolType.Chase;
    }

    public IEnumerator FrogAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Frog Attack");
        enemyPatrol.ChangeSpeed(0);
        //PlayAnimation("Attack");
        yield return new WaitForSeconds(3);
        enemyPatrol.ChangeSpeed(enemyPatrol.mySpeed);
        enemyPatrol.myPatrol = PatrolType.Chase;
    }

    public IEnumerator SpiderAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Spider Attack");
        enemyPatrol.ChangeSpeed(0);
        //PlayAnimation("Attack");
        yield return new WaitForSeconds(3);
        enemyPatrol.ChangeSpeed(enemyPatrol.mySpeed);
        enemyPatrol.myPatrol = PatrolType.Detect;
    }
}
