using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBoostMetronome : MonoBehaviour
{
    float angle;
    int direction;
    Vector2 angleComparison = new Vector2(0, 1);

    public GameObject boostActiveIndicator;
    public SpriteRenderer leftFork;
    public SpriteRenderer rightFork;


    [Header("Metronome settings")]
    [Tooltip("Minimun and maximum angle the metronome can hit on each swing")]
    public float minMaxAngle;
    [Tooltip("How fast the metronome swings")]
    public float swingSpeed;
    [Tooltip("Angle of the metronome when the boost activates")]
    public float boostActivationAngle;
    // Start is called before the first frame update
    void Start()
    {
        direction = -1;
    }

    private void OnEnable()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, minMaxAngle);
        direction = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angle = Vector2.SignedAngle(angleComparison, transform.GetChild(0).GetChild(0).right);
        transform.GetChild(0).Rotate(0.0f, 0.0f, direction * swingSpeed * Time.deltaTime, Space.Self);

        if (angle >= minMaxAngle)
        {
            direction = -1;
        }
        else if (angle <= -minMaxAngle)
        {
            direction = 1;
        }

        if (-boostActivationAngle < angle && angle < boostActivationAngle)
        {
            //Debug.Log("THE PRICE IS RIGHT BITCH " + angle);
            FingerSling.throwBoostActive = true;
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            leftFork.color = new Color(255, 0, 0);
            rightFork.color = new Color(255, 0, 0);
            boostActiveIndicator.SetActive(true);
            //if (!GetComponent<AudioSource>().isPlaying)
                //GetComponent<AudioSource>().Play();
        }
        else
        {
            //Debug.Log("THE PRICE IS WRONG BITCH " + angle);
            FingerSling.throwBoostActive = false;
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            leftFork.color = new Color(255, 255, 255);
            rightFork.color = new Color(255, 255, 255);
            boostActiveIndicator.SetActive(false);
        }
    }
}
