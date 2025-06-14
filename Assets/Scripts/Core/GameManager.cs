using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }
    public GameState state;
    public int wave;
    public int damageDealt;
    public int damageReceived;
    public int timeSpent;
    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;



    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a, b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    private GameManager()
    {
        wave = 1;
        damageDealt = 0;
        damageReceived = 0;
        timeSpent = 0;
        enemies = new List<GameObject>();
    }

    public IEnumerable<GameObject> GetAllEnemies()
    {
        return enemies;
    }

    public void resetGame()
    {
        wave = 1;
        damageDealt = 0;
        damageReceived = 0;
        timeSpent = 0;
        enemies.Clear();
    }
}
