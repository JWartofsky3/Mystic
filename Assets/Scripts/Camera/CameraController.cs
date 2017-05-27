using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Constants
    private const float Y_ANGLE_MIN = -85f;
    private const float Y_ANGLE_MAX = 85f;
    private const float MAX_ZOOM = 4.0f;
    private const float MIN_ZOOM = 2.0f;
    //Public variables
    public Transform target;
    public float distance;
    public float sensitivityX;
    public float sensitivityY;
    public float smoothing;
    //Private variables
    private float currentDistance = 0.0f;
    private float distToCol = Mathf.Infinity;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private Vector3 offset = new Vector3(0, 1f, 0);


    private void Start()
    {
        currentDistance = distance;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal2");
        float v = Input.GetAxisRaw("Vertical2");
        currentX += h * sensitivityX;
        currentY += v * sensitivityY;
        if (Input.GetButtonDown("Zoom"))
        {
            zoom();
        }
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -1 * currentDistance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 targetPos = target.position + offset + rotation * dir;
        transform.position = targetPos;
        transform.LookAt(target.position + offset);
        if (toCamera())
        {
            currentDistance = distToCol - 0.2f;
        }
        currentDistance = Mathf.Lerp(currentDistance, distance, 0.1f);
    }

    private void zoom()
    {
        if (distance < MAX_ZOOM)
        {
            distance = MAX_ZOOM;
        } else
        {
            distance = MIN_ZOOM;
        }
    }

    private bool toCamera()
    {
        RaycastHit hit;
        bool collision = Physics.Raycast(target.position + offset, transform.position - (target.position + offset), out hit, distance);
        if (collision)
        {
            distToCol = hit.distance;
        } else
        {
            distToCol = Mathf.Infinity;
        }
        return collision;
    }
}
