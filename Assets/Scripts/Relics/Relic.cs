using UnityEngine;

public class Relic : MonoBehaviour
{
    
    public string Name { get; private set; }
    public int SpriteId { get; private set; }
    public RelicTrigger Trigger { get; private set; }
    public RelicEffect Effect { get; private set; }

    public Relic(string name, int spriteId, RelicTrigger trigger, RelicEffect effect)
    {
        Name = name;
        SpriteId = spriteId;
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
    }

    private void OnDamageTaken()
    {
        effect.Apply();
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
        Debug.Log($"Gained {amount} mana from relic.");
    }

}
