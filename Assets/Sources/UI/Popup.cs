using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour
{
    public void Display()
    {
        gameObject.LeanScale(Vector3.zero, 0f);
        gameObject.SetActive(true);
        gameObject.LeanScale(new Vector3(1, 1, 1), 0.25f);
    }

    public void Hide()
    {
        gameObject.LeanScale(Vector3.zero, 0.25f).setOnComplete(Disable);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
