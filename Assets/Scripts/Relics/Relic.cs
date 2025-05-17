using UnityEngine;

public class Relic : MonoBehaviour
{

    public string Name { get; private set; }
    public int SpriteId { get; private set; }
    public RelicTrigger Trigger { get; private set; }
    public RelicEffect Effect { get; private set; }

    public Relic(string name, int sprite, RelicTrigger trigger, RelicEffect effect)
    {
        Name = name;
        SpriteId = sprite;
        Trigger = trigger;
        Effect = effect;

        Trigger.Initialize(Effect);
        Trigger.Register();
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
}

public abstract class RelicEffect
{
    public abstract void Apply();
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
        if (target.CompareTag("Player")){
            effect.Apply();
        }
        
    }
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
        //add mana
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.AddMana(this.amount);
        Debug.Log($"Gained {amount} mana from relic.");
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
        //casting as diff class
        var spellPower = effect as GainSpellPowerEffect;
        if (spellPower != null)
        {
            spellPower.RemoveBuff();
        }
    }
}

public class GainSpellPowerEffect : RelicEffect
{
    private int baseAmount;
    private int perWave;

    public GainSpellPowerEffect(int baseAmount, int perWave)
    {
        this.baseAmount = baseAmount;
        this.perWave = perWave;
    }

    public override void Apply()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spell_power += baseAmount;
        // apply the spell‐power buff here
    }

    public void RemoveBuff()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spell_power -= baseAmount;
        // undo the spell‐power buff here
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
        // Reset the timer any time the player moves
        EventBus.Instance.OnMove += OnPlayerMove;
        // Hook into our per-frame tick so we can accumulate time
        EventBus.Instance.OnUpdate += OnUpdate;
    }

    private void OnPlayerMove(Vector3 newPos)
    {
        // whenever the player moves, zero out the timer
        elapsed = 0f;
    }

    private void OnUpdate(float deltaTime)
    {
        // only count time if the player hasn't moved recently
        elapsed += deltaTime;
        if (elapsed >= requiredSeconds)
        {
            effect.Apply();
            // fire only once until the next move
            elapsed = 0f;
        }
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
        // assume your GainSpellPowerEffect has a RemoveBuff() method
        var spellpower = effect as GainSpellPowerEffect;
        if (spellpower != null)
        {
            spellpower.RemoveBuff();
        }
    }
}
