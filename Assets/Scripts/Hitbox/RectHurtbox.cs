using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectHurtbox : MonoBehaviour
{
    [SerializeField]
    private float rangeX = 5;
    [SerializeField]
    private float rangeY = 3;

    void OnDrawGizmosSelected() {
        if (enabled) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(rangeX/2, rangeY/2));
        }
    }
}
