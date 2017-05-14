using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner {
	[Tooltip("List of rarity wages used to random.")]
	public int[] rarityValues; // Min value is 1
	public float initialDelay = 1.0f;
	public float spawnDelay = 5.0f;
	
	void onEnemyDead(Vector3 deadPoint) {
		randomSpawn(deadPoint);
	}
	
	void override randomSpawn(Vector3 spawnPoint) {
		int score = getScore();
		GameObject randomItem = getRandomItem(score);
		spawn(randomItem, spawnPoint);
	}
	
	GameObject getRandomItem(int score) {
		int poolSize = 0;
		int i;
		for(i = 0; i != variability.Length; ++i) {
			if(varability[i] < score) {
				poolSize += rarityValues[i];
			}
		}
		if(poolSize == 0)
			return null;
		int randomValue = rng.Next (1, poolSize);
		for(i = 0; i != variability.Length; ++i) {
			if(varability[i] < score) {
				randomValue -= rarityValues[i];
			}
			if(randomValue <= 0)
				break;
		}
		return prefabs[i];
	}

}