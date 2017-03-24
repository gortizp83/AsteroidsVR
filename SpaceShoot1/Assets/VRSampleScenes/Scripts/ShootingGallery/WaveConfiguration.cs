﻿using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.ShootingGallery;

public class WaveConfiguration
{
    private int m_WaveNumber;
    private string m_WaveGoals;
    private GameScore m_MinScoreToPass;
    private List<TargetType> m_targetSequence;

    public WaveConfiguration(int waveNumber, GameScore minScoreToPass, string waveGoals, List<TargetType> targetSequence)
    {
        m_WaveNumber = waveNumber;
        m_MinScoreToPass = minScoreToPass;
        m_WaveGoals = waveGoals;
        m_targetSequence = targetSequence;
    }

    public int WaveNumber
    {
        get { return m_WaveNumber; }
        set { m_WaveNumber = value; }
    }

    public string WaveGoals
    {
        get { return m_WaveGoals; }
        set { m_WaveGoals = value; }
    }

    public GameScore MinScoreToPass
    {
        get { return m_MinScoreToPass; }
        set { m_MinScoreToPass = value; }
    }

    public List<TargetType> TargetSequence
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