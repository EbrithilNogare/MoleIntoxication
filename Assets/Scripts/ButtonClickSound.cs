using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();

        if (button != null )
        {
            button.onClick.AddListener(Play);
        }
    }

    private void Play()
    {
        SFXManager.Instance.PlayButton();
    }
}
