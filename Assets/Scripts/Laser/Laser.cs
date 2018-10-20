using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer lineRenderer;

    [Header("Ending")]
    [SerializeField]
    private ParticleSystem endParticles;
    [SerializeField]
    private GameObject endGlow;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        endParticles = GameObject.Instantiate(endParticles);
        endParticles.Stop();

        endGlow = GameObject.Instantiate(endGlow);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        if (hit) {
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, hit.point);
            ConfigureEndingHit(hit);
        } else {
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, this.transform.position + transform.up * 1000);
            ConfigueEndingMiss();
        }
    }

    private void ConfigureEndingHit(RaycastHit2D hit) {
        if (endParticles != null) {
            Vector2 particleDirection = Vector2.Reflect(transform.up, hit.normal);
            endParticles.transform.rotation = Quaternion.LookRotation(particleDirection, Vector3.up);
            endParticles.transform.position = hit.point;
            if (!endParticles.isPlaying) {
                endParticles.Play();
            }
        }

        if (endGlow != null) {
            endGlow.transform.position = hit.point;
            endGlow.SetActive(true);
        }
    }

    private void ConfigueEndingMiss() {
        if (endParticles != null) {
            if (endParticles.isPlaying) {
                endParticles.Stop();
            }
        }

        if (endGlow != null) {
            endGlow.SetActive(false);
        }
    }
}
