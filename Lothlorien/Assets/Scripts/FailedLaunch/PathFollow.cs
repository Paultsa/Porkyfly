using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{

    PathNode[] nodes;
    public GameObject player;
    public GameObject manager;

    public float moveSpeed;
    public float quickSpeed;
    float originalSpeed;
    float timer;
    int currentNode;
    static Vector2 currentPositionHolder;
    Vector2 startPosition;
    Quaternion originalRotation;
    Vector3 originalCameraPosition;

    private void Awake()
    {
        originalRotation = player.transform.rotation;
        originalCameraPosition = Camera.main.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        nodes = GetComponentsInChildren<PathNode>();
        originalSpeed = moveSpeed;
        CheckNode();
    }

    private void OnEnable()
    {
        //currentPositionHolder = player.transform.position;
        manager.GetComponent<AnimationManager>().ChangeDefaultSprite();
        player.transform.rotation = originalRotation;
        startPosition = player.transform.position;
        Camera.main.GetComponent<CameraMovement>().ResetTracking();
        Camera.main.transform.position = originalCameraPosition;
        player.GetComponent<CircleCollider2D>().enabled = false;
        //player.GetComponent<PlayerTest>().minHeightActive = false;
        //Camera.main.GetComponent<AirBoost>().isAvailable = false;
    }

    void CheckNode()
    {
        if (currentNode < nodes.Length - 1)
        {

            timer = 0;
        }
        else
        {
            timer = 0;
            currentNode = 0;
            moveSpeed = originalSpeed;
            Camera.main.GetComponent<FingerSling>().enabled = true;
            gameObject.GetComponent<PathFollow>().enabled = false;
            player.GetComponent<PlayerTest>().gameManager.mainMenu.SetActive(true);
            player.GetComponent<CircleCollider2D>().enabled = true;
            //player.GetComponent<PlayerTest>().minHeightActive = true;
            //Camera.main.GetComponent<AirBoost>().isAvailable = true;
        }
        currentPositionHolder = nodes[currentNode].transform.position;
        //Debug.Log(currentPositionHolder);
        startPosition = player.transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * moveSpeed;

        if (AsVector2(player.transform.position) != currentPositionHolder)
        {
            //player.transform.position = Vector2.Lerp(player.transform.position, currentPositionHolder, timer);
            player.transform.position = Vector2.Lerp(startPosition, currentPositionHolder, timer);
        }
        else
        {
            if (currentNode < nodes.Length - 1)
            {
                moveSpeed = quickSpeed;
                currentNode++;
                //Debug.Log(currentNode);
                CheckNode();
            }
        }
    }

    public static Vector2 AsVector2(Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }
}
