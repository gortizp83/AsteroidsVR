using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class VRButton : MonoBehaviour {

    [SerializeField] private Button m_Button;
    [SerializeField] private VRInteractiveItem m_InteractiveItem;
    [SerializeField] private VRInput m_VRInput;
    [SerializeField] private AudioSource m_Audio;                       // Reference to the audio source that will play effects when the user looks at it and when it fills.
    [SerializeField] private AudioClip m_OnOverClip;                    // The clip to play when the user looks at the button.
    [SerializeField] private AudioClip m_OnPressedClip;                  // The clip to play when the button is pressed.

    public event Action<VRButton> OnDown;

    private bool m_Pressed = false;
    private bool m_GazeOver = false;                                            // Whether the user is currently looking at the bar.

    public VRInput VRInput
    {
        get
        {
            return m_VRInput;
        }

        set
        {
            m_VRInput = value;

            m_VRInput.OnDown += HandleDown;
            m_VRInput.OnUp += HandleUp;
        }
    }

    private Dictionary<string, object> m_propertyBag = new Dictionary<string, object>();

    public Dictionary<string, object> PropertyBag
    {
        get { return m_propertyBag; }
    }

    private void OnEnable()
    {
        if (VRInput)
        {
            VRInput.OnDown += HandleDown;
            VRInput.OnUp += HandleUp;
        }

        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }

    private void HandleOut()
    {
        m_GazeOver = false;

        if (m_Pressed)
            return;

        m_Button.image.color = m_Button.colors.normalColor;
    }

    private void HandleOver()
    {
        m_GazeOver = true;

        if (m_Pressed)
            return;

        m_Button.image.color = m_Button.colors.highlightedColor;

        m_Audio.clip = m_OnOverClip;
        m_Audio.Play();
    }

    private void HandleUp()
    {
        // TODO: handle up
    }

    private void HandleDown()
    {
        if (m_GazeOver)
        {
            m_Button.image.color = m_Button.colors.pressedColor;
            m_Pressed = true;

            m_Audio.clip = m_OnPressedClip;
            m_Audio.Play();

            if (OnDown != null)
                OnDown(this);
        }
    }

    private void OnDisable()
    {
        //VRInput.OnDown -= HandleDown;
        //VRInput.OnUp -= HandleUp;

        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnClick += HandleDown;
    }

    private void OnDestroy()
    {
        // Ensure that all events are unsubscribed when this is destroyed.
        OnDown = null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
