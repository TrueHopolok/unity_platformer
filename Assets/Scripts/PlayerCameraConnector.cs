using UnityEngine;

public class PlayerCameraConnector : MonoBehaviour
{
    void Start()
    {
        // Important that this method is called ONCE not every frame
        // Since otherwise it would EAT perfomance
        GameObject obj = GameObject.FindWithTag("MainCamera");
        CameraController control = obj.GetComponent<CameraController>();
        control.target = gameObject;
    }
}
