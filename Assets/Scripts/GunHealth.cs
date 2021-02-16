using UnityEngine;
using UnityEngine.Events;

public class GunHealth : MonoBehaviour, IDamage
{
    [SerializeField] private float health;
    [SerializeField] private UnityEvent dead;

    [SerializeField] private float _health1;

    private void OnEnable()
    {
        _health1 = health;
    }

    public void SetDamage(float damage)
    {
        _health1 = damage > _health1 ? 0 : _health1 - damage;
    }

    private void Update()
    {
        if (_health1 == 0) dead.Invoke();
    }
}
