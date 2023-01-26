using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class AndroidCamera : MonoBehaviour
{
    private WebCamTexture cam;
    private bool toggleRequest = false;
    private WebCamDevice[] devices;

    //variables bound to UI elements. Set these in the inspector
    public RawImage background;
    public AspectRatioFitter fit;
    public int current = 0;
    public Button button;


    // Start is called before the first frame update
    void Start()
    {
        //Make sure the camera is available
        devices = WebCamTexture.devices;

        Screen.SetResolution(640, 480, true);
        if (devices.Length == 0 || devices == null)
        {
            Debug.Log("No camera detected");
        }
        else
        { 
            // If a camera is avaialable play the feed and fit the feed to the screen
            cam = new WebCamTexture(devices[current].name);
            cam.Play();
            background.texture = cam;

            float ratio = (float)cam.width / (float)cam.height;
            fit.aspectRatio = ratio;

            float scaleY = cam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -cam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        }
        // Make sure that UI elements are selectable and that one of them is selected to start.
        button.Select();
    }

    // Update is called once per frame
    void Update()
    {
        //Select the next camera checking if the last camera is selected. If so loop back to the first. 
        if (toggleRequest)
        {
            if (current == devices.Length - 1)
            {
                current = 0;
            }
            else
            {
                current++;
            }
            // Stop the feed before the camera is changed.
            cam.Stop();
            cam = new WebCamTexture(devices[current].name);
            cam.Play();
            background.texture = cam;
            toggleRequest = false;
        }
    }

    public void toggle()
    {
        toggleRequest = true;
    }
}
