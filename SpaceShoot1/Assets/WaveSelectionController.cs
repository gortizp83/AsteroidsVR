using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;

public class WaveSelectionController : MonoBehaviour {

    [SerializeField] private Canvas m_UICanvas;
    [SerializeField] private GameObject m_VRButonPrefab;
    [SerializeField] private VRInput m_VRInput;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < SessionData.Wave; i++)
        {
            var newButton = Instantiate(m_VRButonPrefab);
            newButton.GetComponent<VRButton>().VRInput = m_VRInput;

            newButton.transform.SetParent(m_UICanvas.transform);

            var btn = newButton.GetComponent<Button>();
            var txt = btn.GetComponentInChildren<Text>();
            txt.text = (i + 1).ToString();
            var rectTransform = newButton.transform as RectTransform;
            var width = rectTransform.rect.width;
            rectTransform.localPosition = new Vector3(i * width, 0, 0);
            rectTransform.localScale = Vector3.one;

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
