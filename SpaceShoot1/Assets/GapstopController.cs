using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class GapstopController : MonoBehaviour {

    [SerializeField]
    private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.

    [SerializeField]
    private BigFireSlider[] m_slider;

    public event Action<int> OnGapstopFilled;
    private List<Coroutine> m_WaitForBarToFillCorroutines = new List<Coroutine>();
    int m_GapstopCount = 0;

    // Use this for initialization
    void Start ()
    {
        //m_slider = this.GetComponentsInChildren<BigFireSlider>();
    }

    private void OnEnable()
    {
        m_VRInput.OnDown += HandleDown;
        m_VRInput.OnUp += HandleUp;
    }

    private void OnDisable()
    {
        m_VRInput.OnDown -= HandleDown;
        m_VRInput.OnUp -= HandleUp;
    }

    private void HandleUp()
    {
        m_GapstopCount = 0;
        foreach (var slider in m_slider)
        {
            slider.StopFilling();
        }

        StopAllCoroutines();
    }

    private void HandleDown()
    {
        StartCoroutine(StartFill());
    }

    private IEnumerator StartFill()
    {
        foreach (var slider in m_slider)
        {
            slider.enabled = true;
            slider.StartFilling();
            var cr = StartCoroutine(slider.WaitForBarToFill());
            m_WaitForBarToFillCorroutines.Add(cr);
            yield return cr;
            m_GapstopCount++;
            OnGapstopFilled(m_GapstopCount);
        }
    }
}
