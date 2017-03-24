using VRStandardAssets.Common;

namespace VRStandardAssets.ShootingGallery
{
    struct PhaseResult
    {
        public bool Pass;
        public string Message;

        public bool IsGameEnd { get; internal set; }
        public GameScore MinScoreToPass { get; internal set; }
    }
}