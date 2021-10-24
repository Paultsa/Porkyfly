using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    bool doneOnce = false;
    public PlayerTest playerScript;
    public bool outOfBounds = false;
    public bool outOfStartCamera = false;
    public bool moving = false;
    public bool movingY = false;
    private float cameraOrigHorz;
    private float cameraOrigVert;
    private Vector2 origScreenBounds;
    public Vector2 cameraOrigPosition;
    float t = 0;
    public float cameraTimeToPlayer;
    Vector2 startPos;
    Vector2 target;
    public GameObject startFrame;
    public float xSpeed;
    public float shotForce;
    public GameObject player;
    private Rigidbody2D playerRB;
    public GameObject[] background;


    void Start()
    {
        origScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        cameraOrigPosition = Camera.main.transform.position;
        cameraOrigVert = Camera.main.orthographicSize;
        cameraOrigHorz = Camera.main.orthographicSize * Screen.width / Screen.height * 2;
        background = GameObject.FindGameObjectsWithTag("Background");
        playerRB = player.GetComponent<Rigidbody2D>();
        startPos = Camera.main.transform.position;
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;
        Sprite s = startFrame.GetComponent<SpriteRenderer>().sprite;
        float unitWidth = s.textureRect.width / s.pixelsPerUnit;
        float unitHeight = s.textureRect.height / s.pixelsPerUnit;
        startFrame.transform.localScale = new Vector3(width / unitWidth, height / unitHeight);
        startFrame.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, startFrame.transform.position.z);


    }

    private void LateUpdate()
    {
        //Debug.Log(xSpeed);
        if (movingY)
        {
            foreach (GameObject obj in background)
            {

                if (obj.GetComponent<ParallaxObj>() != null)
                {
                    obj.transform.position = new Vector2(obj.transform.position.x, obj.GetComponent<ParallaxObj>().startY + ((Camera.main.transform.position.y - cameraOrigPosition.y) * obj.GetComponent<ParallaxObj>().ySpeed));
                }
            }
        }
    }
    void FixedUpdate()
    {

        target = player.transform.position;
        background = GameObject.FindGameObjectsWithTag("Background");

        if (moving)
        {

            foreach (GameObject obj in background)
            {

                if (obj.GetComponent<ParallaxObj>() != null)
                {
                    if (outOfBounds)
                    {
                        obj.transform.Translate(new Vector2(xSpeed * Time.deltaTime * (obj.GetComponent<ParallaxObj>().parallaxSpeed * 0.5f), 0));
                        
                        if (!doneOnce)
                        {
                            Camera.main.GetComponent<AirBoost>().isAvailable = true;
                            doneOnce = true;
                        }
                    }
                    else
                    {
                        obj.transform.Translate(new Vector2((playerRB.velocity.x * Time.deltaTime / 2) - playerRB.velocity.x * Time.deltaTime * (obj.GetComponent<ParallaxObj>().parallaxSpeed * 0.5f), 0));
                    }
                    //obj.transform.position = new Vector2(obj.transform.position.x, obj.GetComponent<ParallaxObj>().startY + ((Camera.main.transform.position.y - cameraOrigPosition.y) * obj.GetComponent<ParallaxObj>().ySpeed));
                }
                else
                {
                    obj.transform.Translate(new Vector2(xSpeed, 0));
                }
                if (player.transform.position.x >= startFrame.GetComponent<SpriteRenderer>().bounds.extents.x * 2)
                {
                    outOfBounds = true;
                    if (playerRB.velocity.x != 0)
                    {
                        if (!playerScript.stop)
                        {

                            xSpeed += -playerRB.velocity.x;
                        }

                        obj.transform.Translate(new Vector2(xSpeed * Time.deltaTime * (obj.GetComponent<ParallaxObj>().parallaxSpeed * 0.5f), 0));
                    }
                    playerRB.velocity = new Vector2(0, playerRB.velocity.y);
                }
            }
        }
        if (player.transform.position.x >= origScreenBounds.x || player.transform.position.y >= origScreenBounds.y)
        {
            //Debug.Log("Out of start camera: " + player.transform.position);
            outOfStartCamera = true;
        }
        else
        {
            outOfStartCamera = false;
        }
        //}
    }


}
