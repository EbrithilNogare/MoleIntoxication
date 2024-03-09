using UnityEngine;

public class LockCameraWidth : MonoBehaviour
{
    public float width;
    void Update()
    {
        Camera.main.orthographicSize = width / ((float)Screen.width / Screen.height) * 0.5f;
    }
}
