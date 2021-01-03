using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    public float hitPoints;
    private EntityAI entityAI;
    enum State { };
    State state;
    public virtual void SetState(string state) { }
    public virtual string GetState() { return state.ToString(); }
    protected virtual void Start()
    {
        entityAI = GetComponent<EntityAI>();
    }

    protected virtual void Update()
    {
        if (hitPoints <= 0f)
        {
            Die();
        }
    }

    public virtual void RecieveDamage(float damage)
    {
        entityAI.SetState("damage");
        hitPoints -= damage;
    }

    protected virtual void Die()
    {
        entityAI.SetState("dead");
    }
}
