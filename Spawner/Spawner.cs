using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour {
	[Tooltip("List of prefabs (null for not generating)")]
	public GameObject[] prefabs;
	[Tooltip("List of minimum score for that prefab to be generated.")]
	public int[] variability;
	private static System.Random rng = new System.Random();
	
	private virtual randomSpawn(Vector3);
	
	private void spawn(GameObject prefab, Vector3 spawnPoint) 
	{
		if (prefab != null)
			Instantiate(prefab, spawnPoint, Random.rotation);
	}

	//Mock
	private int getScore() 
	{
		return 1;
	}
	
	private Vector3 getRandomLocation()
    {
         NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
         int t = Random.Range(0, navMeshData.indices.Length-3);
         Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t+1]], Random.value);
         Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t+2]], Random.value);
         return point;
     }
	
	IEnumerator SpawnCoroutine()
	{	yield return new WaitForSeconds(initialDelay);
		while (true)
		{
			Vector3 randomPoint = getRandomLocation();
			randomSpawn(randomPoint);
			yield return new WaitForSeconds(spawnDelay);
		}
	}
	
	void OnEnable()
	{
		StartCoroutine(SpawnCoroutine());
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}
	
}