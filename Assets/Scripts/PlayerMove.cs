using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerMove : Agent
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;
    private EnvironmentManager env;
    private void Awake()
    {
        rb= GetComponent<Rigidbody>();
        env=transform.parent.GetComponent<EnvironmentManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public override void Initialize()
    {
        base.Initialize();
        gameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize = 13 + env.FinishObjects.Count;
        Debug.Log("vector obs size = " + gameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize);
    }
    void RotatePlayer(float input)
    {
        float rotation = input * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation, 0f));
    }
    void MoveForward(float input)
    {
        Vector3 move = input * moveSpeed * Time.fixedDeltaTime * transform.forward;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
    public override void OnEpisodeBegin()
    {
        //Debug.Log("starting episode");
        transform.SetLocalPositionAndRotation(env.PlayerStartLocalPosition, env.PlayerStartRotation);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        env.ResetEnvironment();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetKey(KeyCode.W) ? 1f : 0f;
        continuousActions[1] = Input.GetAxis("Mouse X");    
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveForward = actionBuffers.ContinuousActions[0];
        float rotate = actionBuffers.ContinuousActions[1];
        MoveForward(moveForward);
        RotatePlayer(rotate);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(rb.linearVelocity);
        sensor.AddObservation(rb.angularVelocity);
        foreach(var finishObj in env.FinishObjects)
        {
            sensor.AddObservation(Vector3.Distance(transform.localPosition, finishObj.transform.localPosition));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.CompareTag("Finish"))
        {
            AddReward(100);
            //Debug.Log("you won, restarting episode");
            EndEpisode();
        }
        if (other.CompareTag("RewardTriggers"))
        {
            var rewardTrigger=other.GetComponent<RewardTrigger>();
            AddReward(rewardTrigger.GetReward());
            //Debug.Log("reward added");
        }
        if (other.CompareTag("GameOverTrigger"))
        {
            AddReward(-1);
            //Debug.Log("game over, restarting episode");
            EndEpisode();
        }
        if (other.CompareTag("Obstacle"))
        {
            AddReward(-1);
            //Debug.Log("game over, restarting episode");
            EndEpisode();
        }
    }
}
