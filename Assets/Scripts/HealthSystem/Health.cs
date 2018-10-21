using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void OnHealthChanged(int health);
    public OnHealthChanged OnHealthChangedEvent;

    public delegate void OnDied();
    public OnDied OnDiedEvent;

    [SerializeField]
    private int hitPoints = 1;

    public void add(int change) {
        this.hitPoints += change;
        if (OnHealthChangedEvent != null) {
            OnHealthChangedEvent(this.hitPoints);
        }
        if (this.hitPoints < 0 && OnDiedEvent != null) {
            OnDiedEvent();
        }
    }

    public void subtract(int change) {
        add(-change);
    }
}
