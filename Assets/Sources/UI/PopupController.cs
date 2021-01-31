using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{

    #region singleton
    public static PopupController instance { private set; get; }

    private void Awake()
    {
        // First destroy any existing instance of it
        if (instance != null)
        {
            Destroy(instance);
        }
        // Then reassign a proper one
        instance = this;
    }
    #endregion

    public Popup GameOverPopup;

}
