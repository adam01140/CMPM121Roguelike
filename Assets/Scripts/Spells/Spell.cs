using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using Unity.Mathematics;
using UnityEditor.SceneManagement;

[Serializable]
public class Spell
{

    public string name;
    public string description;
    public int icon;
    public string N;
    public float spray;
    public DamageTemp damage; //Temp used for conversion to proper damage class
    public Damage damageFull;
    public string mana_cost;
    public float cooldown;
    public Projectile projectile;
    public Projectile secondary_projectile;


    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;
    public RPN rpn;

    public List<Damage> damageOverTimeEvents;

    public List<Tuple<int, float>> slowEvents;
    public float knockback;

    public Spell()
    {
        this.damageOverTimeEvents = new List<Damage>();
        this.slowEvents = new List<Tuple<int, float>>();
        this.knockback = 0.0f;

    }
    public Spell(Spell other)
    {
        this.name = other.name;
        this.description = other.description;
        this.icon = other.icon;
        this.N = other.N;
        this.spray = other.spray;
        this.damage = new DamageTemp(other.damage.amount, other.damage.type);
        this.mana_cost = other.mana_cost;
        this.cooldown = other.cooldown;
        this.projectile = other.projectile;
        this.secondary_projectile = other.secondary_projectile;
        this.team = other.team;
        this.damageOverTimeEvents = new List<Damage>();
        this.slowEvents = new List<Tuple<int, float>>();
        this.knockback = other.knockback;
    }

    public void AssignOwner(SpellCaster owner)
    {
        this.owner = owner;
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["power"] = this.owner.spell_power;
        this.rpn = new RPN(tempDict);
    }

    public void ChangeProjectileTrajectory(string newTrajectory)
    {
        this.projectile.trajectory = newTrajectory;
    }

    // private IEnumerator ChangeTrajectoryAfterDelay(ModifierSpell spell, float delay, string newTrajectory)
    // {
    //     yield return new WaitForSeconds(delay);
    //     spell.ChangeProjectileTrajectory(newTrajectory);
    // }


    public string GetName()
    {
        return this.name;
    }

    public virtual int GetManaCost()
    {
        if (this.rpn != null)
        {
            return this.rpn.RPN_to_int(this.mana_cost);
        }
        else
        {
            return 10;
        }
    }

    public virtual int GetDamage()
    {

        if (damageFull != null)
        {
            return damageFull.amount;
        }
        else
        {
            if (this.rpn != null)
            {

                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                return damageFull.amount;
            }
            else
            {
                return 100;
            }


        }
        //^^
    }
    public virtual Damage GetDamageObj() //For modifying the inner damage object
    {
        if (damageFull != null)
        {
            return damageFull;
        }
        else
        {
            if (this.rpn != null)
            {
                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                return damageFull;
            }
            else
            {
                return null;
            }


        }
        //^^
    }

    public virtual float GetCooldown()
    {
        return this.cooldown;
    }

    public virtual int GetIcon()
    {
        return this.icon;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }

    public virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            if (this.slowEvents.Count > 0)
            {
                var enemyController = other.owner.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    foreach (var slow in this.slowEvents)
                    {
                        enemyController.StartCoroutine(ApplySlow(enemyController, slow.Item1, slow.Item2));
                    }
                }
            }

            Debug.Log("Seen KB: " + this.knockback);
            var rb = other.owner.GetComponent<EnemyController>();
            if (rb != null)
            {
                rb.StartCoroutine(DoKnockback(rb, (int)this.knockback));
            }

            if (this.damageFull != null)
            {
                if (this.damageOverTimeEvents.Count > 0)
                {
                    other.Damage(damageFull, this.damageOverTimeEvents);
                }
                else
                {
                    other.Damage(damageFull);
                }
            }
            else
            {
                this.damageFull = new Damage(this.rpn.RPN_to_int(this.damage.amount), this.damage.type);
                if (this.damageOverTimeEvents.Count > 0)
                {
                    other.Damage(damageFull, this.damageOverTimeEvents);
                }
                else
                {
                    other.Damage(damageFull);
                }

            }

        }

    }
    public IEnumerator ApplySlow(EnemyController enemy, int slowAmount, float slowDuration)
    {
        var unit = enemy.GetComponent<Unit>();
        if (unit != null)
        {
            if (enemy.speed - slowAmount > 0)
            {
                enemy.speed -= slowAmount;
                yield return new WaitForSeconds(slowDuration);
                enemy.speed += slowAmount;
            }
        }
    }
    public IEnumerator DoKnockback(EnemyController enemy, int knockback)
    {
        var unit = enemy.GetComponent<Unit>();
        if (unit != null && knockback > 0)
        {
            int speedBase = enemy.speed;
            enemy.speed = (knockback * -1);
            yield return new WaitForSeconds(0.5f);
            enemy.speed = speedBase;

        }
    }

}

