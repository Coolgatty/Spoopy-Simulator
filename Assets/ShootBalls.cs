using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBalls : MonoBehaviour
{
    private Animator anim;
    public Rigidbody spell01;
    public GameObject spine;
    public float speed = 25;
    private Camera cam;
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
            Rigidbody p = Instantiate(spell01, spine.transform.position, transform.rotation);
            p.GetComponent<life>().lifeTime = 100;
            p.velocity = (cam.transform.forward + new Vector3(0, 0.2f, 0)) * speed;
            anim.Play("CastSpell");
        }

    }
}
