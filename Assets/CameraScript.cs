using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float rotationSpeed = 2;
    //[HideInInspector]
    public Transform target;
    float mouseX, mouseY, mouseM;

    public Vector3 offset;
    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 0.7f, -2f);
            offset = target.position - transform.position;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {   
        if (target == null)
        {
            Debug.Log("No target transform attached");
            return;
        }
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -90f, 60f);

        transform.position = target.position - (Quaternion.Euler(mouseY, mouseX, 0) * offset);

        transform.eulerAngles = new Vector3(mouseY, mouseX, 0.0f);

        mouseM = Input.GetAxis("Mouse ScrollWheel");
        offset += new Vector3(0, 0, -mouseM * 2);
        offset = new Vector3(offset.x, offset.y, Mathf.Clamp(offset.z, 0, 4));
    }
}
