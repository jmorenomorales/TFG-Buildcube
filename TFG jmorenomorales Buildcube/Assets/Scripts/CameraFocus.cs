using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraFocus : MonoBehaviour
{
    void Start()
    {
        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);
    }

    private void OnVuforiaStarted()
    {
        SetAutofocus();
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set autofocus mode again when app is resumed
            SetAutofocus();
        }
    }

    public void SetAutofocus()
    {
        bool focusModeSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

        Debug.Log(focusModeSet ? "Focus mode set to autofocus" : "Failed to set focus mode (unsupported mode)");
    }
}
