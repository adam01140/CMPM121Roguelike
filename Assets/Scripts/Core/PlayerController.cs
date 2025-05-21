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
    int maxHealth;
    int maxMana;
    int regenRate;
    int spellPower;

    public Unit unit;

    public EnemySpawner enemySpawner;

    public List<Relic> relics;

    public TextAsset classesJson;
    public RPN rpn;

    public string selectedClass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
        classesJson = Resources.Load<TextAsset>("classes");
        relics = new List<Relic>();

    }

    public void StartLevel()
    {



        int wave = GameManager.Instance.wave;
        updateClass();
        spellcaster = new SpellCaster(maxMana, regenRate, Hittable.Team.PLAYER, spellPower);
        StartCoroutine(spellcaster.ManaRegeneration());

        hp = new Hittable(maxHealth, Hittable.Team.PLAYER, gameObject);
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
        updateClass();
        //spellcaster = new SpellCaster(wave * 10 + 90, wave + 10, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        spellcaster.updateSpellPower(spellPower);
        hp = new Hittable(maxHealth, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spell);
    }

    private void updateClass()
    {
        //parse through classes.json
        int wave = GameManager.Instance.wave;
        var root = JObject.Parse(classesJson.text);
        var mageData = (JObject)root[this.selectedClass];
        var vars = new Dictionary<string, int> { { "wave", wave } };
        rpn = new RPN(vars);

        //update stats
        maxHealth = rpn.RPN_to_int(mageData["health"].ToString());
        maxMana = rpn.RPN_to_int(mageData["mana"].ToString());
        regenRate = rpn.RPN_to_int(mageData["mana_regeneration"].ToString());
        spellPower = rpn.RPN_to_int(mageData["spellpower"].ToString());
        speed = rpn.RPN_to_int(mageData["speed"].ToString());

        Debug.Log("maxHealth " + maxHealth);
        Debug.Log("maxMana " + maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        EventBus.Instance.DoUpdate(Time.deltaTime);
    }

    public void SetPlayerRelic(Relic relic)
    {

        this.relics.Add(relic);
        relic.Trigger.Initialize(relic.Effect);
        relic.Trigger.Register();
        if (relic.Until != null)
        {
            relic.Until.Initialize(relic.Effect);
            relic.Until.Register();
        }
        EventBus.Instance.DoRelicGained(relic);

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
