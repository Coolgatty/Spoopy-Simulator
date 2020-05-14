using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject iceshard;

    [SerializeField] float fireballSize = 1f;
    [SerializeField] float iceShardSize = 1f;

    [SerializeField] private GameObject spine;

    private int currentSlot = 1;
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
            switch(currentSlot)
            {
                case 1:
                    SpawnFireball();
                    break;
                case 2:
                    SpawnIceShard();
                    break;
                default:
                    Debug.Log("No current slot. Something broke");
                    break;
            }
        }

    }

    private void SpawnFireball()
    {
        GameObject p = Instantiate(fireball, spine.transform.position, cam.transform.rotation);
        p.transform.localScale = Vector3.one * fireballSize;
        anim.Play("CastSpell");
    }

    private void SpawnIceShard()
    {
        GameObject p = Instantiate(iceshard, spine.transform.position, cam.transform.rotation);
        p.transform.localScale = Vector3.one * iceShardSize;
    }

    public void switchSlot(int slot)
    {
        currentSlot = slot;
    }
}
