using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;
    private int currentWave = 1;



    public Dictionary<string, Enemy> enemy_types;
    public Dictionary<string, Level> levels;

    private string current_level;

    public TextMeshProUGUI gameOver;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject selector_easy = Instantiate(button, level_selector.transform);
        selector_easy.transform.localPosition = new Vector3(-100, 30);
        selector_easy.GetComponent<MenuSelectorController>().spawner = this;
        selector_easy.GetComponent<MenuSelectorController>().SetLevel("Easy");
        GameObject selector_medium = Instantiate(button, level_selector.transform);
        selector_medium.transform.localPosition = new Vector3(100, 30);
        selector_medium.GetComponent<MenuSelectorController>().spawner = this;
        selector_medium.GetComponent<MenuSelectorController>().SetLevel("Medium");
        GameObject selector_endless = Instantiate(button, level_selector.transform);
        selector_endless.transform.localPosition = new Vector3(0, -30);
        selector_endless.GetComponent<MenuSelectorController>().spawner = this;
        selector_endless.GetComponent<MenuSelectorController>().SetLevel("Endless");

        //Json Deserialization
        enemy_types = new Dictionary<string, Enemy>();
        var enemytext = Resources.Load<TextAsset>("enemies");
        JToken jo = JToken.Parse(enemytext.text);
        foreach (var enemy in jo)
        {
            Enemy en = enemy.ToObject<Enemy>();
            enemy_types[en.name] = en;
        }
        levels = new Dictionary<string, Level>();
        var leveltext = Resources.Load<TextAsset>("levels");
        JToken jol = JToken.Parse(leveltext.text);
        foreach (var level in jol)
        {
            Level lvl = level.ToObject<Level>();
            levels[lvl.name] = lvl;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLevel(string levelname)
    {
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        current_level = levelname;
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(SpawnWave());
    }

    public void NextWave()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER) {
            //gameOver.text = "Restart";
            Debug.Log("restarted");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.Instance.state = GameManager.GameState.PREGAME;
            return;
        } 

        StartCoroutine(SpawnWave());
    }

    public int RPN_to_int(string rpn, int enemy_base_hp = 0)
    {

        Stack<int> stack = new Stack<int>();
        string[] tokens = rpn.Split(' ');

        foreach (string token in tokens)
        {
            if (token == "wave")
            {
                stack.Push(currentWave);
            }
            else if (token == "base")
            {
                stack.Push(enemy_base_hp);
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%")
            {


                int b = stack.Pop();
                int a = stack.Pop();
                if (token == "+")
                {
                    stack.Push(a + b);
                }
                else if (token == "-")
                {
                    stack.Push(a - b);
                }
                else if (token == "*")
                {
                    stack.Push(a * b);
                }
                else if (token == "/")
                {
                    stack.Push(a / b);
                }
                else if (token == "%")
                {
                    stack.Push(a % b);
                }
            }
            else
            {
                if (int.TryParse(token, out int value))
                {
                    stack.Push(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        return stack.Count > 0 ? stack.Pop() : 0;
    }



    IEnumerator SpawnWave()
    {
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.state = GameManager.GameState.INWAVE;

        Level level = levels[current_level];
        Debug.Log("Spawing wave " + currentWave + " from level " + current_level);
        foreach (var wave in level.spawns)
        {
            StartCoroutine(ManageWave(wave));
        }

        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        currentWave += 1;
        if (currentWave > level.waves)
        {
            GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        }
        else
        {
            GameManager.Instance.state = GameManager.GameState.WAVEEND;
        }


    }

    IEnumerator ManageWave(Spawn spawn)
    {
        // int spawned = 0;
        // while (spawned < 10)
        // {
        //     int num_to_spawn = 5;
        //     for (int i = 0; i < num_to_spawn; i++)
        //     {
        //         yield return SpawnEnemy(spawn.enemy);
        //         spawned++;
        //     }
        //     yield return new WaitForSeconds(spawn.delay);
        // }

        int[] sequence = spawn.sequence;
        if (sequence == null)
        {
            sequence = new int[] { 1 };
        }
        int sequence_iterator = 0;

        int total_to_spawn = RPN_to_int(spawn.count); // e.g., "5 wave +" should become int
        Enemy enemy_to_spawn = enemy_types[spawn.enemy];
        int enemy_base_hp = enemy_to_spawn.hp;
        int modified_hp = RPN_to_int(spawn.hp, enemy_base_hp);
        int spawned = 0;
        int[] spawn_spaces = parseSpawn(spawn.location);
        float delay = spawn.delay > 0 ? spawn.delay : 1f;

        while (spawned < total_to_spawn)
        {
            for (int i = 0; i < sequence[sequence_iterator]; i++)
            {
                if (spawned == total_to_spawn)
                {
                    break;
                }
                yield return SpawnEnemy(spawn.enemy, modified_hp, spawn_spaces);
                spawned++;
            }
            sequence_iterator = (sequence_iterator + 1) % sequence.Length;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator SpawnEnemy(string enemy_name, int hp, int[] spawn_location)
    {
        Enemy enemy_stats = enemy_types[enemy_name];
        SpawnPoint spawn_point = SpawnPoints[Random.Range(spawn_location[0], spawn_location[0] + spawn_location[1])];
        Vector2 offset = Random.insideUnitCircle * 1.8f;

        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemy_stats.sprite);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = enemy_stats.speed;
        en.damage = enemy_stats.damage;
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f);
    }

    int[] parseSpawn(string spawn_string) => spawn_string switch
    {
        "random green" => new int[] { 0, 3 },
        "random bone" => new int[] { 3, 1 },
        "random red" => new int[] { 4, 3 },
        "random" => new int[] { 0, 7 },
        _ => new int[] { 0, 7 },

    };
}



