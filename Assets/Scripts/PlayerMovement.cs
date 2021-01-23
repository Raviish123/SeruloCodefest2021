using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public float speed;
    public bool isRunning;

    private Animator anim;
    private GameObject head;
    private GameObject mainCam;
    private Vector3 moveDir;
    private Rigidbody rb;
    private float mainCamX;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = transform.GetChild(0).gameObject;
        mainCam.SetActive(photonView.IsMine);
        if (!photonView.IsMine) return;
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        anim.SetFloat("Speed", 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        head = transform.GetChild(1).GetChild(0).GetComponent<PlayerHead>().head;
        if (photonView.IsMine)
        {
            Move();
        }
    }

    private void Move()
    {
        anim = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        
        
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var running = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        mainCam.GetComponent<Camera>().nearClipPlane = running == 2 ? Mathf.Lerp(mainCam.GetComponent<Camera>().nearClipPlane, 0.75f, Time.deltaTime * 6f) : Mathf.Lerp(mainCam.GetComponent<Camera>().nearClipPlane, 0.375f, Time.deltaTime * 6f);
        var currentSpeed = Mathf.Clamp(new Vector2(horizontalMovement, verticalMovement).sqrMagnitude, 0, 1);
        anim.SetFloat("Speed", Mathf.Clamp(currentSpeed * running, 0, 2));
        moveDir = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized * running;
        isRunning = running == 2;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, -1.35f, 0);
        transform.GetChild(1).GetChild(0).localRotation = Quaternion.identity;
        if (photonView.IsMine)
        {
            head.transform.eulerAngles = new Vector3(mainCam.transform.eulerAngles.x, head.transform.eulerAngles.y, head.transform.eulerAngles.z);
        } else
        {
            head.transform.eulerAngles = new Vector3(mainCamX, head.transform.eulerAngles.y, head.transform.eulerAngles.z);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mainCam.transform.eulerAngles.x);
        } else
        {
            mainCamX = (float)stream.ReceiveNext();
        }
    }
}
