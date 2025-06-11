using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public int speed;
    public Hittable hp;
    public HealthBar healthui;
    public bool dead;
    public int damage;
    public float last_attack;

    public bool isStunned = false;
    private float vulnerabilityMultiplier = 1f;
    private float missChance = 0f;

    void Start()
    {
        target = GameManager.Instance.player.transform;
        hp.OnDeath += Die;
        healthui.SetHealth(hp);
    }

    void Update()
    {
        if (isStunned) return;

        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 2f)
        {
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }

    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;

            if (Random.value < missChance) return;

            target.gameObject.GetComponent<PlayerController>().hp.Damage(
                new Damage((int)(damage * vulnerabilityMultiplier), Damage.Type.PHYSICAL)
            );
        }
    }

    void Die()
    {
        if (!dead)
        {
            dead = true;
            GameManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }

    public void ApplyVulnerability(float multiplier, float duration) // scott
    {
        StartCoroutine(ApplyVulnerabilityCoroutine(multiplier, duration));
    }

    private IEnumerator ApplyVulnerabilityCoroutine(float multiplier, float duration)
    {
        vulnerabilityMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        vulnerabilityMultiplier = 1f;
    }

    public void ApplyInaccuracy(float chance, float duration)
    {
        StartCoroutine(ApplyInaccuracyCoroutine(chance, duration));
    }

    private IEnumerator ApplyInaccuracyCoroutine(float chance, float duration)
    {
        missChance = chance;
        yield return new WaitForSeconds(duration);
        missChance = 0f;
    }
}