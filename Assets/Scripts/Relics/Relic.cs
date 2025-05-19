using UnityEngine;
using System.Collections.Generic;
public class Relic
{

    public string Name { get; private set; }
    public int SpriteId { get; private set; }
    public RelicTrigger Trigger { get; private set; }
    public RelicEffect Effect { get; private set; }

    public Relic(string name, int sprite, RelicTrigger trigger, RelicEffect effect)
    {
        Debug.Log("wharg?");
        Name = name;
        SpriteId = sprite;
        Trigger = trigger;
        Effect = effect;


    }

    public Relic(Relic other)
    {
        Name = other.Name;
        SpriteId = other.SpriteId;
        Trigger = other.Trigger?.Clone();
        Effect = other.Effect?.Clone();

    }

    /**
    Relic greenGem = new Relic(
        "Green Gem",
        spriteId: 0,
        trigger: new TakeDamageTrigger(),
        effect: new GainManaEffect(5)
    );
    */
}

public abstract class RelicTrigger
{
    protected RelicEffect effect;

    public void Initialize(RelicEffect effect)
    {
        this.effect = effect;
    }

    public abstract void Register(); // Hook into the relevant game event

    public abstract RelicTrigger Clone();
}

public abstract class RelicEffect
{
    public abstract void Apply();
    public abstract RelicEffect Clone();
}

public class GainManaEffect : RelicEffect
{
    private int amount;

    public GainManaEffect(int amount)
    {
        this.amount = amount;
    }

    public override void Apply()
    {
        Debug.Log("Effect Applied");
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.AddMana(this.amount);
        Debug.Log($"Gained {amount} mana from relic.");
    }

    public override RelicEffect Clone()
    {
        return new GainManaEffect(this.amount);
    }
}

public class GainSpellPowerEffect : RelicEffect
{
    private string amountString;
    private Queue<int> amountApplied;
    private RPN rpn;

    public GainSpellPowerEffect(string amountString)
    {
        this.amountString = amountString;
        Dictionary<string, int> tempDict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.wave }
        };
        rpn = new RPN(tempDict);
    }

    public override void Apply()
    {
        Debug.Log("Effect Applied");
        Dictionary<string, int> tempDict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.wave }
        };
        rpn.updateRPNVars(tempDict);
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spell_power += 5;//rpn.RPN_to_int(amountString);
        // amountApplied.Enqueue(rpn.RPN_to_int(amountString));
    }

    public void RemoveBuff()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spell_power -= amountApplied.Dequeue();
    }

    public override RelicEffect Clone()
    {
        return new GainSpellPowerEffect(this.amountString);
    }
}

public class TakeDamageTrigger : RelicTrigger
{
    public override void Register()
    {
        //maybe something like this?
        //EventBus.OnTakeDamage += OnDamageTaken;
        EventBus.Instance.OnDamage += OnDamageTaken;
    }

    private void OnDamageTaken(Vector3 where, Damage dmg, Hittable target)
    {
        effect.Apply();
    }

    public override RelicTrigger Clone()
    {
        return new TakeDamageTrigger();
    }
}

public class CastSpellTrigger : RelicTrigger
{
    public override void Register()
    {
        EventBus.Instance.OnSpellCast += OnSpellCast;
    }

    private void OnSpellCast(Spell spell)
    {
        effect.Apply();
    }

    public override RelicTrigger Clone()
    {
        return new CastSpellTrigger();
    }
}


// var jadeElephant = new Relic(
//     "Jade Elephant",
//     sprite: 1,
//     trigger: new StandStillTrigger(3f),
//     effect: new GainSpellPowerEffect(baseAmount: 10, perWave: 5)
// );

// jadeElephant.Trigger = new CompositeTrigger(
//     new StandStillTrigger(3f),
//     new CancelOnMoveTrigger()
// );
public class CancelOnMoveTrigger : RelicTrigger
{
    public override void Register()
    {
        EventBus.Instance.OnMove += OnPlayerMove;
    }

    private void OnPlayerMove(Vector3 newPos)
    {
        effect.Apply();
    }

    public override RelicTrigger Clone()
    {
        return new CancelOnMoveTrigger();
    }
}

public class OnKillTrigger : RelicTrigger
{
    public override void Register()
    {
        EventBus.Instance.OnEnemyKilled += OnEnemyKill;
    }

    private void OnEnemyKill(Hittable enemy)
    {
        effect.Apply();
    }

    public override RelicTrigger Clone()
    {
        return new OnKillTrigger();
    }
}


public class StandStillTrigger : RelicTrigger
{
    private float requiredSeconds;
    private float elapsed = 0f;

    public StandStillTrigger(float seconds)
    {
        requiredSeconds = seconds;
    }

    public override void Register()
    {
        EventBus.Instance.OnMove += OnPlayerMove;
        EventBus.Instance.OnUpdate += OnUpdate;
    }

    private void OnPlayerMove(Vector3 newPos)
    {
        elapsed = 0f;
    }

    private void OnUpdate(float deltaTime)
    {
        elapsed += deltaTime;
        if (elapsed >= requiredSeconds)
        {
            effect.Apply();
            elapsed = 0f;
        }
    }

    public override RelicTrigger Clone()
    {
        return new StandStillTrigger(this.requiredSeconds);
    }
}


//
// Cancels out spell up there
//
// var goldenMask = new Relic(
//     "Golden Mask",
//     sprite: 2,
//     trigger: new TakeDamageTrigger(),
//     effect: new GainSpellPowerEffect(100)
// );

// // also register the cancel‐on‐cast trigger
// goldenMask.Trigger = new CompositeTrigger(
//     new TakeDamageTrigger(),
//     new CancelOnCastSpellTrigger()
// );
public class CancelOnCastSpellTrigger : RelicTrigger
{
    public override void Register()
    {
        EventBus.Instance.OnSpellCast += OnSpellCast;
    }

    private void OnSpellCast(Spell spell)
    {
        var spellpower = effect as GainSpellPowerEffect;
        if (spellpower != null)
        {
            spellpower.RemoveBuff();
        }
    }

    public override RelicTrigger Clone()
    {
        return new CancelOnCastSpellTrigger();
    }
}
