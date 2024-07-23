using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 PlayerStartLocalPosition;
    [HideInInspector]
    public Quaternion PlayerStartRotation;
    public List<GameObject> FinishObjects;
    [SerializeField] private Transform PlayerTransform;
    public List<RewardTrigger> RewardTriggers;
    private void Awake()
    {
        PlayerStartLocalPosition = PlayerTransform.localPosition;
        PlayerStartRotation = PlayerTransform.rotation;
    }
    public void ResetEnvironment()
    {
        foreach (var rewardTrigger in RewardTriggers)
        {
            rewardTrigger.ResetReward();
        }
    }
}