public class ModifierSpell : Spell
{
    private Spell innerSpell;
    private List<ValueModifier> damageModifiers = new List<ValueModifier>();
    private List<ValueModifier> manaModifiers = new List<ValueModifier>();
    private List<ValueModifier> cooldownModifiers = new List<ValueModifier>();
    private List<ValueModifier> speedModifiers = new List<ValueModifier>();



    private int casts;
    private float delay;
    private float splitSpread;

    public float stunDuration = 0f;
    public float vulnerabilityMultiplier = 1f;
    public float missChance = 0f;

    public ModifierSpell(Spell inner)
    {
        this.innerSpell = inner;
        // Copy base properties
        this.name = inner.name;
        this.description = inner.description;
        this.icon = inner.icon;
        this.projectile = inner.projectile;
        this.secondary_projectile = inner.secondary_projectile;
        this.damageFull = inner.damageFull;
        this.casts = 0;
        this.delay = 0.0f;
        this.splitSpread = 0.0f;
        this.damageOverTimeEvents = inner.damageOverTimeEvents;
        this.slowEvents = inner.slowEvents;
        this.knockback = inner.knockback;

    }

    public void AddDamageModifier(ValueModifier mod) => damageModifiers.Add(mod);
    public void AddManaModifier(ValueModifier mod) => manaModifiers.Add(mod);
    public void AddCooldownModifier(ValueModifier mod) => cooldownModifiers.Add(mod);
    public void AddSpeedModifier(ValueModifier mod) => speedModifiers.Add(mod);

    public void AddCasts(int numCasts)
    {
        this.casts += numCasts;
    }
    public void AddMultiCastDelay(float newDelay)
    {
        this.delay += newDelay;
    }
    public void AddMutliCastSpread(float newSpread)
    {
        this.splitSpread += newSpread;
    }

