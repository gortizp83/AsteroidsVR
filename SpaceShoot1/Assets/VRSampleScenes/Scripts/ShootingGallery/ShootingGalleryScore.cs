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
                scoreUI.ScoreText.text = SessionData.Score.GetScore(targetType).ToString();
                // Set the timer bar to be filled by the amount 
                float score = SessionData.Score.GetScore(targetType);
                float minScoreToPass = SessionData.MinScoreToPassWave.GetScore(targetType);
                scoreUI.TimerBar.fillAmount = score / minScoreToPass;
            }
        }
    }
}