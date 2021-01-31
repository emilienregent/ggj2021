using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Wave;
    public TextMeshProUGUI Items;

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

    public void SetGameOverPopup(int PlayerScore, float PlayerTimer, string PlayerWave, int PlayerItem) {
        Score.text = "Score:"+PlayerScore.ToString();
        Timer.text = "Time:"+PlayerTimer.ToString()+"'";
        Wave.text = "Waves:"+PlayerWave;
        Items.text = "Items found:"+PlayerItem.ToString();
    }
}
