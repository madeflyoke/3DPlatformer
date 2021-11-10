using UnityEngine;

public class Orc : Enemy
{
    protected override void Attack()
    {
        if (Time.time>prevTime+attackRate)
        {
            animator.SetTrigger("Attack");
            prevTime = Time.time;
        }      
    }
    protected override void Hit()//animator controller
    {
        if ((player.transform.position - transform.position).magnitude<=attackRange)
        {
            player.GetDamage(damage);
            audioSource.PlayOneShot(attackSFX);
        }     
    }
    protected override void CheckDistanceToPlayer()
    {
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        if (distanceToPlayer.magnitude<=attackRange)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                          Quaternion.LookRotation(distanceToPlayer),
                                                      Time.deltaTime * 500);
            Attack();        
        }
    }

}
