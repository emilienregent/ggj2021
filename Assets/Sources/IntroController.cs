using TMPro;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public GameObject text = null;

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
    }

    private void OnButtonClicked()
    {
        GameManager.instance.CurrentGameState = GameState.Tutorial;

        LeanTween.moveZ(gameObject, transform.position.z + 0.5f, 0.5f).setEase(LeanTweenType.easeInBounce);
        text.SetActive(false);
    }
}