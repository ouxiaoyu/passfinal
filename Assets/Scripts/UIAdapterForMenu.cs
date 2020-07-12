using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAdapterForMenu : MonoBehaviour
{
    public GameObject title;
    public GameObject playButton;
    public GameObject quitButton;
    public GameObject tipButton;
    public Image bg;
    public GameObject canvas;

    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            title.transform.localScale = new Vector3(3f, 3f, 1f);
            title.transform.localPosition = new Vector3(10, 150, 0);
            playButton.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
            playButton.transform.localPosition = new Vector3(0, -20, 0);
            tipButton.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
            tipButton.transform.localPosition = new Vector3(0, -80, 0);
            quitButton.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
            quitButton.transform.localPosition = new Vector3(0, -140, 0);
            bg.sprite = Resources.Load<Sprite>("Sprites/background_ver");
            canvas.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            title.transform.localScale = new Vector3(5, 5, 1);
            title.transform.localPosition = new Vector3(-120, 50, 0);
            playButton.transform.localScale = new Vector3(3, 1, 1);
            playButton.transform.localPosition = new Vector3(260, 100, 0);
            tipButton.transform.localScale = new Vector3(3, 1, 1);
            tipButton.transform.localPosition = new Vector3(260, 0, 0);
            quitButton.transform.localScale = new Vector3(3, 1, 1);
            quitButton.transform.localPosition = new Vector3(260, -100, 0);
            bg.sprite = Resources.Load<Sprite>("Sprites/background_hor");
            canvas.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
    }
}
