using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMelee : Unit
{
    void OnEnable()
    {
        StartCoroutine(VisionCast(_recoil));

    }
    protected  override IEnumerator  VisionCast ( float recoil)
    {
        //_watching = true;
        while (gameObject.activeSelf)
        {
            if (!_target)
            {
                _target = SearchForTarget();
            }

            var targetsCanAttack = Physics.BoxCastAll(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1f), new Vector3(.2f, .3f, 1f), transform.forward, Quaternion.identity, 1f, mask);

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
                _agent.isStopped = true;
                StopCoroutine(Walk());
                _walking = false;
                _animator.SetTrigger("Attack");
                transform.LookAt(validTargets[0].transform);
                validTargets[0].transform.GetComponent<Entity>().TakeDamage(_damage);
                yield return new WaitForSeconds(recoil);

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
