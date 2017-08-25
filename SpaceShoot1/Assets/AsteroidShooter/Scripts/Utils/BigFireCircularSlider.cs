using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRStandardAssets.Utils
{
    // This class works similarly to the SelectionRadial class except
    // it has a physical manifestation in the scene.  This can be
    // either a UI slider or a mesh with the SlidingUV shader.  The
    // functions as a bar that fills up whilst the user looks at it
    // and holds down the Fire1 button.
    public class BigFireCircularSlider : BigFireSlider
    {
        [SerializeField] private Image m_CircularSlider;

        private const string k_SliderMaterialPropertyName = "_SliderValue"; // The name of the property on the SlidingUV shader that needs to be changed in order for it to fill.

        private void Start()
        {
            // Setup the radial to have no fill at the start and hide if necessary.
            m_CircularSlider.fillAmount = 0f;
            m_CircularSlider.gameObject.SetActive(true);
        }

        protected override void SetSliderValue(float sliderValue)
        {
            if (m_CircularSlider)
                m_CircularSlider.fillAmount = sliderValue;

            // If there is a renderer set the shader's property to the given slider value.
            if (m_Renderer)
                m_Renderer.sharedMaterial.SetFloat(k_SliderMaterialPropertyName, sliderValue);
        }
    }
}