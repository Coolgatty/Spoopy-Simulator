using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update

    public float rotationSpeed = 2;
    public Transform Target, Player;
    float mouseX, mouseY, mouseM;

    private Vector3 offset;
    public GameObject camFollow;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        offset = Target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {   
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -90f, 60f);

        transform.position = Target.position - (Quaternion.Euler(mouseY, mouseX, 0) * offset);

        transform.eulerAngles = new Vector3(mouseY, mouseX, 0.0f);

        mouseM = Input.GetAxis("Mouse ScrollWheel");
        offset += new Vector3(0, 0, -mouseM * 2);
        offset = new Vector3(offset.x, offset.y, Mathf.Clamp(offset.z, -2, 2));
    }
}
