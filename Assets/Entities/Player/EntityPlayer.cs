using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityPlayer : Entity
{
    Animator anim;
    enum PlayerState { damage, dead, alive};
    PlayerState player_state;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        SetState("alive");
    }
    public override string GetState()
    {
        return player_state.ToString();
    }
    public override void SetState(string desiredState)
    {
        switch (desiredState)
        {
            case "alive":
                player_state = PlayerState.alive;
                break;
            case "dead":
                player_state = PlayerState.dead;
                break;
            case "damage":
                player_state = PlayerState.damage;
                break;
            default:
                throw new ArgumentException();
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        ProcessStates();
    }

    void ProcessStates()
    {
        switch (player_state)
        {
            case PlayerState.alive:
                anim.SetBool("Dead", false);
                break;
            case PlayerState.dead:
                anim.SetBool("Dead", true);
                Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
                break;
            case PlayerState.damage:
                anim.SetInteger("Hit", UnityEngine.Random.Range(0, 2));
                anim.SetTrigger("isHit");
                SetState("alive");
                break;
            default:
                throw new ArgumentException();
        }
    }

    public override void RecieveDamage(float damage)
    {
        SetState("damage");
        hitPoints -= damage;
    }

    protected override void Die()
    {
        if (player_state != PlayerState.dead)
        {
            SetState("dead");
        }
    }
}
