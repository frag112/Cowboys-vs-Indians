using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMelee : Unit
{
    [SerializeField] LayerMask mask;
    void Start()
    {
       _target = SearchForTarget();
        MoveToTarget();
        StartCoroutine(Attack());
    }
    private IEnumerator Attack()
    {
        while (!_attacking)
        {
            VisionCast();
            yield return new WaitForSeconds(2f);
        }

    }
    void Update()
    {
        if (_target == null)
        {
            _target = SearchForTarget();
            return;
        }
  /*      if (!_attacking)
        {
            MoveToTarget();
        }*/
    }
    protected override void  VisionCast()
    {
        var targetsCanAttack = Physics.BoxCastAll(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1f), new Vector3(.2f, .3f, 1f), transform.forward, Quaternion.identity, 1f, mask);
        // select random from all colliders and start hitting courotine. hit it til it is dead. ONLY THEN cast another vision, and hit first target

        foreach (var target in targetsCanAttack)
        {
            if (target.transform.CompareTag(filter) && target.transform.GetComponent<Entity>())
            {
                _attacking = true;
                _animator.SetBool("Attack", true);
                _agent.isStopped = true;
                StopCoroutine(Walk());
                target.transform.GetComponent<Entity>().TakeDamage(_damage);
                Debug.Log(target.transform.name + " taking hit - " + _damage);
            }
        }
        _attacking = false;
    }
}
