using System;
using Pool;
using UnityEngine;

[RequireComponent( typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    [SerializeField] private string parentTag;
    [SerializeField] private float liveTimeInSecond = 3;
    public float damage; 

    private float _liveTime;
    
    private int _layerNumber = 8;
    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _liveTime = liveTimeInSecond;
        _rigidbody.velocity = transform.forward * speed;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        TimeCalculate();
    }

    private void TimeCalculate()
    {
        _liveTime -= Time.fixedDeltaTime;

        if (_liveTime <= 0)
        {
            ExitAction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckActionCondition(other.gameObject)) TriggerAction(other.gameObject);
        if (!other.CompareTag(parentTag)) ExitAction();
    }

    private bool CheckActionCondition(GameObject other)
    {
        return _layerNumber.Equals(other.layer) && !other.CompareTag(parentTag);
    }

    private void TriggerAction(GameObject enemyObject)
    {
        enemyObject.GetComponent<IDamage>()?.SetDamage(damage);
    }

    private void ExitAction()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.SetElementByTag(tag, gameObject);
    }
}
