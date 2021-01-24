using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipMouse : MonoBehaviour
{
    public Vector3 offset;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + offset;
    }
}
