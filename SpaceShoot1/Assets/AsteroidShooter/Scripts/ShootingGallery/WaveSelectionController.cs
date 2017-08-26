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
    private int m_selectedWave = -1;
    private List<GameObject> m_buttons = new List<GameObject>();

    public int SelectedWave
    {
        get
        {
            return m_selectedWave;
        }
    }

    public void ShowWaveButtons()
    {
        m_pressedButton = null;

        for (int wave = SessionData.WaveCount; wave > 0; wave--)
        {
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
            // position the buttons in reverse order, i.e. put the last one closer to the user's view
            rectTransform.localPosition = new Vector3(m_buttons.Count * width, 0, 0);
            rectTransform.localScale = Vector3.one;

            m_buttons.Add(newButton);
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

        m_selectedWave = -1;
        object value;
        if (m_pressedButton != null && m_pressedButton.PropertyBag.TryGetValue(kWaveValue, out value))
        {
            m_selectedWave = (int)value;
        }

        m_pressedButton = null;

        // clear all buttons, we'll recreate them again when showing the menu
        for (int i = m_buttons.Count - 1; i >= 0; i--)
        {
            m_buttons[i].GetComponent<VRButton>().OnDown -= VrButton_OnDown;
            GameObject.Destroy(m_buttons[i], 0.5f);
        }

        m_buttons.Clear();
    }
}
