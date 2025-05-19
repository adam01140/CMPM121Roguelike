using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
public class Relic
{

    public string Name { get; private set; }
    public int SpriteId { get; private set; }
    public RelicTrigger Trigger { get; private set; }
    public RelicEffect Effect { get; private set; }

    public RelicTrigger Until { get; private set; }


    public Relic(string name, int sprite, RelicTrigger trigger, RelicEffect effect, RelicTrigger until)
    {
        Name = name;
        SpriteId = sprite;
        Trigger = trigger;
        Effect = effect;
        Until = until;


    }

    public Relic(Relic other)
    {
        Name = other.Name;
        SpriteId = other.SpriteId;
        Trigger = other.Trigger?.Clone();
        Effect = other.Effect?.Clone();
        Until = other.Until?.Clone();

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

    public virtual void Remove()
    {

    }
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
    private int amountApplied;
    private bool applied;
    private int casts;
    private RPN rpn;

    public GainSpellPowerEffect(string amountString)
    {
        this.amountString = amountString;
        Dictionary<string, int> tempDict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.wave }
        };
        rpn = new RPN(tempDict);
        applied = false;
        casts = 0;
    }

    public override void Apply()
    {

        Dictionary<string, int> tempDict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.wave }
        };
        rpn.updateRPNVars(tempDict);
        if (applied == false)
        {
            Debug.Log("Spellpower Applied " + amountString);
            GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spell_power += rpn.RPN_to_int(amountString);
            amountApplied = rpn.RPN_to_int(amountString);
            applied = true;
            casts = 1;
        }

    }

    public override void Remove()
    {

        if (applied == true && casts <= 0)
        {
            Debug.Log("Spellpower Removed");
            GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spell_power -= amountApplied;
            applied = false;
        }
        else if (applied == true)
        {
            casts -= 1;
        }

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
        if (target.team == Hittable.Team.PLAYER)
        {
            effect.Apply();
        }
    }

    public override RelicTrigger Clone()
    {
        return new TakeDamageTrigger();
    }
}





public class OnCastSpellTrigger : RelicTrigger
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
        return new OnCastSpellTrigger();
    }
}





public class OnCastSpellTriggerUntil : RelicTrigger
{
    public override void Register()
    {
        EventBus.Instance.OnSpellCast += OnSpellCast;
    }

    private void OnSpellCast(Spell spell)
    {
        effect.Remove();
    }

    public override RelicTrigger Clone()
    {
        return new OnCastSpellTriggerUntil();
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

    private bool active;

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
        active = false;
        elapsed = 0f;

    }

    private void OnUpdate(float deltaTime)
    {
        elapsed += deltaTime;
        if (elapsed >= requiredSeconds && active == false)
        {
            effect.Apply();
            elapsed = 0f;
            active = true;
        }
    }

    public override RelicTrigger Clone()
    {
        return new StandStillTrigger(this.requiredSeconds);
    }
}





public class OnMoveTriggerUntil : RelicTrigger //Specifically a "Until" trigger so the relic effect can be properly removed or updated
{
    public override void Register()
    {
        EventBus.Instance.OnMove += OnMove;
    }

    private void OnMove(Vector3 vec)
    {
        effect.Remove();
    }

    public override RelicTrigger Clone()
    {
        return new OnMoveTriggerUntil();
    }
}


