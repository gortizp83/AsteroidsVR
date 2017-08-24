using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class VRDebugUtilities : MonoBehaviour
{
    [SerializeField] private bool m_MouseLookEnabled = true;        // Use mouse look for debugging purposes.
    [SerializeField] private MouseLook m_MouseLook;

    private Camera m_Camera;

//    private void Awake()
//    {
//#if UNITY_EDITOR
//        var parent = transform.parent;
//        var mainCamera = parent.Find("MainCamera");
//        mainCamera.transform.parent = this.transform;
//#endif
//    }

    // Use this for initialization
    void Start ()
    {
#if UNITY_EDITOR
        m_Camera = Camera.main;
        m_MouseLook.Init(transform, m_Camera.transform);
#endif
    }

    // Update is called once per frame
    void Update () {
        RotateView();
    }

    private void FixedUpdate()
    {
 #if UNITY_EDITOR
        m_MouseLook.UpdateCursorLock();
#endif
    }

    private void RotateView()
    {
#if UNITY_EDITOR
        m_MouseLook.LookRotation(transform, m_Camera.transform);
#endif
    }
}
