using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [Header("Unit Settings")]
    [Tooltip("GameObjects with this tag will be attacked")]
    [SerializeField] protected string filter = "Player";
    [SerializeField] float _health = 50f;
    [SerializeField] protected float _damage;
    [Header("Components")]
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Animator _animator;

    protected GameObject _target;
    protected List<RaycastHit> _targetsCanAttack;



    protected void Idle()
    {
        _animator.Play("Base Layer.Idle", 0, 0);
        _agent.isStopped = true;
        StopAllCoroutines();
    }
    protected GameObject SearchForTarget()
    {
        // list in all available targets and pick one  
        var possibleTargets = GameObject.FindGameObjectsWithTag(filter);
        if (possibleTargets.Length == 0)
        {
            //Destroy(gameObject);
            return null;
        }
        return possibleTargets[Random.Range(0, possibleTargets.Length)];
    }
    protected void MoveToTarget()
    {
        // enable navmesh and move to target
        _agent.isStopped = false;
        StartCoroutine(Walk());
    }
    IEnumerator Walk()
    {
        while ((_target.transform.position != null) && (Vector3.Distance(transform.position, _target.transform.position) > 1.0f))
        {
            _agent.destination = _target.transform.position;
            yield return new WaitForSeconds(1);
        }
            SearchForTarget();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

    
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * 1f);
        
    }
    protected void VisionCast()
    {
        // cast ray or sphere or box to see enemies or targets nearby
        // if any go to attack
        _targetsCanAttack = null;
        var targetsCanAttack = Physics.BoxCastAll(transform.forward*.5f, new Vector3(.5f,.5f,.5f), transform.forward, Quaternion.identity, 1f);
        foreach (var target in targetsCanAttack)
        {
            if (target.transform.CompareTag(filter))
            {
                _targetsCanAttack.Add(target);
            }
        }
  
    }
    protected void Attack(List<RaycastHit> enemyUnits)
    {

        // use weapon play animation to deal damage to enemies and targets
        foreach (var enemyUnit in enemyUnits)
        {
            if (enemyUnit.transform.GetComponent<Targetable>())
            {
                enemyUnit.transform.GetComponent<Targetable>().TakeDamage(_damage);
                _animator.SetBool("Attack", true);
                _agent.isStopped = true;
                return;
            }
            if (enemyUnit.transform.GetComponent<Unit>())
            {
                enemyUnit.transform.GetComponent<Unit>().TakeDamage(_damage);
                _animator.SetBool("Attack", true);
                _agent.isStopped = true;
                return;
            }
            // check if it basic melee or shooter or archer and deal damage
        }
    }
    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0) Die();
    }
    protected void Die()
    {
        // disable  collider, play dead
        gameObject.GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        _agent.isStopped = true;
        StartCoroutine(DeathRoutine());
    }
    IEnumerator DeathRoutine()
    {
        // wait for seconds and destroy gameobject
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    protected void EndMatchReact(string reaction)
    {
        // game ended, but this unit is alive, disable navmesh and colliders, play animation
        var animation = "Base Layer." + reaction;
        _agent.isStopped = true;
        _animator.Play(animation, 0, 0);
    }


    //////
    ///service 
    ///
    protected void Init()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponentInChildren<Animator>();
    }
}
