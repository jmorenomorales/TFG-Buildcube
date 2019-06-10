using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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

    private void Start()
    {
        togglePhotoModeOn = false;
        togglePhotoModeAnimator = GameObject.Find("PhotoMode").GetComponent<Animator>();
        togglePhotoModeButton = GameObject.Find("PhotoMode").GetComponentInChildren<Button>();
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    
    public void TogglePhotoMode()
    {
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

    public void ColorPanel()
    {
        if (colorPanel.activeSelf)
            colorPanel.SetActive(false);
        else
        {
            colorPanel.SetActive(true);
        }
    }

    public void onTogglePictureClick()
    {
        if (togglePictureMode.isOn)
        {
            togglePictureMode.isOn = true;
            foundation.SetActive(false);
        }
        else
        {
            togglePictureMode.isOn = false;
            foundation.SetActive(true);
        }
    }

    public void OnMainMenuClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
