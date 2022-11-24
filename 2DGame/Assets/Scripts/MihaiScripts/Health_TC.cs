using UnityEngine;

public class Health_TC : MonoBehaviour
{

    public int maxHP = 100;
    public int currentHP;
    public bool IsAlive => currentHP > 0;


    public void Increment()
    {
        currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
        if (currentHP == maxHP)
        {
            //Vedem pe parcurs
        }
    }

    public void Increment(int count)
    {
        currentHP = Mathf.Clamp(currentHP + Mathf.Abs(count), 0, maxHP);
        if (currentHP == maxHP)
        {
            //nu merge aici respawnul
        }
    }

    /// <summary>
    /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
    /// current HP reaches 0.
    /// </summary>
    public void Decrement()
    {
        currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
        if (currentHP == 0)
        {
            //Vedem percurs :))
        }
    }
    public void Decrement(int count)
    {
        currentHP = Mathf.Clamp(currentHP - Mathf.Abs(count), 0, maxHP);
        if (currentHP == 0)
        {
            //Vedem percurs :))
        }
    }

    /// <summary>
    /// Decrement the HP of the entitiy until HP reaches 0.
    /// </summary>
    public void Die()
    {
        while (currentHP > 0) Decrement();
    }

    public void Respawn()
    {
        while (currentHP < maxHP) Increment();

        transform.position = new Vector2(0f, 1f);
        //dintr-un motiv sau altul, aspectul asta imi muta toate obiectele la intamplare, din fundal, dar revin dupa ce ruleaza codul de parallax
    }

    void Awake()
    {
        currentHP = maxHP;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rose" && this.currentHP != maxHP)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            while (currentHP < maxHP) Increment();
        }

        if (collision.gameObject.tag == "FreeFall")
            Respawn();
    }
}