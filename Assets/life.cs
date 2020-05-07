using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= 1;
        if (lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