    public void AddDamageOverTimeEffect(DamageTemp damageEvent, int instancesToQue = 5)
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["damage"] = this.GetDamage();
        this.rpn = new RPN(tempDict);
        Damage damageEventTemp = new Damage((int)Math.Floor(this.rpn.RPN_to_float(damageEvent.amount)), damageEvent.type, Damage.EventType.DOT);
        for (int i = 0; i <= instancesToQue; i++)
        {
            this.damageOverTimeEvents.Add(damageEventTemp);
        }

    }

    public void AddSlowEffect(int slowAmmount, float slowDuration)
    {
        this.slowEvents.Add(new Tuple<int, float>(slowAmmount, slowDuration));

    }

    public void AddKnockback(float distance)
    {

        innerSpell.knockback += distance;

    }

    public void AddStunEffect(float duration) => this.stunDuration = duration;
    public void AddVulnerabilityEffect(float multiplier, float duration) => this.vulnerabilityMultiplier = multiplier;
    public void AddMissChanceEffect(float chance, float duration) => this.missChance = chance;



    public override int GetDamage()
    {
        Damage baseObj = innerSpell.GetDamageObj();
        if (baseObj == null)
        {
            return 100;
        }
        int baseVal = baseObj.baseDam;
        innerSpell.GetDamageObj().amount = ValueModifier.ApplyModifiers(baseVal, damageModifiers);
        return innerSpell.GetDamageObj().amount;
    }

    public override int GetManaCost()
    {
        int baseVal = innerSpell.GetManaCost();
        return ValueModifier.ApplyModifiers(baseVal, manaModifiers);
    }

    public override float GetCooldown()
    {
        float baseVal = innerSpell.GetCooldown();
        int modified = ValueModifier.ApplyModifiers((int)baseVal, cooldownModifiers);
        return modified;
    }

    // Speed is typically retrieved via RPN; here we adjust speed before casting.
    public float GetSpeed()
    {
        float baseSpeed = innerSpell.rpn.RPN_to_float(innerSpell.projectile.speed);
        int modified = ValueModifier.ApplyModifiers((int)baseSpeed, speedModifiers);
        return modified;
    }


    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float projDelay = 0.0f, float splitSpread = 0.0f)
    {
        this.AssignOwner(this.owner);
        innerSpell.damageFull = new Damage(this.rpn.RPN_to_int(innerSpell.damage.amount), innerSpell.damage.type);
        innerSpell.GetDamageObj().amount = this.GetDamage();

        yield return new WaitForSeconds(projDelay);

        if (cast < this.casts)
        {
            Debug.Log("MultiCast");
            CoroutineManager.Instance.Run(this.Cast(where, target, team, cast += 1, this.delay, this.splitSpread));
        }
        // Ensure inner spell has the same owner and RPN context
        innerSpell.AssignOwner(owner);
        this.team = team;

void WrappedOnHit(Hittable other, Vector3 impact)
{

    if (other.team != team)
    {
        var ec = other.owner.GetComponent<EnemyController>();
        if (ec != null)
        {

            if (stunDuration > 0)
            {
                ec.StartCoroutine(ApplyStun(ec, stunDuration));
            }

            if (vulnerabilityMultiplier > 1f)
            {
                ec.ApplyVulnerability(vulnerabilityMultiplier, 5f);
            }

            if (missChance > 0)
            {
                ec.ApplyInaccuracy(missChance, 5f);
            }
        }
    }

    innerSpell.OnHit(other, impact);
}




        GameManager.Instance.projectileManager.CreateProjectile(0, innerSpell.projectile.trajectory, where, target - where, GetSpeed(), WrappedOnHit);

        //Updates inner spell's damage object

        // Override projectile speed if modifiers exist
        float finalSpeed = GetSpeed();
        var proj = innerSpell.projectile;
        if (splitSpread != 0.0f)
        {
            Debug.Log(this.innerSpell);
            CoroutineManager.Instance.Run(innerSpell.Cast(where, target, team));
        }
        else
        {
            Debug.Log(this.innerSpell);
            CoroutineManager.Instance.Run(innerSpell.Cast(where, target, team));
        }


        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator ApplyStun(EnemyController enemy, float duration)
    {
        if (enemy != null)
        {
            enemy.isStunned = true;
            yield return new WaitForSeconds(duration);
            enemy.isStunned = false;
        }
    }
}
public class ArcaneBolt : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }
    ArcaneBolt() : base()
    {
    }
}

public class MagicMissile : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }
    MagicMissile() : base()
    {
    }
}

public class ArcaneBlast : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }

    override public void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            if (this.slowEvents.Count > 0)
            {
                var enemyController = other.owner.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    foreach (var slow in this.slowEvents)
                    {
                        enemyController.StartCoroutine(ApplySlow(enemyController, slow.Item1, slow.Item2));
                    }
                }
            }

            Debug.Log("Seen KB: " + this.knockback);
            var rb = other.owner.GetComponent<EnemyController>();
            if (rb != null)
            {
                rb.StartCoroutine(DoKnockback(rb, (int)this.knockback));
            }
            if (this.damageFull != null)
            {
                other.Damage(damageFull);
            }
            else
            {
                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                other.Damage(damageFull);
            }
        }
        Projectile projectile = this.secondary_projectile;
        float currCircleOffset = 0.0f;
        int numProj = this.rpn.RPN_to_int(this.N);
        float projOffsetDiff = 2.0f / (float)numProj;
        for (int x = 0; x < numProj; x++)
        {
            float radAddPi = currCircleOffset * Mathf.PI;
            Vector3 targetOffset = new Vector3(impact.x - (Mathf.Sin(radAddPi) * 100.0f), impact.y - (Mathf.Cos(radAddPi) * 100.0f), impact.z);
            GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, impact, targetOffset, this.rpn.RPN_to_float(projectile.speed), this.SecondaryOnHit/*, this.rpn.RPN_to_float(projectile.lifetime)*/);

            currCircleOffset += projOffsetDiff;
        }


    }



    public void SecondaryOnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            if (this.damageFull != null)
            {
                other.Damage(damageFull);
            }
            else
            {
                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                other.Damage(damageFull);
            }

        }
    }

    ArcaneBlast() : base()
    {
    }
}

