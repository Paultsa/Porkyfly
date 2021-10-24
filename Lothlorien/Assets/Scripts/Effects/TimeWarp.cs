using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TimeWarp : MonoBehaviour
{

    [Header("Time warp settings")]
    [Tooltip("How many times is the time being slowed down")]
    public float slowMultiplier = 2f;
    [Tooltip("How fast the time changes")]
    public float timeWarpSpeed;
    public bool warping = false;
    public bool doneWarping = false;
    bool paused = false;
    float tempWarp;
    float prevTimeScale;
    [HideInInspector]
    public float originalSlowMultiplier;

    [Header("Camera zoom settings")]
    [Tooltip("How much the camera zooms out during slow motion. This helps with aiming and gives a nice visual. Transition duration is the same as time warp")]
    public float CameraZoomMultiplier;

    float originalCameraZoom;

    private void Awake()
    {
        Time.maximumDeltaTime = 1 / 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalCameraZoom = Camera.main.orthographicSize;
        tempWarp = Time.timeScale;
        slowMultiplier = 1 / slowMultiplier;
        prevTimeScale = 1;
        originalSlowMultiplier = slowMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("FPS: " + 1 / Time.unscaledDeltaTime);
        if (Input.GetKey("up"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetButtonDown("Jump"))
        {
            ToggoleSlowTime();
        }
        if (Input.GetButtonDown("Vertical"))
        {
            ToggolePause();
        }

        if (!paused)
        {
            //Debug.Log(doneWarping);
            if (warping && !doneWarping)
            {
                //tempWarp = Time.timeScale;
                tempWarp -= Time.unscaledDeltaTime / timeWarpSpeed;
                tempWarp = Mathf.Clamp(tempWarp, slowMultiplier, 1);
                Time.timeScale = tempWarp;

                Camera.main.orthographicSize += (Time.unscaledDeltaTime * (originalCameraZoom * CameraZoomMultiplier)) / timeWarpSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, originalCameraZoom, originalCameraZoom * CameraZoomMultiplier);

                if (tempWarp == slowMultiplier || tempWarp == 1)
                    doneWarping = true;
            }
            else if (!warping && !doneWarping)
            {
                //tempWarp = Time.timeScale;
                tempWarp += Time.unscaledDeltaTime / timeWarpSpeed;
                tempWarp = Mathf.Clamp(tempWarp, slowMultiplier, 1);
                Time.timeScale = tempWarp;

                Camera.main.orthographicSize -= (Time.unscaledDeltaTime * (originalCameraZoom * CameraZoomMultiplier)) / timeWarpSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, originalCameraZoom, originalCameraZoom * CameraZoomMultiplier);

                if (tempWarp == (slowMultiplier) || tempWarp == 1)
                    doneWarping = true;
            }

        }

        //Fixedtime changes automatically if framerate drops or climbs enough
        if (!paused && (Time.fixedDeltaTime < Time.deltaTime / 1.1f || Time.fixedDeltaTime > Time.deltaTime * 1.1f))
        {
            //Debug.Log("ONE " + Time.fixedDeltaTime + " " + Time.deltaTime);
            Time.fixedDeltaTime = Time.deltaTime;
            //Debug.Log("TWO " + Time.fixedDeltaTime + " " + Time.deltaTime);
        }

    }

    public bool ToggoleSlowTime()
    {
        if (!paused && doneWarping)
        {
            warping = !warping;
            doneWarping = false;
            if (warping)
                originalCameraZoom = Camera.main.orthographicSize;
        }
        return warping;
    }

    public bool ToggolePause()
    {
        paused = !paused;
        if (paused)
        {
            //Debug.Log("PAUSE");
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.unscaledDeltaTime;
            AirBoost.airBoost.isAvailable = false;
        }
        else if (!paused)
        {
            //Debug.Log("UNPAUSE");
            Time.timeScale = prevTimeScale;
            AirBoost.airBoost.isAvailable = true;
        }
        return paused;
    }
}
