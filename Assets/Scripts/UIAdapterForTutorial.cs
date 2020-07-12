using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAdapterForTutorial : MonoBehaviour
{
    public GameObject firstPanel;
    public GameObject leftButton;
    public GameObject rightButton; 
    public GameObject playButton;
    public GameObject backButton;
    public GameObject perfect;
    public GameObject canvas;
    public GameObject final;
    public GameObject finalbg;




    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            leftButton.transform.localScale = new Vector3(1, 1, 1);
            leftButton.transform.localPosition = new Vector3(-60, -190, 0);
            rightButton.transform.localScale = new Vector3(1, 1, 1);
            rightButton.transform.localPosition = new Vector3(60, -190, 0);
            playButton.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
            playButton.transform.localPosition = new Vector3(0, -130, 0);
            backButton.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
            backButton.transform.localPosition = new Vector3(0, -190, 0);
            firstPanel.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            firstPanel.transform.localPosition = new Vector3(0, 180, 0);
            final.transform.localScale = new Vector3(2.5f, 3f, 0.5f);
            final.transform.localPosition = new Vector3(0, 70, 0);
            finalbg.transform.localScale = new Vector3(2.5f, 2.7f, 1f);
            finalbg.transform.localPosition = new Vector3(0, 30, 0);
            perfect.transform.localScale = new Vector3(2, 1, 1);
            canvas.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        }
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            leftButton.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            leftButton.transform.localPosition = new Vector3(-300, -190, 0);
            rightButton.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            rightButton.transform.localPosition = new Vector3(300, -190, 0);
            playButton.transform.localScale = new Vector3(3f, 1f, 1f);
            playButton.transform.localPosition = new Vector3(-300, -190, 0);
            backButton.transform.localScale = new Vector3(3f, 1f, 1f);
            backButton.transform.localPosition = new Vector3(300, -190, 0);
            firstPanel.transform.localScale = new Vector3(1, 1, 1);
            firstPanel.transform.localPosition = new Vector3(0, 180, 0);
            final.transform.localScale = new Vector3(5f, 5.5f, 1f);
            final.transform.localPosition = new Vector3(0, 20, 0);
            finalbg.transform.localScale = new Vector3(5f, 5f, 1f);
            finalbg.transform.localPosition = new Vector3(0, -55, 0);
            perfect.transform.localScale = new Vector3(4, 2, 1);
            canvas.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
    }


}
