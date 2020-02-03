using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    [SerializeField] [Tooltip("In seconds")] private float duration;

    private float timeOfCollection = 0;

    public bool Collected { get; set; } = false;
    public float Duration
    {
        get => duration;
        set => duration = value;
    }

    protected virtual void Update()
    {
        if (!Collected)
        {
            return;
        }

        if (timeOfCollection + Duration <= Time.time)
        {
            OnEndOfDuration();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timeOfCollection = Time.time;
        Collected = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        OnPickup(collision);
    }

    protected abstract void OnPickup(Collider2D collision);
    protected abstract void OnEndOfDuration();
}
