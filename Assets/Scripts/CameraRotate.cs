using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    public float mouseSmoothFactor = 1f;

    private Vector2 totalMouseRot = Vector2.zero;
    private Vector2 smooth = Vector2.zero;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 newMouseRot = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        newMouseRot = Vector2.Scale(newMouseRot, new Vector2(mouseSensitivity * mouseSmoothFactor, mouseSensitivity * mouseSmoothFactor));
        smooth.x = Mathf.Lerp(smooth.x, newMouseRot.x, 1 / mouseSmoothFactor);
        smooth.y = Mathf.Lerp(smooth.y, newMouseRot.y, 1 / mouseSmoothFactor);
        totalMouseRot += smooth;
        totalMouseRot.y = Mathf.Clamp(totalMouseRot.y, -60f, 60f);
        transform.localRotation = Quaternion.AngleAxis(totalMouseRot.y * -1, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(totalMouseRot.x, player.up);
    }
}
