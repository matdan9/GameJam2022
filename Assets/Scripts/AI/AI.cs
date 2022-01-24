using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum AiState{
    Chase,
    Roaming,
    Patrol
}

public class AI 
{
    // AI Settings
    private AiState aiState;
    private string triggerTag;
    private LayerMask triggerMask;
    private AiState defaultAiState = AiState.Patrol;
    private Transform patrolArea;

    //AI Navigation
    private int navMeshArea;
    private NavMeshAgent agent;
    private Vector3 target;
    private bool hasTarget = false;

    // AI View
    private float fov;
    private float contactRange;
    private float dangerRange;
    private float viewRange;

    private bool pause = false;

    public bool Update(Transform trans){
        if(this.pause) return false;
        Collider[] colliders = Physics.OverlapSphere(trans.position, viewRange*2, 1 << triggerMask);
        updatePath(trans);
        foreach(Collider col in colliders){
            Transform target = col.transform;
            Vector3 targetDirection = (target.position - trans.position).normalized;
            float distance = Vector3.Distance(col.transform.position, trans.position);
            if(!hasTargetValue(trans, targetDirection)){
                vocation(trans);
                return false;
            }
            if(Vector3.Angle(trans.forward, targetDirection) < fov / 2){
                if(distance <= dangerRange){
                    return true;
                }
                if (distance <= viewRange * (1 + col.gameObject.GetComponent<LightMecanic>().IntensityRatio()))
                {
                    Chase(col.gameObject);
                    return false;
                }
            }
            if(distance <= contactRange * col.gameObject.GetComponent<LightMecanic>().IntensityRatio())
            {
                Chase(col.gameObject);
                return false;
            }
        }
        vocation(trans);
        return false;
    }

    private void updatePath(Transform trans){
        if(target == null ||  Vector3.Distance(trans.position, target) <= 1f){
            hasTarget = false;
        }
    }

    private void vocation(Transform trans){
        if(defaultAiState == AiState.Roaming){
            roam(trans);
            return;
        }
        patrol();
    }

    private void Chase(GameObject obj){
        agent.stoppingDistance = 2f;
        target = NavMeshHelper.GetCloseCoordinate(obj.transform.position, navMeshArea);
        aiState = AiState.Chase;
        setNewDestination(target);
    }

    private void patrol(){
        if (hasTarget) return;
        agent.stoppingDistance = 0f;
        target = NavMeshHelper.RandomCoordinateInRange(patrolArea.position, 20f, navMeshArea);
        setNewDestinationSafely(target);
        aiState = AiState.Patrol;
    }

    private void roam(Transform trans){
        if (hasTarget) return;
        agent.stoppingDistance = 0f;
        target = NavMeshHelper.RandomCoordinateInRange(trans.position, 20f, navMeshArea);
        setNewDestinationSafely(target);
        aiState = AiState.Roaming;
    }

    private void setNewDestinationSafely(Vector3 target){
        if(hasTarget) return;
        setNewDestination(target);
    }

    private void setNewDestination(Vector3 target){
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        if(path.status == NavMeshPathStatus.PathComplete){
            hasTarget = true;
            agent.SetDestination(target);
        }
        
    }

    private bool hasTargetValue(Transform trans, Vector3 targetDirection){
        RaycastHit hit;
        if(Physics.Raycast(trans.position, targetDirection, out hit, viewRange*2)){
            return hit.collider.tag == triggerTag;
        }
        return false;
    }

    public AiState GetAiState(){
        return this.aiState;
    }

    public void SetAiView(float viewRange, float contactRange, float dangerRange, float fov){
        this.fov = fov;
        this.viewRange = viewRange;
        this.contactRange = contactRange;
        this.dangerRange = dangerRange;
    }

    public void UpdateViewRange(float viewRange){
        this. viewRange = viewRange;
    }

    public void SetNavigation(int area, NavMeshAgent agent){
        this.agent = agent;
        this.navMeshArea = area;
    }

    public void SetAiSettings(string triggerTag, LayerMask mask, AiState defaultAiState, Transform patrolArea){
        this.defaultAiState = defaultAiState;
        this.triggerTag = triggerTag;
        this.triggerMask = mask;
        this.patrolArea = patrolArea;
    }

    public void Pause(){
        pause = true;
        agent.isStopped = true;

    }

    public void Start(){
        pause = true;
        agent.isStopped = false;
    }
}
