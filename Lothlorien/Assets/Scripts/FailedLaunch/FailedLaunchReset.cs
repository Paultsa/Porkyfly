using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedLaunchReset : MonoBehaviour
{
    public GameObject thePath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("TEST");
        if (collision.CompareTag("Throwable"))
        {
            thePath.GetComponent<PathFollow>().enabled = true;
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
