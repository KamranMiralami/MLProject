using UnityEngine;

public class RewardTrigger : MonoBehaviour
{
    public int Reward = 1;
    [SerializeField]bool HasReward = true;
    public int GetReward()
    {
        var value = HasReward ? Reward : 0;
        HasReward = false;
        return value;
    }
    public void ResetReward()
    {
        HasReward = true;
    }
}
