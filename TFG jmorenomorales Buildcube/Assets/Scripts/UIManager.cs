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
    public GameObject colorPanel, savePanel;

    private bool menuAnimating;
    private bool areMenusShowing;
    private float menuAnimationTransition;
    private float animationDuration = 0.2f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            OnTheOneButtonClick();

        if (menuAnimating)
        {
            if (areMenusShowing)
            {
                menuAnimationTransition += Time.deltaTime * (1-animationDuration);
                if (menuAnimationTransition >= 1)
                {
                    menuAnimationTransition = 1;
                    menuAnimating = false;
                }
            }
            else
            {
                menuAnimationTransition -= Time.deltaTime * (1 - animationDuration);
                if (menuAnimationTransition <= 0)
                {
                    menuAnimationTransition = 0;
                    menuAnimating = false;
                }
            }

            colorMenu.anchoredPosition = Vector2.Lerp(new Vector2(0, 1000), new Vector2(0, -250), menuAnimationTransition);
            actionMenu.anchoredPosition = Vector2.Lerp(new Vector2(-750,0), new Vector2(250, 0), menuAnimationTransition);
        }
    }

    public void OnTheOneButtonClick()
    {
        areMenusShowing = !areMenusShowing;
        PlayMenuAnimation();
    }

    private void PlayMenuAnimation()
    {
        menuAnimating = true;
    }

    public void ChangeImage(int buttonType)
    {
        switch(buttonType)
        {
            case 0: // Erase
                /*if (eraseButton.image.sprite == eraseSel)
                    eraseButton.image.sprite = eraseDes;
                else
                {
                    eraseButton.image.sprite = eraseSel;
                    saveButton.image.sprite = saveDes;
                    colorButton.image.sprite = colorDes;
                }*/
                break;
            case 1: // Save
                if (saveButton.image.sprite == saveSel)
                    saveButton.image.sprite = saveDes;
                else
                {
                    saveButton.image.sprite = saveSel;
                    eraseButton.image.sprite = eraseDes;
                    colorButton.image.sprite = colorDes;
                }
                break;
            case 2: // Color
                if (colorButton.image.sprite == colorSel)
                    colorButton.image.sprite = colorDes;
                else
                {
                    colorButton.image.sprite = colorSel;
                    //eraseButton.image.sprite = eraseDes;
                    saveButton.image.sprite = saveDes;
                }
                break;
            case 3:
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
            savePanel.SetActive(false);
        }

    }

}
