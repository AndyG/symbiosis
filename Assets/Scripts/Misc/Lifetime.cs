using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{

    [SerializeField]
    private float lifetime = 5f;

    float timeAlive = 0f;

    // Update is called once per frame
    void Update()
    {
        if (timeAlive >= lifetime) {
            Destroy(gameObject);
        }
        timeAlive += Time.deltaTime;
    }
}
