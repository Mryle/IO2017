﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YAGTSS.Serialization;

public class UIGame : MonoBehaviour
{
    public string pauseButton = "Pause";
    public GameObject pauseMenu;
    public GameObject firstSelected;
    public GameObject deadMenu;
    public Slider healthBar;
    public Text scoreText;
    public Text scoreTextDead;
    public InputField playerName;

    private GameObject[] players;
    private PlayerStatus[] playerStatuses;
    private bool isPaused = false;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerStatuses = new PlayerStatus[players.Length];
        int i = 0;
        foreach (GameObject player in players)
        {
            playerStatuses[i] = players[i].GetComponent<PlayerStatus>();
            ++i;
        }
        if (i == 0)
        {
            Debug.Log("No player");
        }
    }

    void Update()
    {
        scoreText.text = "Score: " + playerStatuses[0].getScore();
        if (playerStatuses[0].getHealth() == 0)
        {
            ToggleDeadMenu();
        }
        if (Input.GetButtonDown(pauseButton))
        {
            TogglePauseMenu();
        }
    }

    public void ToggleDeadMenu()
    {
        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }
        Time.timeScale = 0.0f;
        deadMenu.SetActive(true);
        playerName.Select();
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        foreach (GameObject player in players)
        {
            player.SetActive(!isPaused);
        }
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        // Toggle container with pause menu items (background, buttons etc.) 
        pauseMenu.SetActive(isPaused);
        EventSystem.current.SetSelectedGameObject(isPaused ? firstSelected : null);
    }

    public void SaveNameAndQuit()
    {
        SaveData data = SaveGameSerializer.Load();
        data.highScores.Add(playerName.text, playerStatuses[0].getScore());
        SaveGameSerializer.Save(data);
        QuitToMainMenu();
    }

    public void QuitToMainMenu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene("Main_Menu"); 
#endif
    }
}
