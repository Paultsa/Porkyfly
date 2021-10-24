using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdgeArrows : MonoBehaviour
{
    Vector2 stageDimensions;
    Vector2 minStageDimensions;
    Vector2 outOfScreen;
    public GameObject posDisplay;
    bool displayArrow;
    List<GameObject> trackedObjects;
    Dictionary<GameObject, GameObject> arrows;
    
    [Header("11.SquirrelFlying, 12.SquirrelRunning")]
    [Header("9.SmallBirdBlue, 10.SmallBirdRed,")]
    [Header("7.RabbitBrown, 8.SleepingBear,")]
    [Header("4.Owl, 5.Beaver, 6.RabbitWhite,")]
    [Header("0.Frog, 1.Hawk, 2.Hedgehog, 3.BearCannon,")]
    [Tooltip("Frog, Hawk, Hedgehog, BearCannon, Owl, Beaver, RabbitWhite, RabbitBrown, SleepingBear, SmallBirdBlue, SmallBirdRed, SquirrelFlying, SquirrelRunning")]
    public GameObject[] AnimalIcons;
    GameObject arrowContainer;
    /*
        Frog,
        Hawk,
        Hedgehog,
        BearCannon,
        Owl,
        Beaver,
        RabbitWhite,
        RabbitBrown,
        SleepingBear,
        SmallBirdBlue,
        SmallBirdRed,
        SquirrelFlying,
        SquirrelRunning,


    */

    void Start()
    {
        outOfScreen = new Vector3(-100, -100, 1);
        arrowContainer = new GameObject();
        arrowContainer.name = "ArrowContainer";
        arrowContainer.transform.parent = null;
        //CreateScreenColliders();
        trackedObjects = new List<GameObject>();
        arrows = new Dictionary<GameObject, GameObject>();
    }

    // Update is called once per frame
    /*void Update()
    {
        
        //stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        //horzExtent = Camera.main.orthographicSize * -Screen.width / Screen.height + Camera.main.transform.position.x;
        //Debug.Log(stageDimensions + " " + minStageDimensions);

        //top.transform.position = new Vector2(minStageDimensions.x, stageDimensions.y);
        //left.transform.position = new Vector2()
    }*/

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0.0f, 0.0f, 0f);

        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }
    float arrowMargin = 0.2f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            //RaycastHit2D hit = Physics2D.Raycast(AsVector2(Camera.main.transform.position), AsVector2(collision.transform.position));
            //Debug.Log(hit.collider.gameObject + " " + hit.point);
            //Debug.DrawRay(AsVector2(Camera.main.transform.position), AsVector2(collision.transform.position));
            if (!collision.GetComponent<Renderer>().isVisible)
            {
                if (trackedObjects.Contains(collision.gameObject))
                {
                    /*if (collision.transform.position.x > stageDimensions.x || collision.transform.position.x < minStageDimensions.x)
                    {
                        float testX = collision.transform.position.x / stageDimensions.x;
                        float posX = collision.transform.position.x / testX;
                        float posY = collision.transform.position.y / testX;
                        Vector2 temp = new Vector2(posX, posY);
                        Debug.Log(temp + " AAAAAA " + collision.transform.position.x + " " + stageDimensions.x);
                    }*/
                    //Debug.DrawLine(AsVector2(collision.transform.position), AsVector2(Camera.main.transform.position));
                    //arrows[collision.gameObject].transform.position = new Vector3(Mathf.Clamp(hit.point.x, -stageDimensions.x, stageDimensions.x), Mathf.Clamp(hit.point.y, -stageDimensions.y, stageDimensions.y), 1);
                    arrows[collision.gameObject].transform.position = new Vector3(Mathf.Clamp(collision.transform.GetChild(1).position.x, minStageDimensions.x + arrowMargin, stageDimensions.x - arrowMargin), Mathf.Clamp(collision.transform.GetChild(1).position.y, minStageDimensions.y + arrowMargin, stageDimensions.y - arrowMargin), 1);
                    //arrows[collision.gameObject].transform.position = hit.point;
                    //arrows[collision.gameObject].transform.position = Vector2.ClampMagnitude(collision.transform.position, 1);
                    //arrows[collision.gameObject].transform.right = AsVector2(collision.transform.position) - AsVector2(Camera.main.transform.position);
                    arrows[collision.gameObject].transform.up = AsVector2(collision.transform.GetChild(1).position) - AsVector2(arrows[collision.gameObject].transform.position);
                    arrows[collision.gameObject].transform.GetChild(1).rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    //Debug.Log(collision.transform.childCount);
                }
                else
                {
                    trackedObjects.Add(collision.gameObject);
                    if (!arrows.ContainsKey(collision.gameObject))
                    {
                        arrows.Add(collision.gameObject, Instantiate(posDisplay, arrowContainer.transform));
                        Instantiate(AnimalIcons[collision.transform.GetComponent<Obstacle>().typeAnimal], arrows[collision.gameObject].transform);
                        arrows[collision.gameObject].transform.GetChild(1).localPosition = arrows[collision.gameObject].transform.GetChild(0).localPosition;
                    }
                }
            }
            else if (arrows.ContainsKey(collision.gameObject))
            {
                arrows[collision.gameObject].transform.position = outOfScreen;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if (trackedObjects.Contains(collision.gameObject))
            {
                trackedObjects.Remove(collision.gameObject);
                Destroy(arrows[collision.gameObject]);
                arrows.Remove(collision.gameObject);
            }

        }
    }
    public static Vector2 AsVector2(Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }


    /*
    GameObject top;
    GameObject bottom;
    GameObject left;
    GameObject right;

    /*void Awake()
    {
        top = new GameObject("Top");
        bottom = new GameObject("Bottom");
        left = new GameObject("Left");
        right = new GameObject("Right");
    }

    void CreateScreenColliders()
    {
        Vector3 bottomLeftScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 topRightScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        //// Create top collider
        BoxCollider2D collider = top.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(Mathf.Abs(bottomLeftScreenPoint.x - topRightScreenPoint.x), 0.1f, 0f);
        collider.offset = new Vector2(collider.size.x / 2f, collider.size.y / 2f);

        top.transform.position = new Vector3((bottomLeftScreenPoint.x - topRightScreenPoint.x) / 2f, topRightScreenPoint.y, 0f);


        // Create bottom collider
        collider = bottom.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(Mathf.Abs(bottomLeftScreenPoint.x - topRightScreenPoint.x), 0.1f, 0f);
        collider.offset = new Vector2(collider.size.x / 2f, collider.size.y / 2f);

        //** Bottom needs to account for collider size
        bottom.transform.position = new Vector3((bottomLeftScreenPoint.x - topRightScreenPoint.x) / 2f, bottomLeftScreenPoint.y - collider.size.y, 0f);


        // Create left collider
        collider = left.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(0.1f, Mathf.Abs(topRightScreenPoint.y - bottomLeftScreenPoint.y), 0f);
        collider.offset = new Vector2(collider.size.x / 2f, collider.size.y / 2f);

        //** Left needs to account for collider size
        left.transform.position = new Vector3(((bottomLeftScreenPoint.x - topRightScreenPoint.x) / 2f) - collider.size.x, bottomLeftScreenPoint.y, 0f);


        // Create right collider
        collider = right.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(0.1f, Mathf.Abs(topRightScreenPoint.y - bottomLeftScreenPoint.y), 0f);
        collider.offset = new Vector2(collider.size.x / 2f, collider.size.y / 2f);

        right.transform.position = new Vector3(topRightScreenPoint.x, bottomLeftScreenPoint.y, 0f);
    }
    */
}
