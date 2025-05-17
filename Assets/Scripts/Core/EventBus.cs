using UnityEngine;
using System;

public class EventBus
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;

    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
        Debug.Log("Did Damage");
    }


    public event Action<Vector3> OnMove;
    public void DoMove(Vector3 newPosition)
    {
        OnMove?.Invoke(newPosition);
        Debug.Log("Did Move");
    }

    public event Action<Spell> OnSpellCast;
    public void DoCastSpell(Spell spell)
    {
        OnSpellCast?.Invoke(spell);
        Debug.Log("Did Cast");
    }

    public event Action<Hittable> OnEnemyKilled;
    public void DoEnemyKilled(Hittable enemy)
    {
        OnEnemyKilled?.Invoke(enemy);
        Debug.Log("Did Enemy");
    }

    public event Action<float> OnUpdate;
    public void DoUpdate(float dt)
    {
        OnUpdate?.Invoke(dt);
        Debug.Log("Did Damage");
    }


}

public class EventBusUpdater : MonoBehaviour
{
    void Update()
    {
        EventBus.Instance.DoUpdate(Time.deltaTime);
    }
}
