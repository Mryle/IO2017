using UnityEngine;
using System.Collections;

public class Info : MonoBehaviour {
	private string message; 
	private Vector3 position;
	private float time;
	
	public Info(string message, Vector3 position, float time) {
		this.message = message;
		this.position = position;
		this.time = time;
	}
	
	public string getMessage() {
		return this.message;
	}
	
	public Vector3 getposition() {
		return this.position;
	}
	
	public float date() {
			return this.time;
	}
}