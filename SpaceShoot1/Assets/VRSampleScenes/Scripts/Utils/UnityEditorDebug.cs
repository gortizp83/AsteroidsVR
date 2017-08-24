using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEditorDebug : MonoBehaviour
{
    [SerializeField] private Transform m_UnityEditorCharacter;

	// Use this for initialization
	void Start ()
    {
#if UNITY_EDITOR
        // Workaround for main camera issue found in Gear VR:
        // When running on the GearVR the main camera needs to be on the root of the tree for the ray caster to work.
        // Otherwise you won't be able to point to anything. This is way the "MainCamera" component needs to live in the root of the tree.
        // PLEASE don't move the "MainCamera" component out of the root tree.
        // However when running on the Unity editor the main camera needs to be inside the "Character" transform that uses the main camera
        // to change the camera using the PC mouse input. Otherwise the camera will only rotate vertically but not horizontally.
        // By changing the transform of the "MainCamera" into the "Character" transform this is issue is fixed, but is should only
        // execute when running on the Unity Editor.
        transform.SetParent(m_UnityEditorCharacter);
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}
