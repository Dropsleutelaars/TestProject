using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
    private static int MAX_HEALTH = 3;
    private int health = MAX_HEALTH;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, MAX_HEALTH);
            if (health == 0)
                OnDeath();
        }
    }

    public virtual void TakeDamage(int amount)
    {
        Health -= amount;

    }

    protected void OnDeath()
    {

    }

    protected void Flip()
    {
            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
    }

    protected bool FacingRight
    {
        get
        {
            return transform.localScale.x > 0f;
        }
    }
}
