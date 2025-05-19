using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;

    public int speed;

    public Unit unit;

    public EnemySpawner enemySpawner;

    public Relic relic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;

    }

    public void StartLevel()
    {
        int wave = GameManager.Instance.wave;
        spellcaster = new SpellCaster(wave * 10 + 90, wave + 10, Hittable.Team.PLAYER, 10 * wave);
        StartCoroutine(spellcaster.ManaRegeneration());

        hp = new Hittable(5 * wave + 95, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spell);
    }
    public void UpdateStats()
    {
        int wave = GameManager.Instance.wave;
        //spellcaster = new SpellCaster(wave * 10 + 90, wave + 10, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        spellcaster.updateSpellPower();
        hp = new Hittable(5 * wave + 95, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spell);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerRelic(Relic relic)
    {
        this.relic = relic;
        this.relic.Trigger.Initialize(this.relic.Effect);
        this.relic.Trigger.Register();
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>() * speed;
        EventBus.Instance.DoMove(unit.transform.position);
    }

    void Die()
    {
        Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }

}
