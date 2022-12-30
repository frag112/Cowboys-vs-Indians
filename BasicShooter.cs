using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : Unit
{
    [SerializeField] private float _weaponRange = 25f;
    void OnEnable()
    {
        StartCoroutine(VisionCast(_recoil));
    }

    protected override IEnumerator VisionCast(float recoil)
    {
        while (gameObject.activeSelf)
        {
            if (!_target)
            {
                _target = SearchForTarget();
            }

            var targetsCanAttack = Physics.SphereCastAll(transform.position, _weaponRange, transform.forward, _weaponRange, mask);

            List<RaycastHit> validTargets = new List<RaycastHit>();

            foreach (var target in targetsCanAttack)
            {
                if (target.transform.CompareTag(filter) && target.transform.GetComponent<Entity>())
                {
                    validTargets.Add(target);
                }
            }
            if (validTargets.Count > 0)
            {
                //я хотел чтобы стрелок проверял нет ли препятствия между собой и целью, но почему то из - за этого куска все ломается, поэтому он теперь среляет сквозь стены
                if (Physics.Raycast(transform.position, (validTargets[0].transform.position - transform.position), Vector3.Distance(transform.position, validTargets[0].transform.position)))
                {
                    //Debug.Log("something blocking me");
                    MoveToTarget();
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    _animator.SetTrigger("Attack");
                    _agent.isStopped = true;
                    StopCoroutine(Walk());
                    _walking = false;
                    transform.LookAt(validTargets[0].transform);
                    validTargets[0].transform.GetComponent<Entity>().TakeDamage(_damage);
                    Debug.Log(validTargets[0].transform.name + " taking hit - " + _damage);
                    yield return new WaitForSeconds(recoil);
                    }
            }
            else
            {
                if (!_walking)
                {
                    MoveToTarget();
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
