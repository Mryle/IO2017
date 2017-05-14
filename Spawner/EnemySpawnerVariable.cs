using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerVariable : Spawner {
	public float initialDelay = 1.0f;
	public float spawnDelay = 5.0f;
	
	void override randomSpawn(Vector3 spawnPoint) {
		int score = getScore();
		GameObject randomEnemy = getRandomEnemy(score);
		spawn(randomEnemy, spawnPoint);
	}
	
	GameObject getRandomEnemy(int score) {
		int poolSize = 0;
		int i;
		for(i = 0; i != variability.Length; ++i) {
			if(varability[i] < score) {
				++poolSize;
			}
		}
		if(poolSize == 0)
			return null;
		int randomValue = rng.Next (1, poolSize);
		for(i = 0; i != variability.Length; ++i) {
			if(varability[i] < score) {
				--randomValue;
			}
			if(randomValue == 0)
				break;
		}
		return prefabs[i];
	}
}