using UnityEngine;

public class IntroController : MonoBehaviour
{
    public GameObject text = null;

    public GameObject door = null;
    public LeanTweenType doorEaseType = LeanTweenType.linear;

    [Header("Button")]
    public GameObject button = null;
    public LeanTweenType buttonEaseType = LeanTweenType.linear;
    public Color normalColor = Color.white;
    public Color emissiveColor = Color.white;

    private Material buttonMaterial = null;

    private void Start()
    {
        buttonMaterial = button.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.tag == "Button")
                {
                    OnButtonClicked();
                }
            }
        }

        if (GameManager.instance.CurrentGameState == GameState.Intro)
        {
            buttonMaterial.SetColor("_OutlineColor", Color.Lerp(normalColor, emissiveColor, Mathf.PingPong(Time.time, 1)));
        }
        else if(button.gameObject.activeSelf)
        {
            buttonMaterial.SetColor("_OutlineColor", Color.black);
        }
    }

    private void OnButtonClicked()
    {
        GameManager.instance.CurrentGameState = GameState.Tutorial;

        LeanTween.moveZ(button, button.transform.position.z + 0.3f, 0.5f).setEase(buttonEaseType);
        LeanTween.rotateY(door, 90f, 1f).setEase(doorEaseType);

        text.SetActive(false);
    }
}