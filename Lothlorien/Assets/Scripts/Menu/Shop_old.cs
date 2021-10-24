using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_old : MonoBehaviour
{
    private Vector2 originalOffsetMax;
    private Vector2 originalOffsetMin;
    public GameObject shopScrollable;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = shopScrollable.GetComponent<RectTransform>();
        originalOffsetMax = rectTransform.offsetMax;
        originalOffsetMin = rectTransform.offsetMin;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Scrollable RectTrans offset min: " + rectTransform.offsetMin.x);
        Debug.Log("Main offset min: " + GetComponent<RectTransform>().offsetMax.x);
        if (rectTransform.offsetMin.x > 0)
        {
            rectTransform.offsetMin = new Vector2(0, originalOffsetMin.y);
            rectTransform.offsetMax = new Vector2(originalOffsetMax.x-originalOffsetMin.x, rectTransform.offsetMax.y);
        }
        else if (rectTransform.offsetMax.x < GetComponent<RectTransform>().offsetMax.x)
        {
            rectTransform.offsetMax = new Vector2(GetComponent<RectTransform>().offsetMax.x, rectTransform.offsetMax.y);
            rectTransform.offsetMin = new Vector2(originalOffsetMin.x-originalOffsetMax.x, rectTransform.offsetMin.y);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase==TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            Vector2 currentPos = shopScrollable.transform.position;
            shopScrollable.transform.Translate(touchDelta.x, 0, 0);
            

            
        }
    }
}
