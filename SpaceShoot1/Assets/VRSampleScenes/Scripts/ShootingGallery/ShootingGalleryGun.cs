using System.Collections;
using UnityEngine;
using UnityEngine.VR;
using VRStandardAssets.Utils;

namespace VRStandardAssets.ShootingGallery
{
    // This script controls the gun for the shooter
    // scenes, including it's movement and shooting.
    public class ShootingGalleryGun : MonoBehaviour
    {
        [SerializeField] private float m_DefaultLineLength = 70f;                       // How far the line renderer will reach if a target isn't hit.
        [SerializeField] private float m_Damping = 0.5f;                                // The damping with which this gameobject follows the camera.
        [SerializeField] private float m_GunFlareVisibleSeconds = 0.07f;                // How long, in seconds, the line renderer and flare are visible for with each shot.
        [SerializeField] private float m_GunContainerSmoothing = 10f;                   // How fast the gun arm follows the reticle.
        [SerializeField] private AudioSource m_GunAudio;                                // The audio source which plays the sound of the gun firing.
        [SerializeField] private ShootingGalleryController m_ShootingGalleryController; // Reference to the controller so the gun cannot fire whilst the game isn't playing.
        [SerializeField] private VREyeRaycaster m_EyeRaycaster;                         // Used to detect whether the gun is currently aimed at something.
        [SerializeField] private VRInput m_VRInput;                                     // Used to tell the gun when to fire.
        [SerializeField] private Transform m_CameraTransform;                           // Used as a reference to move this gameobject towards.
        [SerializeField] private Transform m_GunContainer;                              // This contains the gun arm needs to be moved smoothly.
        [SerializeField] private Transform m_GunEnd;                                    // This is where the line renderer should start from.
        [SerializeField] private LineRenderer m_GunFlare;                               // This is used to display the gun as a laser.
        [SerializeField] private Reticle m_Reticle;                                     // This is what the gun arm should be aiming at.
        [SerializeField] private BigFireSlider m_BigFireSlider;

        private const float k_DampingCoef = -20f;                                       // This is the coefficient used to ensure smooth damping of this gameobject.
        private bool m_isBigFire;
        private AnimationCurve m_GunFlareInitialWidthCurve;
        private AnimationCurve m_GunFlareBigShotWidthCurve;

        private void Awake()
        {
            m_GunFlare.enabled = false;
            m_GunFlareInitialWidthCurve = new AnimationCurve(m_GunFlare.widthCurve.keys);

            m_GunFlareBigShotWidthCurve = new AnimationCurve();
            foreach (var key in m_GunFlareInitialWidthCurve.keys)
            {
                Keyframe newKey = key;
                newKey.value *= 15;
                m_GunFlareBigShotWidthCurve.AddKey(newKey);
            }
        }


        private void OnEnable ()
        {
            m_BigFireSlider.OnBarFilled += HandleBigFireSliderFill;
            m_VRInput.OnDown += HandleDown;
            m_VRInput.OnUp += HandleUp;
        }

        private void OnDisable ()
        {
            m_BigFireSlider.OnBarFilled -= HandleBigFireSliderFill;
            m_VRInput.OnDown -= HandleDown;
            m_VRInput.OnUp -= HandleUp;
        }


        private void Update()
        {
            // Smoothly interpolate this gameobject's rotation towards that of the user/camera.
            transform.rotation = Quaternion.Slerp(transform.rotation, InputTracking.GetLocalRotation(VRNode.Head),
                m_Damping * (1 - Mathf.Exp(k_DampingCoef * Time.deltaTime)));
            
            // Move this gameobject to the camera.
            transform.position = m_CameraTransform.position;

            // Find a rotation for the gun to be pointed at the reticle.
            Quaternion lookAtRotation = Quaternion.LookRotation (m_Reticle.ReticleTransform.position - m_GunContainer.position);

            // Smoothly interpolate the gun's rotation towards that rotation.
            m_GunContainer.rotation = Quaternion.Slerp (m_GunContainer.rotation, lookAtRotation,
                m_GunContainerSmoothing * Time.deltaTime);
        }

        private void HandleBigFireSliderFill()
        {
            m_isBigFire = true;
        }

        private void HandleDown ()
        {
            ExecuteFire(false);
        }

        private void HandleUp()
        {
            if (m_isBigFire)
            {
                ExecuteFire(true);
            }

            m_isBigFire = false;
        }

        private void ExecuteFire(bool isBigFire)
        {
            // If the game isn't playing don't do anything.
            if (!m_ShootingGalleryController.IsPlaying)
                return;

            // Otherwise, if there is an interactible currently being looked at, try to find it's ShootingTarget component.
            ShootingTarget shootingTarget = m_EyeRaycaster.CurrentInteractible ? m_EyeRaycaster.CurrentInteractible.GetComponent<ShootingTarget>() : null;

            if (shootingTarget != null)
                shootingTarget.TargetHit(isBigFire ? 4 : 1);

            // If there is a ShootingTarget component get it's transform as the target for shooting at.
            Transform target = shootingTarget ? shootingTarget.transform : null;

            // Start shooting at the target.
            StartCoroutine(Fire(target, isBigFire));
        }


        private IEnumerator Fire(Transform target, bool isBigFire)
        {
            // Play the sound of the gun firing.
            m_GunAudio.Play();

            // Set the length of the line renderer to the default.
            float lineLength = m_DefaultLineLength;

            m_GunFlare.widthCurve = m_GunFlareInitialWidthCurve;
            if (isBigFire)
            {
                m_GunFlare.widthCurve = m_GunFlareBigShotWidthCurve;
            }

            // If there is a target, the line renderer's length is instead the distance from the gun to the target.
            if (target)
                lineLength = Vector3.Distance (m_GunEnd.position, target.position);

            // Turn the line renderer on.
            m_GunFlare.enabled = true;
            
            // Whilst the line renderer is on move it with the gun.
            yield return StartCoroutine (MoveLineRenderer (lineLength));
            
            // Turn the line renderer off again.
            m_GunFlare.enabled = false;
        }


        private IEnumerator MoveLineRenderer (float lineLength)
        {
            // Create a timer.
            float timer = 0f;

            // While that timer has not yet reached the length of time that the gun effects should be visible for...
            while (timer < m_GunFlareVisibleSeconds)
            {
                // ... set the line renderer to start at the gun and finish forward of the gun the determined distance.
                m_GunFlare.SetPosition(0, m_GunEnd.position);
                m_GunFlare.SetPosition(1, m_GunEnd.position + m_GunEnd.forward * lineLength);

                // Wait for the next frame.
                yield return null;

                // Increment the timer by the amount of time waited.
                timer += Time.deltaTime;
            }
        }
    }
}