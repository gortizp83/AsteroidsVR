using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.ShootingGallery;

public class WaveConfiguration
{
    private int m_WaveNumber;
    private GameScore m_MinScoreToPass;
    private List<TargetConfiguration> m_targetSequence;

    public WaveConfiguration(int waveNumber, List<TargetConfiguration> targetSequence)
    {
        m_WaveNumber = waveNumber;
        m_MinScoreToPass = CalculateMinScoreToPass(targetSequence);
        m_targetSequence = targetSequence;
    }

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

    private GameScore CalculateMinScoreToPass(List<TargetConfiguration> targetSequence)
    {
        GameScore score = new GameScore();
        Dictionary<TargetType, int> targetTypeToCountMap = new Dictionary<TargetType, int>();

        foreach(var config in targetSequence)
        {
            if (targetTypeToCountMap.ContainsKey(config.Type))
            {
                targetTypeToCountMap[config.Type]++;
            }
            else
            {
                targetTypeToCountMap[config.Type] = 1;
            }
        }

        foreach (var targetTypeCount in targetTypeToCountMap)
        {
            score.SetScore(targetTypeCount.Key, targetTypeCount.Value);
        }

        return score;
    }
}