public class ArcaneSpray : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        int numProj = this.rpn.RPN_to_int(this.N);
        for (int x = 0; x < numProj; x++)
        {
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(this.spray * -1.0f, this.spray), 0, 0);
            GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where + randomOffset, this.rpn.RPN_to_float(projectile.speed), this.OnHit, this.rpn.RPN_to_float(projectile.lifetime));
        }


        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }
    ArcaneSpray() : base()
    {
    }
}

public class ArcaneBounce : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
        this.last_cast = Time.time;
        yield return new WaitForEndOfFrame();
    }

    override public void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            Debug.Log("Seen KB: " + this.knockback);
            var rb = other.owner.GetComponent<EnemyController>();
            if (rb != null)
            {
                rb.StartCoroutine(DoKnockback(rb, (int)this.knockback));
            }
            if (this.damageFull != null)
            {
                other.Damage(damageFull);
            }
            else
            {
                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                other.Damage(damageFull);
            }
            if (this.damageFull != null)
            {
                other.Damage(damageFull);
            }
            else
            {
                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                other.Damage(damageFull);
            }
        }
        Projectile projectile = this.secondary_projectile;
        impact += Vector3.up;
        //int numProj = this.rpn.RPN_to_int(this.N);
        //float projOffsetDiff = 2.0f / (float)numProj;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, impact, impact + Vector3.up, this.rpn.RPN_to_float(projectile.speed), this.SecondaryOnHit);


    }
    public void SecondaryOnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            if (this.damageFull != null)
            {
                other.Damage(damageFull);
            }
            else
            {
                damageFull = new Damage(this.rpn.RPN_to_int(damage.amount), damage.type);
                other.Damage(damageFull);
            }
        }
    }

    ArcaneBounce() : base()
    {

    }
}

public class ChainingLightningSpell : Spell
{
    private int jumpsLeft = 3;
    private float jumpRange = 10f;
    GameObject current;

    // public ChainingLightningSpell(SpellCaster owner) : base(owner) { }

    // public override string GetName() => "Chain Lightning";

