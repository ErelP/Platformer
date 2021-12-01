using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;

    private void Update()
    {
        transform.position = Target.position + offset;
    }
}