using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Chase, Patrol
    }

    public Transform player;
    public EnemyState currentState;

    Transform target;
    NavMeshAgent agent;

    public Transform[] path;
    public int pathIndex = 0;
    public float distThreshold = 0.2f; //floating point math is inexact, this allows us to get close enough to a waypoint and move to the next one.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        target = (path.Length > 0) ? path[0] : player ? player.transform : null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target) return;

        if (currentState == EnemyState.Chase) target = player;

        if (currentState == EnemyState.Patrol)
        {
            if (target == player) target = path[pathIndex];

            if (agent.remainingDistance < distThreshold)
            {
                pathIndex++;
                //if we reach the end of the path - go back to zero
                //if (pathIndex == path.Length) pathIndex = 0;
                pathIndex %= path.Length;
                target = path[pathIndex];
            }
        }

        agent.SetDestination(target.position);
    }
}
