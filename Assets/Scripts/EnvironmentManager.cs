using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Vector3 PlayerStartLocalPosition;
    public Quaternion PlayerStartRotation;
    public GameObject FinishObject;
    [SerializeField] private Transform PlayerTransform;
    private void Awake()
    {
        PlayerStartLocalPosition = PlayerTransform.localPosition;
        PlayerStartRotation = PlayerTransform.rotation;
    }
}
