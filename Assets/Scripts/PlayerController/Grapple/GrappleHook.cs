using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Ending")]
    [SerializeField]
    private ParticleSystem endParticles;
    [SerializeField]
    private GameObject endGlow;
    [SerializeField]
    private float speed = 7;
    [SerializeField]
    private LayerMask targets;

    private LineRenderer lineRenderer;
    private Vector3 direction = Vector3.right;

    private State state;
    private float distance = 0f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        endParticles = GameObject.Instantiate(endParticles);
        endParticles.Stop();

        endGlow = GameObject.Instantiate(endGlow);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.HITTING) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, targets);
            if (hit) {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                ConfigureEndingHit(hit);
            }
        }
        if (state == State.NONE) {
            lineRenderer.enabled = false;
        }
        if (state == State.EXTENDING) {
            distance = distance + speed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, targets);
            if (hit) {
                this.state = State.HITTING;
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                ConfigureEndingHit(hit);
            } else {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, this.transform.position + direction * distance);
                ConfigureEndingMiss();
            }
        }
        if (state == State.RETRACTING) {
            distance = distance - speed * Time.deltaTime;
            if (distance <= 0f) {
                this.state = State.NONE;
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, targets);
            if (hit) {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                ConfigureEndingHit(hit);
            } else {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, this.transform.position + (direction * distance));
                ConfigureEndingMiss();
            }
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

    private void ConfigureEndingMiss() {
        if (endParticles != null) {
            if (endParticles.isPlaying) {
                endParticles.Stop();
            }
        }

        if (endGlow != null) {
            endGlow.SetActive(false);
        }
    }

    public void Extend() {
        this.state = State.EXTENDING;
        lineRenderer.enabled = true;
    }

    public void Retract() {
        this.state = State.RETRACTING;
    }

    public void RetractInstant() {
        this.distance = 0f;
        ConfigureEndingMiss();
        this.state = State.NONE;
    }

    public Vector2 GetEndingPosition() => transform.position + direction * distance;

    public State GetState => state;

    public enum State {
        NONE,
        EXTENDING,
        RETRACTING,
        HITTING
    }
}
