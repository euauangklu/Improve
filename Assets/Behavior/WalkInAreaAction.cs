using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Walk in area", story: "[Agent] Walk Random in area", category: "Action", id: "93cf60139b4442815e118e1192a83798")]
public partial class WalkInAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Transform> areaTopLeft;

    [SerializeReference] public BlackboardVariable<Transform> areaBottomRight;

    [SerializeReference] public BlackboardVariable<float> Speed;
    
    [SerializeReference] public BlackboardVariable<float> WaitTime = new BlackboardVariable<float>(1.0f);
    
    private float WaitTimer;
    
    private bool Waiting;

    private Vector2 targetPosition;

    protected override Status OnStart()
    {
        PickNewTarget();
        WaitTimer = WaitTime.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position, targetPosition, Speed * Time.deltaTime);
        if (Vector2.Distance(Agent.Value.transform.position, targetPosition) < 0.1f)
        {
            Waiting = true;
        }

        if (Waiting)
        {
            if (WaitTimer > 0f)
            {
                WaitTimer -= Time.deltaTime;
            }
            else
            {
                Waiting = false;
                return Status.Success;
            }
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
    
    void PickNewTarget()
    {
        float x = Random.Range(areaTopLeft.Value.position.x, areaBottomRight.Value.position.x);
        float y = Random.Range(areaBottomRight.Value.position.y, areaTopLeft.Value.position.y);
        targetPosition = new Vector2(x, y);
    }
}

