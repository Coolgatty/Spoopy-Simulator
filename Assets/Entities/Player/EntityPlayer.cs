using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : Entity
{
    Animator anim;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (anim.GetBool("Dead") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }

    protected override void Die()
    {
        anim.SetBool("Dead", true);
    }
}
