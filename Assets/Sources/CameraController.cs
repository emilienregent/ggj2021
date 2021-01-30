using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera introCamera = null;
    public CinemachineVirtualCamera gameCamera = null;

    private void Start()
    {
        introCamera.Priority = 0;
        gameCamera.Priority = 100;
    }
}