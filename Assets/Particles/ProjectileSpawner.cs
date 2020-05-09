using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    private Animator anim;
    public GameObject projectile;
    public GameObject spine;
    [SerializeField] float projectileSize = 1f;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnProjectile();
        }

    }

    private void SpawnProjectile()
    {
        GameObject p = Instantiate(projectile, spine.transform.position, cam.transform.rotation);
        p.transform.localScale = Vector3.one * projectileSize;
        anim.Play("CastSpell");
    }
}
