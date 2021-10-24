using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBoost : MonoBehaviour
{
    public static Animator pickupAnim;
    public static Image image;
    // Start is called before the first frame update
    void Start()
    {
        pickupAnim = GetComponent<Animator>();
    }


    public static void Pickup()
    {
        pickupAnim.SetTrigger("Pickup");
        //pickupAnim.ResetTrigger("Pickup");
    }
}
