using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidingText : MonoBehaviour
{
    public float TimeBeforeHide;

    public Text PanelText;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;   
    }

    private void OnEnable() {
        LeanTween.moveX(gameObject, 0, 1);

        Invoke("Hide", TimeBeforeHide);
    }

    void Hide() {
        LeanTween.moveX(gameObject, Screen.width + 20, 1);

        Invoke("Disable", 2);
    }

    private void Disable() {
        this.enabled = false;
    }
}
