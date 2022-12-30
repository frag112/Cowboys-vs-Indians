using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Unit : Entity
{
    [Header("Unit Settings")]
    [Tooltip("GameObjects with this tag will be attacked")]
    [SerializeField] protected string filter = "Player";
    [Tooltip("GameObjects on this layer will be attacked")]
    [SerializeField] protected LayerMask mask;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _recoil;
    [Tooltip("set as weapon range if aplicable, otherwise do not touch")]
    [SerializeField] protected float _stopHere = 1f;
    [Header("Components")]
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Animator _animator;

    protected GameObject _target;
    protected bool _walking = false;



    protected void Idle()
    {
        _animator.Play("Base Layer.Idle", 0, 0);
        _agent.isStopped = true;
        StopAllCoroutines();
    }
    protected GameObject SearchForTarget()
    { 
        var possibleTargets = GameObject.FindGameObjectsWithTag(filter);
        if (possibleTargets.Length == 0)
        {
            //Destroy(gameObject);
            Idle();  // check if end round and play sad or happy anim
            return null;
        }
        return possibleTargets[Random.Range(0, possibleTargets.Length)];
    }
    protected void MoveToTarget()
    {
        _agent.isStopped = false;
        StartCoroutine(Walk());
    }
    protected IEnumerator Walk() 
    {
        _walking = true;
        while ((_target != null) && ((int)Vector3.Distance(transform.position, _target.transform.position) > _stopHere))
        {
            _agent.destination = _target.transform.position;
            yield return new WaitForSeconds(1);
        }            
    }

    protected virtual IEnumerator VisionCast(float recoil)
    {
        while (gameObject.activeSelf)
        {
            if (!_target)
            {
                _target = SearchForTarget();
            }
            yield return new WaitForSeconds(recoil);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

    
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * 1f);
        //Gizmos.DrawCube(transform.position, new Vector3(2f, 1f, .5f));
        Gizmos.DrawSphere(transform.position, 1f);

    }*/


    protected override void Die()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.tag = "Deads";
        gameObject.layer = 0;
        StopAllCoroutines();
        _agent.isStopped = true;
        _animator.SetBool("Death", true);
        StartCoroutine(DeathRoutine());
    }
    IEnumerator DeathRoutine()
    {
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
}
