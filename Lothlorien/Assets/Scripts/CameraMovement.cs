using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Quaternion camRotation;
    public BackgroundManager backgroundManager;
    Vector2 lastPos;
    Vector2 currentPos;
    public Vector2 cameraSpeed;
    public GameObject trackedObject;
    bool isTracking = false;
    float accelerationX = 0f;
    float accelerationY = 0f;
    Vector3 tempVector;
    Vector3 tempPositionVector;
    float originalZoom;
    float zoomTimer;
    float zoomHeight;
    Vector2 playerSpeed;
    bool startFollowY;

    [Tooltip("X-axis is the height of the player, Y-axis is how far the camera zooms out")]
    public AnimationCurve cameraZoomHeight = AnimationCurve.Linear(0, 0, 100, 3);
    [Tooltip("How far the camera zooms out at max because of the height. Keep this the same as the max value on the X-axis on the curve above")]
    public float maxHeightZoom;
    [Tooltip("The height after which the camera starts zooming. This value can be found on the curve above")]
    public float zoomThresholdHeight;

    float ortograpicSum;

    [Tooltip("X-axis is the speed of the player, Y-axis is how far the camera zooms out")]
    public AnimationCurve cameraZoom = AnimationCurve.Linear(0, 0, 100, 3);
    [Tooltip("How far the camera zooms out at max because of the speed. Keep this the same as the max value on the X-axis on the curve above")]
    public float maxSpeedZoom;
    [Tooltip("The speed after which the camera starts zooming. This value can be found on the curve above")]
    public float zoomThreshold;
    [Tooltip("Minimum size for the camera window")]
    public float minZoom = 0f;
    [Tooltip("Maximum size for the camera window")]
    public float maxZoom = 0f;
    //[Tooltip("Minimum height for the camera position")]
    float minHeight;
    [Tooltip("Maximum height for the camera position")]
    public float maxHeight;
    [Tooltip("The point after which the camera starts following the object on x-axis")]
    public float followThresholdX = 0f;
    [Tooltip("The point after which the camera starts following the object on Y-axis")]
    public float followThresholdY = 0f;

    bool xFollow = false;
    bool yFollow = false;
    bool isSet = false;

    void Awake()
    {
        camRotation = Camera.main.transform.rotation;
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalZoom = Camera.main.orthographicSize;
        zoomTimer = zoomThreshold;
        tempVector = transform.position;
        tempPositionVector = transform.position;
        minHeight = transform.position.y;
        startFollowY = false;
    }

    private void Update()
    {
        if (isTracking && gameObject.GetComponent<TimeWarp>().doneWarping && !gameObject.GetComponent<TimeWarp>().warping)
        {
            playerSpeed.y = trackedObject.GetComponent<Rigidbody2D>().velocity.y;
            playerSpeed.x = -backgroundManager.GetComponent<BackgroundManager>().xSpeed;
            //Debug.Log(playerSpeed.x + " SPEED");
            if (zoomTimer + 10 < playerSpeed.x)
            {
                zoomTimer += 50 * Time.unscaledDeltaTime;
            }
            else if (zoomTimer - 10 > playerSpeed.x)
            {
                zoomTimer -= 50 * Time.unscaledDeltaTime;
            }

            if (zoomHeight + 5 < trackedObject.transform.position.y)
            {
                zoomHeight += 50 * Time.unscaledDeltaTime;
            }
            else if (zoomHeight - 5 > trackedObject.transform.position.y)
            {
                zoomHeight -= 50 * Time.unscaledDeltaTime;
            }
            ortograpicSum = cameraZoom.Evaluate(zoomTimer) + cameraZoomHeight.Evaluate(zoomHeight);
            //ortograpicSum = cameraZoomHeight.Evaluate(zoomHeight);
            zoomTimer = Mathf.Clamp(zoomTimer, originalZoom, maxSpeedZoom);
            Camera.main.orthographicSize = ortograpicSum;
        }
        //Debug.Log(playerSpeed.magnitude + " " + zoomTimer + " " + cameraZoom.Evaluate(zoomTimer));
        //Debug.Log(trackedObject.transform.position.y + " " + zoomHeight + " " + cameraZoomHeight.Evaluate(zoomHeight));

    }
    // Update is called once per frame
    void LateUpdate()
    {
        float camWidthOffset = ((Camera.main.orthographicSize * 2) * Camera.main.aspect) / 6;
        //Debug.Log(camWidth);
        Camera.main.transform.rotation = camRotation;

        if (isTracking && trackedObject.transform.position.x > followThresholdX && !isSet)
        {
            backgroundManager.moving = true;
            backgroundManager.movingY = true;
            tempVector.x = Vector3.Lerp(transform.position, new Vector3(trackedObject.transform.position.x + camWidthOffset, 0, transform.position.z), accelerationX).x;
            tempVector.y = Vector3.Lerp(transform.position, trackedObject.transform.position, accelerationY).y;
            xFollow = true;
            yFollow = true;
            if (accelerationX < 0.9f)
                accelerationX += 3 * Time.unscaledDeltaTime;
            if (accelerationY < 0.9f)
                accelerationY += 3 * Time.unscaledDeltaTime;
        }
        if (isTracking && trackedObject.transform.position.y > followThresholdY && !isSet)
        {
            startFollowY = true;
        }
        if (startFollowY)
        {
            backgroundManager.movingY = true;
            tempVector.y = Vector3.Lerp(transform.position, trackedObject.transform.position, accelerationY).y;
            yFollow = true;
            if (accelerationY < 0.9f)
                accelerationY += 3 * Time.unscaledDeltaTime;
        }

        if (isTracking && (xFollow || yFollow) && !isSet)
        {
            //Debug.Log(xFollow + " " + yFollow);
            if (!transform.IsChildOf(trackedObject.transform))
                transform.SetParent(trackedObject.transform);
            tempVector.y = Mathf.Clamp(tempVector.y, minHeight, maxHeight);
            //tempVector.x = tempVector.x + 1;
            transform.position = tempVector;
        }
        if (accelerationX > 0.9f && accelerationY > 0.9f && !isSet)
        {
            isSet = true;
        }

        if (isSet)
        {
            tempVector.x = trackedObject.transform.position.x + camWidthOffset;
            tempVector.y = trackedObject.transform.position.y;
            tempVector.y = Mathf.Clamp(tempVector.y, minHeight, maxHeight);
            transform.position = tempVector;

        }
        //Debug.Log(Camera.main.orthographicSize - 0.8f);
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

    }


    public void StartTracking()
    {
        isTracking = true;
        //backgroundManager.moving = true;
    }

    public void ResetTracking()
    {
        isTracking = false;
        startFollowY = false;
        xFollow = false;
        yFollow = false;
        accelerationX = 0;
        accelerationY = 0;
        backgroundManager.moving = false;
        backgroundManager.movingY = false;
        isSet = false;
        transform.parent = null;
        //backgroundManager.moving = true;
    }
}
