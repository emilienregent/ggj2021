using UnityEngine;

public class UVScrollBehaviour : MonoBehaviour
{
    public MeshRenderer meshRenderer = null;
    public Vector2 speed = Vector2.zero;
    private Vector2 offset = Vector2.zero;

    private void Update()
    {
        offset += speed;

        meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}
