using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnHit : MonoBehaviour
{
    [SerializeField]
    private GameObject prototype;

    public void Spawn() {
        GameObject.Instantiate(prototype, transform.position, transform.rotation);
    }
}
