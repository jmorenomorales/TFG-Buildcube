using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;
using Vuforia;

public class StickerCapture : MonoBehaviour
{
    public GameObject camera, worldSpacePlane;
    public RawImage rawImage;
    public RectTransform touchPanel;
    public GameObject togglePhotoMode;
    public Sprite togglePhotoModeOff;

    private RectTransform helper;

    private bool isFocus = false;
    private string shareSubject, shareMessage;
    private bool isProcessing = false;
    private string screenshotName;
    private Animator togglePhotoModeAnim;
    UIManager uiManager;

    private bool cameraMode = false;

    private void Start()
    {
        togglePhotoModeAnim = togglePhotoMode.GetComponent<Animator>();
        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        CreateDirectory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeImage();
    }

    private void ResetRectTransform()
    {
        touchPanel.offsetMin = new Vector2(0, 0);
        touchPanel.offsetMax = new Vector2(0, 0);
        touchPanel.localRotation = Quaternion.identity;
        touchPanel.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    public void TakeImage()
    {
        ResetRectTransform();
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        if (!worldSpacePlane.activeSelf) worldSpacePlane.SetActive(true);
        camera.SetActive(true);
        yield return new WaitForEndOfFrame();
        camera.SetActive(false);
        UIManager.Instance.setTogglePhotoModeBool(false);
        if (PlayerPrefs.GetInt("ISUSINGQR") == 1)    // Usa custom QR
            togglePhotoModeAnim.SetBool("PhotoModeOnQR", false);
        else
            togglePhotoModeAnim.SetBool("PhotoModeOnNoQR", false);
        togglePhotoMode.GetComponentInChildren<Button>().image.sprite = togglePhotoModeOff;
        yield return null;
    }

    public void ShareImage()
    {
        Debug.Log("He entrado en ShareImage()");
        screenshotName = "Screenshot_" + System.DateTime.Now.ToString("MM_dd_yyyy") + "_" + System.DateTime.Now.ToString("hh_mm_ss") + ".png";
        shareSubject = "I challenge you to beat my high score in Fire Block";
        shareMessage = "I challenge you to beat my high score in Fire Block. " +
        ". Get the Fire Block app from the link below. \nCheers\n" +
        "\nhttp://onelink.to/fireblock";

        ShareScreenshot();
    }

    private void ShareScreenshot()
    {
#if UNITY_ANDROID
        if (!isProcessing)
        {
            StartCoroutine(ShareScreenshotInAnroid());
        }
#else
		Debug.Log("No sharing set up for this platform.");
#endif
    }

#if UNITY_ANDROID
    public IEnumerator ShareScreenshotInAnroid()
    {
        isProcessing = true;
        // wait for graphics to render
        yield return new WaitForEndOfFrame();

        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        GameObject.Find("NewGUI").GetComponent<Canvas>().enabled = false;
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(screenshotName, 1);
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("NewGUI").GetComponent<Canvas>().enabled = true;

        if (!Application.isEditor)
        {
            //Create intent for action send
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            //create image URI to add it to the intent
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + screenShotPath);

            //put image and string extra
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);
            /*
            t1.text = "He entrado en las 3 ultimas instrucciones";
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            t1.text = "He pasado la primera chunga";
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
            currentActivity.Call("startActivity", chooser);*/

            string Path = GetAndroidInternalFilesDir() + "/DCIM/BuildCube/" + screenshotName;
            if (File.Exists(screenShotPath))
            {
                File.Move(screenShotPath, Path);
            }

            using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
            using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
            using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
            using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
            {
                jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { Path }, null, null);
            }

            Debug.Log("Por favor he funcionado");
        }

        //yield return new WaitUntil(() => isFocus);
        isProcessing = false;
        yield return new WaitUntil(() => isFocus);
    }

    public void CreateDirectory()
    {
        Directory.CreateDirectory(GetAndroidInternalFilesDir() + "/DCIM/BuildCube/");
    }

    public static string GetAndroidInternalFilesDir()
    {
        string[] potentialDirectories = new string[]
        {
        "/mnt/sdcard",
        "/sdcard",
        "/storage/sdcard0",
        "/storage/sdcard1"
        };

        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < potentialDirectories.Length; i++)
            {
                if (Directory.Exists(potentialDirectories[i]))
                {
                    return potentialDirectories[i];
                }
            }
        }
        return "";
    }
#endif
}