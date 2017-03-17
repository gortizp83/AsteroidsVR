using System;
using System.Collections;
using UnityEngine;

namespace VRStandardAssets.ShootingGallery
{
    public abstract class ShootingTargetBase : MonoBehaviour
    {
        public abstract void Restart(float gameTimeRemaining);
        public abstract void DoUpdate();
        protected abstract void HandleDown();
    }
}