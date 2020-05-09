using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float fireRate;

    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionSize;

    Vector3 velocity;
    Vector3 initPos;
    void Start()
    {
        initPos = transform.position;
        velocity = (transform.forward + Vector3.up * 0.2f) * speed;
        transform.rotation = Quaternion.LookRotation(velocity);
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
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag != "Player")
        {
            Explode();
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
