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

    private const string kWaveValue = "waveValue";
    private VRButton m_pressedButton = null;

    public int SelectedWave
    {
        get
        {
            int wave = -1;
            object value;
            if (m_pressedButton != null && m_pressedButton.PropertyBag.TryGetValue(kWaveValue, out value))
            {
                wave = (int)value;
            }
            return wave;
        }
    }

    // Use this for initialization
    void Start ()
    {
        ShowWaveButtons();
    }

    private void ShowWaveButtons()
    {
        m_pressedButton = null;

        for (int i = 0; i < SessionData.Wave; i++)
        {
            int wave = i + 1;
            var newButton = Instantiate(m_VRButonPrefab);
            var vrButton = newButton.GetComponent<VRButton>();
            vrButton.VRInput = m_VRInput;
            vrButton.PropertyBag.Add(kWaveValue, wave);
            vrButton.OnDown += VrButton_OnDown;

            newButton.transform.SetParent(m_UICanvas.transform);

            var btn = newButton.GetComponent<Button>();
            var txt = btn.GetComponentInChildren<Text>();
            txt.text = wave.ToString();
            var rectTransform = newButton.transform as RectTransform;
            var width = rectTransform.rect.width;
            rectTransform.localPosition = new Vector3(i * width, 0, 0);
            rectTransform.localScale = Vector3.one;
        }
    }

    private void VrButton_OnDown(VRButton sender)
    {
        m_pressedButton = sender;
    }

    public IEnumerator WaitForWaveSelection()
    {
        while (m_pressedButton == null)
        {
            yield return null;
        }
    }
}
