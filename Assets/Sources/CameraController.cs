using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera introCamera = null;
    public CinemachineVirtualCamera gameCamera = null;

    private void Start()
    {
        GameManager.instance.StateUpdated += OnStateUpdated;

        OnStateUpdated(GameManager.instance.CurrentGameState);
    }

    private void OnStateUpdated(GameState state)
    {
        switch(state)
        {
            case GameState.Intro:
                introCamera.Priority = 100;
                gameCamera.Priority = 0;
            break;
            case GameState.Tutorial:
                introCamera.Priority = 0;
                gameCamera.Priority = 100;
            break;
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.StateUpdated -= OnStateUpdated;

    }
}