using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GameManager/CameraManager")]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance{ get; private set; }

    [SerializeField]
    private Camera PlayerCamera;
    private Transform CameraParent;
    private Rigidbody2D CameraParentRigidbody2D;
    [SerializeField]
    private Transform playerTransform;

    private Vector3 LerpValue;
    private bool isOneByOne = true;

    public bool IsOneByOne
    {
        private get
        {
            return isOneByOne;
        }

        set
        {
            isOneByOne = value;
        }
    }

    public Transform PlayerTransform
    {
        get
        {
            return playerTransform;
        }

        set
        {
            playerTransform = value;
        }
    }

    void Start()
    {
        CameraParent = PlayerCamera.transform.parent;
        CameraParentRigidbody2D = CameraParent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsOneByOne)
            return;

        LerpValue = Vector3.Lerp(PlayerTransform.position, CameraParent.position, 0.2f);

        CameraParentRigidbody2D.velocity = (LerpValue - CameraParent.position) * 5;
    }

}
