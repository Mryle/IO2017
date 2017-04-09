using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

/* Basic message oriented priority queue. */
public class BasicPriorQueue {
	protected List<Info> Infos;
	
	public BasicPriorQueue() {
		Infos = new List<Info>();
	}
	
	public bool isEmpty() {
		return Infos.Count == 0; 
	}
	
	public Info get() {
		Info result = Infos[0];
		Infos.RemoveAt(0);
		return result;
	}
	
	public void add(Info info) {
		int i;
		/* Last information about player location should be removed. */
		for(i = 0; i != Infos.Count; ++i) {
			if(Infos[i].message == info.message)
				break;
		}
		Infos.RemoveAt(i);
		/* Order is set by time, distance. */
		/* Time in low range should be unrecognized... todo*/
		for(i = 0; i != Infos.Count; ++i) {
			if(!(Infos[i].time < info.time))
				break;
		}
		Infos.Insert(i, info);
	}
}

public class EnemyBasicAI : MonoBehaviour {
	protected NavMeshAgent Agent;
	protected EnemyPerception Perception;
	protected BasicPriorQueue queue;
	protected float PerceptionDelay;
	protected float AttackDistance;
	protected float AttackTimeDelay;
	protected float MinimalDistance;
	protected float PatrolDistance;
	
	void start() {
		Agent = GetComponent<NavMeshAgent>();
		Perception = GetComponent<EnemyPerception>();
		StartCoroutine(basicAI());
	}
	
	void chase(Vector3 location) {
		Agent.SetDestination(location);
	}
	
	void attack(Vector3 location) {
		/*Shoot at location*/
	}
	
	void iddle() {
		if(!Agent.hasPath) {
			/* Get random destination on map */
			Vector3 randomPoint = Random.insideUnitSphere * PatrolDistance;
			/* Add as offset from current position */
			randomPoint += transform.position;
			/* Get nearest valid position */
			NavMeshHit hit;
			NavMesh.SamplePosition(randomPoint, out hit, PatrolDistance, 1);
			/* Set destination */
			Agent.SetDestination(hit.position);			
		}
	}
	
	void updatePerception() {
		List<Info> Infos = Perception.getInfos();
		foreach(Info info in Infos) {
			queue.add(info);
		}
	}
	
	IEnumerator basicAI() {
		for(;;) {
			yield return new WaitForSeconds(PerceptionDelay);
			updatePerception();
			/* No info from perception. */
			if(queue.isEmpty()) {
				iddle();
				continue;
			}
			Info info = queue.get();
			if(Vector3.Distance(Agent.transform.position, info.location) <  AttackDistance &&
			Time.time < AttackTimeDelay + info.time) {
				attack(info.location);
			}
			else if(Vector3.Distance(Agent.transform.position, info.location) > MinimalDistance){
				/* Chase could be stoped by next acction. So we add it back to queue. */
				queue.add(info);
				chase(info.location);
			}
		}
	}	
	
	
	
}