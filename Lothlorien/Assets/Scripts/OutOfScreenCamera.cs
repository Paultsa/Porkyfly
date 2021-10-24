using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfScreenCamera : MonoBehaviour
{
    Quaternion camRotation;
    [SerializeField] private GameObject player;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Camera outOfScreenCamera;
    Image arrow;
    GameObject mask;
    GameObject circle;

    // Start is called before the first frame update
    void Start()
    {
        arrow = GetComponent<Image>();
        mask = transform.GetChild(0).gameObject;
        circle = transform.GetChild(1).gameObject;
        camRotation = outOfScreenCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > cameraMovement.maxHeight + player.GetComponent<SpriteRenderer>().size.y)
        {
            arrow.enabled = true;
            mask.SetActive(true);
            circle.SetActive(true);
            outOfScreenCamera.enabled = true;
        }
        else
        {
            arrow.enabled = false;
            mask.SetActive(false);
            circle.SetActive(false);
            outOfScreenCamera.enabled = false;
        }
    }
    private void LateUpdate()
    {
        outOfScreenCamera.transform.rotation = camRotation;
    }

    Texture2D RTImage()
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = outOfScreenCamera.targetTexture;

        // Render the camera's view.
        outOfScreenCamera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(outOfScreenCamera.targetTexture.width, outOfScreenCamera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, outOfScreenCamera.targetTexture.width, outOfScreenCamera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
        //return Sprite.Create(image, new Rect(0,0,image.width,image.height), new Vector2(0.5f, 0.5f));
    }
}
