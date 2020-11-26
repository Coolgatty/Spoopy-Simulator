using System;
using UnityEngine;

public class SpiderAI : EntityAI
{
    Vector3 destination;
    [SerializeField]
    private float moveSpeed;
    enum State { idle, moving, jumping, dead, changingState, damage }
    State state;
    Animation anim;

    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        SetState("idle");
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
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
                Invoke("GetDestination", UnityEngine.Random.Range(2f, 4f));
                SetState("changingState");
                anim.clip = anim.GetClip("idle");
                break;
            case State.moving:
                MoveTowards(destination);
                anim.clip = anim.GetClip("run");
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
        SetState("moving");
    }

    private void MoveTowards(Vector3 movePos)
    {
        Vector3 moveVector = movePos - transform.position;
        moveVector.Normalize();
        transform.position += moveVector * Time.deltaTime * moveSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), 0.25f);

        float deltaDistance = (destination - transform.position).magnitude;
        if (deltaDistance < 0.5f)
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
            print(hit.point - transform.position);
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
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity);
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
            default:
                throw new ArgumentException();
        }
    }
    public override string GetState()
    {
        return state.ToString();
    }
}
