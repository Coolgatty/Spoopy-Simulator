using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour // todo: create base class projectile/spell
{
    const float cooldown = 1.25f;
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

    [SerializeField] float speed;
    [SerializeField] float fireRate; // todo: implement fireRate

    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionSize;

    [SerializeField]
    private float minDamage;
    [SerializeField]
    private float maxDamage;

    ParticleSystem ps;

    Vector3 velocity;
    Vector3 initPos;
    void Start()
    {
        initPos = transform.position;
        velocity = (transform.forward + Vector3.up * 0.1f) * speed; // direction * speed; direction should go in base class
        transform.rotation = Quaternion.LookRotation(velocity);
        ps = GetComponent<ParticleSystem>();
        onCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTravelled = (transform.position - initPos).magnitude;
        if(speed != 0)
        {
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            Debug.Log("No Speed");
        }

        if (distanceTravelled > 35)
        {
            Explode();
        }

        if (!ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag != "Player")
        {
            Explode();
        }
        if (other.GetComponent<Entity>() != null)
        {
            other.GetComponent<Entity>().RecieveDamage(Random.Range(minDamage, maxDamage));
        }
    }

    private void Explode()
    {
        if (explosion != null)
        {
            var vfx = Instantiate(explosion, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * explosionSize;
        }
        Destroy(gameObject);
    }
}
