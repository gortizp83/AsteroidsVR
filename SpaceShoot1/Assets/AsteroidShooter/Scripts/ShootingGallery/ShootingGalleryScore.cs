using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Common;

namespace VRStandardAssets.ShootingGallery
{
    // This script displays the player's score during the
    // shooter scenes.
    public class ShootingGalleryScore : MonoBehaviour
    {
        [SerializeField] private ScoreUI[] m_ScoreUI;

        private void Update()
        {
            foreach (var scoreUI in m_ScoreUI)
            {
                var targetType = scoreUI.TargetType;

                float score = SessionData.Score.GetCurrentScore(targetType);
                float minScoreToPass = SessionData.MinScoreToPassWave.GetCurrentScore(targetType);
                scoreUI.ScoreText.text = string.Format("{0}/{1}", score, minScoreToPass);
                // Set the timer bar to be filled by the amount 
                scoreUI.TimerBar.fillAmount = score / minScoreToPass;
            }
        }
    }
}