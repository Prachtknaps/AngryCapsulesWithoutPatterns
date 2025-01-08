using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image healthbar = null;
    private float anger = 0.0f;

    [Header("Movement")]
    [SerializeField] private float walkRadius = 10.0f;
    [SerializeField] private float idleTime = 2.0f;

    private NavMeshAgent agent = null;
    private bool isMoving = false;
    private bool shouldWander = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Wander());
    }

    public void ApplyDamage(float damage)
    {
        anger += damage;
        if (anger >= 100.0f)
        {
            anger = 100.0f;
            StopWandering();
            Destroy(transform.gameObject);
        }

        healthbar.fillAmount = anger / 100.0f;
    }

    private IEnumerator Wander()
    {
        while (shouldWander)
        {
            if (!isMoving)
            {
                Vector3 randomDestination = GetRandomPoint(transform.position, walkRadius);

                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(randomDestination);
                    isMoving = true;

                    while (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
                    {
                        yield return null;
                    }
                }

                yield return new WaitForSeconds(idleTime);
                isMoving = false;
            }

            yield return null;
        }
    }

    private Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return origin;
    }

    public void StopWandering()
    {
        shouldWander = false;
    }
}
