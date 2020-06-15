using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardBehaviour : MonoBehaviour // todo: create base class projectile/spell
{
    const float cooldown = 0.75f;
    public static bool onCooldown;

    private static float cooldownTimer = cooldown;
    public static float CooldownTimer // todo: should be in base class
    {
        get
        {
            return cooldownTimer;
        }
        set
        {
            if (onCooldown)
            {
                cooldownTimer = value;
            }
            else
            {
                cooldownTimer = cooldown;
            }
            if (value <= 0)
            {
                cooldownTimer = cooldown;
                onCooldown = false;
            }
        }
    }

    [SerializeField]
    private float minDamage;
    [SerializeField]
    private float maxDamage;

    Vector3 velocity;
    private void Start()
    {
        velocity = (transform.forward + Vector3.up * 0.1f);
        transform.rotation = Quaternion.LookRotation(velocity);
        onCooldown = true;
    }

    private void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Entity>() != null)
        {
            other.GetComponent<Entity>().RecieveDamage(Random.Range(minDamage, maxDamage));
        }
    }
}
