using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashExplosion : MonoBehaviour
{
    Vector2 direction;
    float torque;
    public int amount = 5;
    public float absorbDelay = 2f;
    public float absorbPause = 0.1f;
    public float absorbSpeed = 1f;
    public float forceMultiplier;
    public float minTorque;
    public float MaxTorque;
    public GameObject cashPrefab;
    public GameObject absorber;

    float absorbTimer;
    float absorbPauseTimer;
    bool absorbing;
    bool once;
    bool destroySelf;

    Vector3 positionVector;

    GameObject[] cashInstances;
    // Start is called before the first frame update
    void Start()
    {
        cashInstances = new GameObject[amount];
        absorbTimer = 0;
        absorbPauseTimer = 0;
        absorbing = false;
        once = false;
        destroySelf = true;
        positionVector = transform.position - absorber.transform.position;
        StartCashExplosion();
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = absorber.transform.position + positionVector;
        if (absorbTimer >= absorbDelay)
        {
            absorbing = true;
        }
        else if (absorbTimer <= absorbDelay)
        {
            absorbTimer += Time.deltaTime;
        }

        if (absorbing)
        {

            if (!once)
            {
                for (int i = 0; i < amount; i++)
                {
                    cashInstances[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                }
                    once = true;
            }

            if (absorbPauseTimer >= absorbPause)
            {
                destroySelf = true;
                for (int i = 0; i < amount; i++)
                {
                    if (cashInstances[i] != null)
                    {
                        destroySelf = false;
                        cashInstances[i].transform.position = Vector2.MoveTowards(cashInstances[i].transform.position, absorber.transform.position, absorbSpeed * Time.deltaTime);
                        if (cashInstances[i].transform.position == absorber.transform.position)
                        {
                            Destroy(cashInstances[i]);
                        }
                    }
                    if (destroySelf)
                        Destroy(gameObject);
                }
            }
            else if (absorbPauseTimer <= absorbPause)
            {
                absorbPauseTimer += Time.deltaTime;
            }
            //absorb stuff
        }
    }

    void StartCashExplosion()
    {
        for (int i = 0; i < amount; i++)
        {
            direction = Vector3.Normalize(Random.insideUnitCircle);
            torque = Random.Range(-1, 1);
            if (torque<0)
            {
                torque = Random.Range(-MaxTorque, -minTorque);
            }
            else
            {
                torque = Random.Range(minTorque, MaxTorque);
            }
            //Debug.Log(torque + " THE TORQUE");
            cashInstances[i] = Instantiate(cashPrefab, transform, false);
            cashInstances[i].GetComponent<Rigidbody2D>().AddForce(direction * forceMultiplier, ForceMode2D.Impulse);
            cashInstances[i].GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
            //Debug.Log(direction + " " + direction.magnitude + " THIS IS IT");
        }

    }
}
