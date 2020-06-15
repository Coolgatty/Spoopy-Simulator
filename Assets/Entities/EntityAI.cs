using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAI : MonoBehaviour
{
    enum State { };
    //todo: move methods from SpiderAI to base class. Also refactor as necessary.
    public abstract void SetState(string state);
    public abstract string GetState();
}
