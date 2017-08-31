using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using System;

namespace VRStandardAssets.ShootingGallery
{
    // This simple class encapsulates the UI for
    // the shooter scenes so that the game
    // controller need only reference one thing to
    // control the UI during the games.
    public class UIController : MonoBehaviour
    {
        [SerializeField] private UIFader m_IntroUI;     // This controls fading the UI shown during the intro.
        [SerializeField] private UIFader m_OutroUI;     // This controls fading the UI shown during the outro.
        [SerializeField] private Text m_OutroUIMessage;
        [SerializeField] private UIFader m_PlayerUI;    // This controls fading the UI that shows around the gun that moves with the player.
        [SerializeField] private UIFader m_EndOfWaveUI;
        [SerializeField] private Text m_EndOfWaveMessage;      // Reference to the Text component that displays the end of wave message
        [SerializeField] private UIFader m_GamePausedUI;

        internal IEnumerator ShowEndOfWaveUI(string endOfWaveMessage)
        {
            m_EndOfWaveMessage.text = endOfWaveMessage;
            yield return StartCoroutine(m_EndOfWaveUI.InteruptAndFadeIn());
        }

        public IEnumerator HideEndOfWaveUI()
        {
            yield return StartCoroutine(m_EndOfWaveUI.InteruptAndFadeOut());
        }

        public IEnumerator ShowIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeIn());
        }


        public IEnumerator HideIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeOut());
        }

        internal IEnumerator ShowOutroUI(string outroMessage)
        {
            m_OutroUIMessage.text = outroMessage;
            yield return StartCoroutine(m_OutroUI.InteruptAndFadeIn());
        }

        public IEnumerator HideOutroUI()
        {
            yield return StartCoroutine(m_OutroUI.InteruptAndFadeOut());
        }

        public IEnumerator ShowPlayerUI ()
        {
            yield return StartCoroutine (m_PlayerUI.InteruptAndFadeIn ());
        }

        public IEnumerator HidePlayerUI ()
        {
            yield return StartCoroutine (m_PlayerUI.InteruptAndFadeOut ());
        }

        public IEnumerator ShowGamePausedUI()
        {
            yield return StartCoroutine(m_GamePausedUI.InteruptAndFadeIn());
        }

        public IEnumerator HideGamePausedUI()
        {
            yield return StartCoroutine(m_GamePausedUI.InteruptAndFadeOut());
        }
    }
}