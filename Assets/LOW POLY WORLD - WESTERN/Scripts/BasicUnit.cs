using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]//
public class BasicUnit : MonoBehaviour
{
    [SerializeField] protected string filter;//
    [SerializeField] protected float _damage;//
    [SerializeField] private float _health;//
    protected GameObject _target;

    protected Vector3 _destination;
    [SerializeField] protected Animator _animator;//
    protected NavMeshAgent _agent;//

    private void GetTargets()
    {
        var possibleTargets = GameObject.FindGameObjectsWithTag(filter);
        if (possibleTargets.Length == 0)
        {
            Destroy(gameObject);
            return;
        }
        var target = possibleTargets[Random.Range(0, possibleTargets.Length)];
        _target = target;//.GetComponent<Targetable>();
        _agent.destination = _target.transform.position;
    }
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
         _destination = _agent.destination;

        GetTargets();


    }
    private void OnTriggerEnter(Collider other)
    {
        //var targetHit = other.gameObject.GetComponent<Targetable>();
        if (other.tag==filter)
        {
            _animator.SetBool("Attack", true);
            transform.LookAt(other.transform);
            _agent.enabled = false;
            //Hit(targetHit);
            Hit(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == filter)
        {
            _animator.SetBool("Attack", false);
            _agent.enabled = true;
            GetTargets();
        }
    }
    protected void Hit(GameObject targetObject)
    {
        if (targetObject.GetComponent<Targetable>())
        {
            targetObject.GetComponent<Targetable>().TakeDamage(_damage);
            return;
        }
        if (targetObject.GetComponent<BasicUnit>())
        {
            targetObject.GetComponent<BasicUnit>().TakeDamage(_damage);
            return;
        }

    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _animator.SetBool("Death", true);
            _agent.enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(Death());
            return;
        }

    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (_target.transform.position != null)//_agent.enabled == true
        {
            if (Vector3.Distance(transform.position, _target.transform.position) > 1.0f)
            {
                 _destination = _target.transform.position;
                _agent.destination = _destination;
                _agent.destination = _target.transform.position;
            }
        }
        else
        {
            GetTargets();
        }
    }
}