    // public override void SetAttributes(JObject attributes)
    // {
    //     base.SetAttributes(attributes);
    //     maxJumps = RPN.ParseInt(attributes["N"]?.ToString(), owner.spellPower);
    //     jumpRange = RPN.ParseFloat(attributes["jump_range"]?.ToString(), owner.spellPower);
    // }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameObject firstTarget = GameManager.Instance.GetClosestEnemy(target);
        GameObject current = firstTarget;
        if (firstTarget != null)
        {
            GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target-where, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
            //yield return ChainHit(firstTarget, maxJumps);
        }
        yield return new WaitForEndOfFrame();
    }

    // private IEnumerator ChainHit(GameObject current, int jumpsLeft)
    // {
    //     // if (jumpsLeft <= 0) break;
    //     if (jumpsLeft <= 0) yield break;

    //     var enemyController = current.GetComponent<EnemyController>();
    //     // if enemyController not null
    //     //hitablle = enemyController.hp
    //     // var hittable = enemyController != null ? enemyController.hp : null;

    //     // if (hittable != null && hittable.team != team)
    //     // {
    //     //     hittable.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
    //     // }

    //     yield return new WaitForSeconds(0.1f);

    //     GameObject nextTarget = null; // can't be "next" lol
    //     float closestDistance = float.MaxValue;
    //     foreach (var enemy in GameManager.Instance.GetAllEnemies())
    //     {
    //         if (enemy == current) continue;
    //         float dist = Vector3.Distance(current.transform.position, enemy.transform.position);
    //         if (dist < closestDistance && dist <= jumpRange)
    //         {
    //             closestDistance = dist;
    //             nextTarget = enemy;
    //         }
    //     }

    //     if (nextTarget != null)
    //     {
    //         GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, current.transform.position, nextTarget.transform.position, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
    //         yield return ChainHit(nextTarget, jumpsLeft - 1);
    //     }
    // }
    override public void OnHit(Hittable other, Vector3 impact)
    {
        
        if (jumpsLeft <= 0) {
            return;
        }
        Debug.Log(jumpsLeft + " JumpsLeft");
        //var enemyController = current.GetComponent<EnemyController>();

        //yield return new WaitForSeconds(0.1f);

        GameObject nextTarget = null; // can't be "next" lol
        float closestDistance = float.MaxValue;
        foreach (var enemy in GameManager.Instance.GetAllEnemies())
        {
            if (enemy == current) continue;
            // Debug.Log("current.transform: " + current.transform.position);
            // Debug.Log("enemy.transform: " + enemy.transform.position);

            float dist = Vector3.Distance(current.transform.position, enemy.transform.position);
            if (dist < closestDistance && dist <= jumpRange)
            {
                closestDistance = dist;
                nextTarget = enemy;
            }
        }

        if (nextTarget != null)
        {
            GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, current.transform.position, nextTarget.transform.position, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
            jumpsLeft--;
            current = nextTarget;
            
            //yield return ChainHit(nextTarget, jumpsLeft - 1);
        }
        
    }

    ChainingLightningSpell() : base()
    {

    }
}

public class FireballSpell : Spell
{
    private float radius = 5f;
    private Vector3 location;

    // public FireballSpell(SpellCaster owner) : base(owner) { }

    // public override string GetName() => "Fireball";

    // public override void SetAttributes(JObject attributes)
    // {
    //     base.SetAttributes(attributes);
    //     radius = RPN.ParseFloat(attributes["radius"]?.ToString(), owner.spellPower);
    // }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, int cast = 0, float delay = 0.0f, float splitSpread = 0.0f)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        location = where;
        //float radius = 5f;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, this.rpn.RPN_to_float(projectile.speed), this.OnHit);
        
        

        yield return new WaitForEndOfFrame();
    }

    override public void OnHit(Hittable other, Vector3 impact)
    {
        var enemies = new List<GameObject>(GameManager.Instance.GetAllEnemies());
        foreach (var enemy in enemies)
        {   
            
            float dist = Vector3.Distance(location, enemy.transform.position);
            if (dist <= radius)
            {
                var enemyController = enemy.GetComponent<EnemyController>();
                var hittable = enemyController != null ? enemyController.hp : null;

                if (hittable != null && hittable.team != team)
                {
                    hittable.Damage(new Damage(GetDamage(), Damage.Type.FIRE));
                }
            }
        }
    }
    FireballSpell() : base()
    {

    }
}


public abstract class ValueModifier
{
    public abstract int Apply(int value);

    public static int ApplyModifiers(int baseValue, IEnumerable<ValueModifier> mods)
    {
        int result = baseValue;
        foreach (var m in mods)
            result = m.Apply(result);
        return result;
    }
}


public class ValueAdder : ValueModifier
{
    public int amount;
    public ValueAdder(int amount)
    {
        this.amount = amount;
    }
    public override int Apply(int value)
    {
        return value + amount;
    }
}


public class ValueMultiplier : ValueModifier
{
    public float amount;
    public ValueMultiplier(float amount) { this.amount = amount; }
    public override int Apply(int value)
    {
        return Mathf.RoundToInt(value * amount);
    }
}
