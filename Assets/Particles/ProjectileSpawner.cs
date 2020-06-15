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
        if (!anim.GetBool("Dead"))
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                switch(currentSlot)
                {
                    case 1:
                        if (!FireballBehaviour.onCooldown)
                        {
                            CastSpell(ref fireball, fireballSize);
                        }
                        break;
                    case 2:
                        if (!IceShardBehaviour.onCooldown)
                        {
                            CastSpell(ref iceshard, iceShardSize);
                        }
                        break;
                    default:
                        Debug.Log("No current slot. Something broke");
                        break;
                }
            }
        }
        FireballBehaviour.CooldownTimer -= Time.deltaTime;
        IceShardBehaviour.CooldownTimer -= Time.deltaTime;

    }

    private void CastSpell(ref GameObject spell, float size)
    {
        GameObject p = Instantiate(spell, spine.transform.position + cam.transform.forward * 1.2f, cam.transform.rotation);
        p.transform.localScale = Vector3.one * size;
        anim.Play("CastSpell");
    }

    public void switchSlot(int slot)
    {
        currentSlot = slot;
    }
}
