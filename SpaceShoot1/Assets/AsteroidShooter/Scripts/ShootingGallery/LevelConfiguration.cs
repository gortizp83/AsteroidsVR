using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfiguration
{
    private List<WaveConfiguration> m_waveConfig = new List<WaveConfiguration>();
    private int m_levelNumber;

    private int m_waveIdx = 0;

    public LevelConfiguration(int levelNumber)
    {
        m_levelNumber = levelNumber;
    }

    public int LevelNumber
    {
        get { return m_levelNumber; }
        set { m_levelNumber = value; }
    }

    public List<WaveConfiguration> WaveConfig
    {
        get
        {
            return m_waveConfig;
        }
    }

    public WaveConfiguration GetCurrentWave()
    {
        return WaveConfig[m_waveIdx];
    }

    public bool TryMoveNextWave()
    {
        if (++m_waveIdx >= WaveConfig.Count)
        {
            // We don't have more waves. Reset wave count.
            m_waveIdx = 0;
            return false;
        }

        return true;
    }

    public void SetInitialWave(int initialWave)
    {
        m_waveIdx = initialWave - 1;
    }
}