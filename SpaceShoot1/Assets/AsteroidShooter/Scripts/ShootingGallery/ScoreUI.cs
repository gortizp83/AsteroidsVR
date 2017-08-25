using UnityEngine;
using UnityEngine.UI;

namespace VRStandardAssets.ShootingGallery
{
    internal class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text m_ScoreText;
        [SerializeField] private Image m_TimerBar;                      // The time remaining is shown on the UI for the gun, this is a reference to the image showing the time remaining.
        [SerializeField] private TargetType m_TargetType;               // The type of target that the score represent

        public Text ScoreText
        {
            get
            {
                return m_ScoreText;
            }

            set
            {
                m_ScoreText = value;
            }
        }

        public Image TimerBar
        {
            get
            {
                return m_TimerBar;
            }

            set
            {
                m_TimerBar = value;
            }
        }

        public TargetType TargetType
        {
            get
            {
                return m_TargetType;
            }

            set
            {
                m_TargetType = value;
            }
        }
    }
}