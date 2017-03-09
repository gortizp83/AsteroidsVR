using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class VRDebugUtilities : MonoBehaviour
{
    [SerializeField] private bool m_MouseLookEnabled = true;        // Use mouse look for debugging purposes.
    [SerializeField] private MouseLook m_MouseLook;

    private Camera m_Camera;

    // Use this for initialization
    void Start ()
    {
        m_Camera = Camera.main;
        m_MouseLook.Init(transform, m_Camera.transform);
    }
    
    // Update is called once per frame
    void Update () {
        RotateView();
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }
}
