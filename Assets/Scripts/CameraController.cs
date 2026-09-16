using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float verticalOffset = 2f;
    public GameObject target = null;

    void LateUpdate()
    {
        // if valid target, then update position, otherwise do nothing
        if (target)
        {
            Vector3 newPos = target.transform.position;
            newPos.z = transform.position.z;
            newPos.y += verticalOffset;
            transform.position = newPos;
        }
    }
}
