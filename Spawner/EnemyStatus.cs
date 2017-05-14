using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
	private int score;
	
    public override void onStart()
    {
        base.onStart();
        healthChanged += OnHealthChanged;

        if (GameController.Instance.gameMode)
            GameController.Instance.gameMode.OnEnemySpawned(this.gameObject);
    }

    void OnHealthChanged(GameObject obj, float health)
    {
        if (health <= 0.0f)
        {
			GameObject itemSpawner = GameObject.FindGameObjectsWithTag("itemSpawner")[0];
			itemSpawner.SendMessage("onEnemyDead", gameObject.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);			
        }
    }
}
