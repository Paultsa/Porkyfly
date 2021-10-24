using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrolling : MonoBehaviour
{
    int textureIndex;
    public GameObject[] levels;
    private Vector2 screenBounds;
    public float choke;

    void Start()
    {
        levels = GameObject.FindGameObjectsWithTag("Background");
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        foreach (GameObject obj in levels)
        {
            loadChildObjects(obj);
        }
    }

    void loadChildObjects(GameObject obj)
    {
        
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 3 / objectWidth);
        GameObject clone = Instantiate(obj) as GameObject;
        textureIndex = clone.GetComponent<ParallaxObj>().differentSprites.Length;
        for (int i = -1; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector2(objectWidth * i, obj.transform.position.y);
            c.name = obj.name + i;
            if(Mathf.Abs(i) < textureIndex)
            {
                c.GetComponent<ParallaxObj>().spriteNumber = Mathf.Abs(i);
                c.GetComponent<SpriteRenderer>().sprite = c.GetComponent<ParallaxObj>().differentSprites[Mathf.Abs(i)];
            }
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void repositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if(children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if(transform.position.x + screenBounds.x * 3 > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector2(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y);
                firstChild.GetComponent<ParallaxObj>().ChangeNextSprite();
                if(firstChild.GetComponent<SpriteRenderer>().sprite == lastChild.GetComponent<SpriteRenderer>().sprite)
                {
                    firstChild.GetComponent<ParallaxObj>().ChangeNextSprite();
                }

            }
            else if(transform.position.x - screenBounds.x * 3 < firstChild.transform.position.x - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector2(firstChild.transform.position.x - halfObjectWidth * 2, lastChild.transform.position.y);
                
            }
        }
    }
    void LateUpdate()
    {
        foreach(GameObject obj in levels)
        {
            repositionChildObjects(obj);
        }
    }
}
