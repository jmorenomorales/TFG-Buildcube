using System;
using System.Collections.Generic;
using System.Reflection;
using TouchScript.Hit;
using TouchScript.Pointers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pointer = TouchScript.Pointers.Pointer;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Variables
    public RectTransform colorMenu;
    public RectTransform actionMenu;
    public Sprite eraseDes, eraseSel, saveDes, saveSel, colorDes, colorSel;
    public Button eraseButton, saveButton, colorButton;
    public GameObject colorPanel, savePanel, foundation;
    public Toggle togglePictureMode;
    public Sprite photoModeOn, photoModeOff;
    public GameObject settingsPanel;

    private bool menuAnimating, areMenusShowing, togglePhotoModeOn;
    private float menuAnimationTransition;
    private float animationDuration = 0.2f;
    private Animator togglePhotoModeAnimator;
    private Button togglePhotoModeButton;

    public static UIManager Instance { set; get; }

    #endregion

    private void Start()
    {
        Instance = this;
        if (SceneManager.GetActiveScene().name == "GameDefault" || SceneManager.GetActiveScene().name == "GameCustomQR")
        {
            togglePhotoModeOn = false;
            togglePhotoModeAnimator = GameObject.Find("PhotoMode").GetComponent<Animator>();
            togglePhotoModeButton = GameObject.Find("PhotoMode").GetComponentInChildren<Button>();
        }
        togglePictureMode.onValueChanged.AddListener(delegate { ToggleQRValueChanged(togglePictureMode); });
    }

    private void ToggleQRValueChanged(Toggle togglePictureMode)
    {
        if (togglePictureMode.isOn)
            foundation.SetActive(true);
        else
            foundation.SetActive(false);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    
    public void setTogglePhotoModeBool(bool mode)
    {
        togglePhotoModeOn = mode;
    }

    public void TogglePhotoMode()
    {
        if (GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>())
        {
            GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>().enabled = false;
        }

        if (!togglePhotoModeOn) // Modo foto cerrado
        {
            if (PlayerPrefs.GetInt("ISUSINGQR") == 1)    // Usa custom QR
            {
                togglePhotoModeAnimator.SetBool("PhotoModeOnQR", true);
            }
            else
            {
                togglePhotoModeAnimator.SetBool("PhotoModeOnNoQR", true);
            }

            togglePhotoModeOn = true;
            togglePhotoModeButton.image.sprite = photoModeOn;
        }
        else // Modo foto abierto
        {
            if (PlayerPrefs.GetInt("ISUSINGQR") == 1)    // Usa custom QR
            {
                togglePhotoModeAnimator.SetBool("PhotoModeOnQR", false);
            }
            else
            {
                togglePhotoModeAnimator.SetBool("PhotoModeOnNoQR", false);
            }
            
            togglePhotoModeOn = false;
            togglePhotoModeButton.image.sprite = photoModeOff;
        }
    }
    
    public void ChangeImage(int buttonType)
    {
        switch (buttonType)
        {
            case 0: // Erase
                eraseButton.image.sprite = eraseSel;
                colorButton.image.sprite = colorDes;
                break;
            case 1: // Color
                if(colorButton.image.sprite != colorSel)
                {
                    eraseButton.image.sprite = eraseDes;
                    colorButton.image.sprite = colorSel;
                }
                break;
        }
    }

    public void OnDeveloperButtonClick(int whichUrl)
    {
        switch (whichUrl)
        {
            case 0:
                Application.OpenURL("https://twitter.com/kokebr_");
                break;
            case 1:
                Application.OpenURL("https://github.com/jmorenomorales");
                break;
        }
    }

    public void ColorPanel()
    {
        if (GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>())
        {
            GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>().enabled = false;
        }

        if (colorPanel.activeSelf)
            colorPanel.SetActive(false);
        else
        {
            colorPanel.SetActive(true);
        }
    }

    

    public void OnMainMenuClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
