using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.ShootingGallery;

public class WaveConfiguration
{
    private int m_WaveNumber;
    private GameScore m_MinScoreToPass;
    private List<TargetConfiguration> m_targetSequence;

    public WaveConfiguration(int waveNumber, GameScore minScoreToPass, List<TargetConfiguration> targetSequence)
    {
        m_WaveNumber = waveNumber;
        m_MinScoreToPass = minScoreToPass;
        m_targetSequence = targetSequence;
    }

    public int WaveNumber
    {
        get { return m_WaveNumber; }
        set { m_WaveNumber = value; }
    }

    public GameScore MinScoreToPass
    {
        get { return m_MinScoreToPass; }
        set { m_MinScoreToPass = value; }
    }

    public List<TargetConfiguration> TargetSequence
    {
        get
        {
            return m_targetSequence;
        }

        set
        {
            m_targetSequence = value;
        }
    }
}