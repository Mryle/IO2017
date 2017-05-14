using UnityEngine;
using System.Collections;
using System.Collections.Generic;

<<<<<<< HEAD
public enum AIPerceptionInformation { seenPlayer, heardPlayer };

public struct Info {
	public AIPerceptionInformation message; 
	public Vector3 location;
	public float time;
	public int playerNumber;

	public Info(AIPerceptionInformation message, Vector3 location, float time, int playerNumber) {
		this.message = message;
		this.location = location;
		this.time = time;
		this.playerNumber = playerNumber;
=======
public struct Info {
	public string message; 
	public Vector3 location;
	public float time;

	public Info(string message, Vector3 location, float time) {
		this.message = message;
		this.location = location;
		this.time = time;
>>>>>>> 8b73297795af03dc9072359402862457e74f8ca8
	}
}

public class EnemyPerception : MonoBehaviour {
	public float hearingDistance;
	public float sightDistance;
	public float sightAngle;
	public float detectionInterval = 0.2f;

	protected GameObject[] players;
	protected List<Info> infos = new List<Info>();
	
	void Start() {
<<<<<<< HEAD
		
=======
>>>>>>> 8b73297795af03dc9072359402862457e74f8ca8
		players =  GameObject.FindGameObjectsWithTag("Player");
		StartCoroutine(Perception());
	}
	
	/*Add delay*/
	IEnumerator Perception() {
		while (true) {
			yield return new WaitForSeconds(detectionInterval);
			infos.Clear();
			int i = 0;
			foreach(GameObject player in players) {
				Vector3 rayDirection = player.transform.position - transform.position;
				float distance = rayDirection.magnitude;
				float angle = Vector3.Angle(rayDirection, transform.forward);
				// Player is in sight area
				if (distance < sightDistance && angle < sightAngle) {
					RaycastHit hitInfo;
					bool hit = Physics.Raycast(transform.position, rayDirection, out hitInfo, sightDistance);
					// Player is seen
					if (hit && hitInfo.transform.tag == "Player") {
<<<<<<< HEAD
						Info info = new Info(AIPerceptionInformation.seenPlayer, player.transform.position, Time.time, i);
						infos.Add(info);
						//print ("Player " + i + " seen");
=======
						Info info = new Info("Player " + i + " seen", player.transform.position, Time.time);
						infos.Add(info);
>>>>>>> 8b73297795af03dc9072359402862457e74f8ca8
					}
				}
				/* Player is heard */
				else if (distance < hearingDistance) {
<<<<<<< HEAD
					Info info = new Info(AIPerceptionInformation.heardPlayer, player.transform.position, Time.time, i);
					infos.Add(info);
					//print ("Player " + i + " heard");
				}
				i++;

=======
					Info info = new Info("Player " + i + " heard", player.transform.position, Time.time);
					infos.Add(info);
				}
				i++;
>>>>>>> 8b73297795af03dc9072359402862457e74f8ca8
			}
		}
	}	
	
	public List<Info> getInfos() {
		return infos;
	}
}
