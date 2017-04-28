<<<<<<< HEAD
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;

public enum AIState { chasePlayer, searchingPlayer, patrol, runAway };

public class EnemyBasicAI : MonoBehaviour
{
    public float attackRange = 2.0f;
    public float maxDistanceToStartChase = 20;
    public float reduceMinDistanceChaseValue = 0.2f;
    public float hpEscapeThreshold = 20.0f;
    public AIState aiState;
    public Vector3 lastAITargetPosition;

    private NavMeshAgent agent;
    private EnemyPerception perception;
    private EnemyAttack attack;
    private Status status;
    private List<Info> infos = new List<Info>();

    void OnHealthChanged(GameObject obj, float health)
    {
        if (health <= float.Epsilon)
        {
            const float delay = 1.0f;
            Destroy(gameObject, delay);
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        perception = GetComponent<EnemyPerception>();
        attack = GetComponent<EnemyAttack>();
        status = GetComponent<Status>();
        status.healthChanged += OnHealthChanged;

        lastAITargetPosition = transform.position;
        aiState = AIState.patrol;

        StartCoroutine(basicAI());
    }

    void chasePlayer(Vector3 location)
    {
        lastAITargetPosition = location;
        agent.SetDestination(location);
    }

    void changeAIState(AIState newState)
    {
        aiState = newState;
    }

    void Attack(Vector3 location)
    {
        if (attack != null)
        {
            attack.Attack();
        }
    }

    Vector3 lastNoticedPlayerPosition()
    {
        Vector3 returnPosition = Vector3.forward;
        float minDistance = float.MaxValue;

        foreach (Info inf in infos)
        {
            float nextPlayerDistance = Vector3.Distance(gameObject.transform.position, inf.location);
            if (minDistance > nextPlayerDistance)
            {
                minDistance = nextPlayerDistance;
                returnPosition = inf.location;
            }
        }
        return returnPosition;
    }

    bool noticedPlayers()
    {
        foreach (Info inf in infos)
        {
            if (inf.message == AIPerceptionInformation.heardPlayer ||
                inf.message == AIPerceptionInformation.seenPlayer)
            {
                return true;
            }
        }
        return false;
    }

    void makeDecisionWhenNoticedPlayer(Vector3 target)
    {
        float distanceToTarget = Vector3.Distance(gameObject.transform.position, target);
        float health = status.getHealth();

        if (distanceToTarget <= maxDistanceToStartChase && health > hpEscapeThreshold)
        {
            chasePlayer(target);
            changeAIState(AIState.chasePlayer);

            if (distanceToTarget <= attackRange)
            {
                Attack(transform.position + transform.forward * attackRange);
            }
        }
        else
        {
            if (health <= hpEscapeThreshold)
            {
                //run away ToDo
                changeAIState(AIState.runAway);
            }
            else
            {
                //player too far to engage
                changeAIState(AIState.patrol);
            }
        }
    }

    void updatePerception()
    {
        infos = perception.getInfos();
    }

    private IEnumerator basicAI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            updatePerception();

            if (noticedPlayers())
            {
                Vector3 aiTargetLocation = lastNoticedPlayerPosition();

                makeDecisionWhenNoticedPlayer(aiTargetLocation);

            }
            else if (aiState == AIState.chasePlayer)
            {
                //going to last indicated point
                changeAIState(AIState.searchingPlayer);
                maxDistanceToStartChase -= reduceMinDistanceChaseValue;
            }
        }
    }
=======
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
	
	
	
>>>>>>> 8b73297795af03dc9072359402862457e74f8ca8
}