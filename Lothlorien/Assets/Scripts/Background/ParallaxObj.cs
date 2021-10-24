using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObj : MonoBehaviour
{
    public Sprite[] differentSprites;
    public int spriteNumber;
    public float parallaxSpeed = 1;
    public float ySpeed = 0;

    [HideInInspector] public float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    public void ChangeNextSprite()
    {
        if (differentSprites.Length != 0)
        {
            spriteNumber++;
            if(spriteNumber < differentSprites.Length)
            {
                GetComponent<SpriteRenderer>().sprite = differentSprites[spriteNumber];
            }
            else
            {
                spriteNumber = 0;
                GetComponent<SpriteRenderer>().sprite = differentSprites[spriteNumber];
            }
            
        }
    }
}
