using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDirt : MonoBehaviour
{
    // Start is called before the first frame update
    float startPos;
    float currentScale;
    public float maxSize;
    public float startSize;
    public float growRate;
    /*void Start()
    {
        startPos = transform.position;
        transform.localScale = new Vector2(startSize, startSize);
        currentScale = startSize;
    }*/

    private void OnEnable()
    {
        startPos = GameManager.getDistance();
        transform.localScale = new Vector2(startSize, startSize);
        currentScale = startSize;
        Debug.Log("LOCAL SCALE " + transform.localScale);
    }
    // Update is called once per frame
    void Update()
    {
        currentScale = Mathf.Clamp(Mathf.Abs(startSize + ((GameManager.getDistance() - startPos) * growRate)), startSize, maxSize);
        transform.localScale = new Vector2(currentScale, currentScale);
        //Debug.Log("SCALE STUFF DIRT " + currentScale);
    }
}
