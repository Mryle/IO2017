﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryGameMode : BaseGameMode, IScoredGameMode<Int32>
{
    public event EventHandler OnScoreChanged;

    public UIDeadMenu deadMenu;
    public int scorePerEnemy = 10;
    public int scorePerLevel = 500;

    private HashSet<GameObject> enemies = new HashSet<GameObject>();
    private HashSet<GameObject> players = new HashSet<GameObject>();
    private int scoreCount = 0;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        deadMenu = Resources.FindObjectsOfTypeAll<UIDeadMenu>().First();
    }

    public override string GetName()
    {
        return "Story";
    }

    public override GameModeType GetModeType()
    {
        return GameModeType.Story;
    }

    public override void OnEnemySpawned(GameObject enemy)
    {
        enemies.Add(enemy);
        enemy.GetComponent<EnemyStatus>().healthChanged += OnEnemyHealthChanged;
    }

    public override void OnPlayerSpawned(GameObject player)
    {
        players.Add(player);
        player.GetComponent<PlayerStatus>().healthChanged += OnPlayerHealthChanged;
    }

    public int GetCurrentScore()
    {
        return scoreCount;
    }

    void OnEnemyHealthChanged(GameObject enemy, float health)
    {
        if (health <= 0 && enemies.Contains(enemy))
        {
            enemies.Remove(enemy);

            scoreCount += scorePerEnemy;

            if (OnScoreChanged == null)
                return;

            foreach (var player in players)
            {
                var args = new ScoreChangedEventArgs<int>() { player = player, value = scoreCount };
                OnScoreChanged(player, args);
            }
        }
    }

    void OnPlayerHealthChanged(GameObject player, float health)
    {
        if (health <= 0)
        {
            deadMenu.gameObject.SetActive(true);
        }
    }

    public int GetScore(GameObject player)
    {
        return scoreCount;
    }

    public int GetMaximumScore(GameObject player)
    {
        return Int32.MaxValue;
    }

    public bool IsScoreCapped()
    {
        return false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        deadMenu = Resources.FindObjectsOfTypeAll<UIDeadMenu>().First();
    }

    public void LevelFinished()
    {
        scoreCount += scorePerLevel;

        if (OnScoreChanged == null)
            return;

        foreach (var player in players)
        {
            var args = new ScoreChangedEventArgs<int>() { player = player, value = scoreCount };
            OnScoreChanged(player, args);
        }
    }
}