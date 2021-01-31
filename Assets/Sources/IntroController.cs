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

    private float _timeStart = 0f;
    private Material _buttonMaterial = null;

    private int _tweenId = -1;

    private void Start()
    {
        _timeStart = Time.time;
        _buttonMaterial = button.GetComponent<MeshRenderer>().material;

        _tweenId = LeanTween.rotateAround(button, button.transform.up, -15f, 0.1f).setOnComplete(OnStartCallToAction).setDelay(15f).id;
    }

    private void OnStartCallToAction()
    {
        _tweenId = LeanTween.rotateAround(button, button.transform.up, 30f, 0.1f).setLoopPingPong(-1).id;
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
            _buttonMaterial.SetColor("_OutlineColor", Color.Lerp(normalColor, emissiveColor, Mathf.PingPong(Time.time, 0.5f)));
            _buttonMaterial.SetFloat("_OutlineWidth", Mathf.Lerp(0.02f, 0.09f, Mathf.PingPong(Time.time, 0.5f)));
        }
        else if(button.gameObject.activeSelf)
        {
            _buttonMaterial.SetColor("_OutlineColor", Color.black);
            _buttonMaterial.SetFloat("_OutlineWidth", 0.07f);
        }
    }

    private void OnButtonClicked()
    {
        GameManager.instance.CurrentGameState = GameState.Tutorial;

        LeanTween.moveZ(button, button.transform.position.z + 0.3f, 0.5f).setEase(buttonEaseType);
        LeanTween.rotateY(door, 90f, 1f).setEase(doorEaseType);

        if (LeanTween.isTweening(_tweenId))
        {
            LeanTween.cancel(_tweenId);
        }

        text.SetActive(false);
    }
}