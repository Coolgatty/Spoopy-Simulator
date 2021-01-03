using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpiderAI : EntityAI
{
    Vector3 destination;
    [SerializeField]
    private float moveSpeed;
    enum State { idle, moving, jumping, dead, changingState, damage, attack, attackEnd }
    State state;
    Animation anim;
    GameObject target = null;
    float attack_speed = 0.5f;
    float attack_cooldown = 0.0f;
    float attack_damage = 20.0f;

    private bool isGrounded;
    // Start is called before the first frame update
    private void Awake()
    {
        SetState("idle");
        anim = GetComponent<Animation>();
    }

    void Start()
    {
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack_cooldown > 0f)
        {
            attack_cooldown -= Time.deltaTime;
        }
        if (!isGrounded && state != State.dead)
        {
            SetState("jumping");
        }
        ProcessStates();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void ProcessStates()
    {
        switch (state)
        {
            case State.idle:
                SetState("changingState");
                bool target_found = GetTarget();
                if (!target_found)
                {
                    Invoke("GetDestination", UnityEngine.Random.Range(2f, 4f));
                }
                anim.clip = anim.GetClip("idle");
                break;
            case State.changingState:
                anim.clip = anim.GetClip("idle");
                GetTarget();
                break;
            case State.moving:
                MoveTowards(destination);
                anim.clip = anim.GetClip("run");
                GetTarget();
                break;
            case State.jumping:
                MoveTowards(destination);
                anim.clip = anim.GetClip("jump");
                if (isGrounded)
                {
                    SetState("moving");
                }
                break;
            case State.dead:
                if (!anim.isPlaying && anim.clip.ToString().Contains("death"))
                {
                    Destroy(gameObject);
                }
                else if (!anim.clip.ToString().Contains("death"))
                {
                    int n = UnityEngine.Random.Range(0, 2);
                    if (n == 0)
                    {
                        anim.clip = anim.GetClip("death1");
                    }
                    else if (n == 1)
                    {
                        anim.clip = anim.GetClip("death2");
                    }
                }
                break;
            case State.damage:
                if (!anim.clip.ToString().Contains("hit"))
                {
                    int n = UnityEngine.Random.Range(0, 2);
                    if (n == 0)
                    {
                        anim.clip = anim.GetClip("hit1");
                    }
                    else if (n == 1)
                    {
                        anim.clip = anim.GetClip("hit2");
                    }
                }
                if (!anim.isPlaying && anim.clip.ToString().Contains("hit"))
                {
                    SetState("idle");
                }
                break;
            case State.attack:
                if (target == null)
                {
                    SetState("idle");
                    break;
                }
                if (Vector3.Magnitude(target.transform.position - transform.position) > 10.0f)
                {
                    SetState("idle");
                }
                else if ((target.transform.position - transform.position).magnitude < 1.7f)
                {
                    if (attack_cooldown <= 0f)
                    {
                        target.GetComponent<Entity>().RecieveDamage(attack_damage);
                        attack_cooldown = 1 / attack_speed;
                    }
                    if (!anim.clip.ToString().Contains("attack"))
                    {
                        int n = UnityEngine.Random.Range(0, 2);
                        if (n == 0)
                        {
                            anim.clip = anim.GetClip("attack1");
                        }
                        else if (n == 1)
                        {
                            anim.clip = anim.GetClip("attack2");
                        }
                    }
                    if (!anim.isPlaying && anim.clip.ToString().Contains("attack"))
                    {
                        SetState("idle");
                    }
                }
                else
                {
                    MoveTowards(target.transform.position);
                    anim.clip = anim.GetClip("run");
                }
                break;
        }
        anim.Play();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();
    }

    private void GetDestination()
    {
        destination = GetRandomPosition(10f);
        target = null;
        SetState("moving");
    }

    private bool GetTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10.0f);
        GameObject target_object;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                target_object = colliders[i].gameObject;
                destination = target_object.transform.position;
                target = target_object;
                SetState("attacking");
                return true;
            }
        }
        return false;
    }

    private void MoveTowards(Vector3 movePos)
    {
        Vector3 moveVector = movePos - transform.position;
        moveVector.Normalize();
        transform.position += moveVector * Time.deltaTime * moveSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), 0.25f);

        float deltaDistance = (destination - transform.position).magnitude;
        if (deltaDistance < 0.1f)
        {
            SetState("idle");
        }
    }

    private Vector3 GetRandomPosition(float radius)
    {
        RaycastHit hit;
        Vector3 position = transform.position + new Vector3(UnityEngine.Random.Range(-radius, radius), 0, UnityEngine.Random.Range(-radius, radius));
        if (Physics.Raycast(position + new Vector3(0, 100.0f, 0), Vector3.down, out hit, 200.0f))
        {
            return hit.point;
        }
        else
        {
            Debug.Log("there seems to be no ground at this position");
            return transform.position;
        }
    }

    private bool CheckIsGrounded()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, 3);
        if (hit.distance < 0.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void SetState(string desiredState)
    {
        switch (desiredState)
        {
            case "idle":
                state = State.idle;
                break;
            case "moving":
                state = State.moving;
                break;
            case "jumping":
                state = State.jumping;
                break;
            case "dead":
                state = State.dead;
                break;
            case "changingState":
                state = State.changingState;
                break;
            case "damage":
                state = State.damage;
                break;
            case "attacking":
                state = State.attack;
                break;
            case "attackEnd":
                state = State.attackEnd;
                break;
            default:
                throw new ArgumentException();
        }
    }
    public override string GetState()
    {
        return state.ToString();
    }
}
