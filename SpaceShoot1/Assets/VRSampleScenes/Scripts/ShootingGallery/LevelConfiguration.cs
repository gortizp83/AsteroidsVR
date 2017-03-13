using System;
using UnityEngine;

public class LevelConfiguration : MonoBehaviour
{
    [SerializeField] private WaveConfiguration[] m_waveConfig;
    [SerializeField] private int m_levelNumber;

    private int m_waveIdx = 0;

    public WaveConfiguration[] WaveConfig
    {
        get { return m_waveConfig; }
        set { m_waveConfig = value; }
    }

    public int LevelNumber
    {
        get { return m_levelNumber; }
        set { m_levelNumber = value; }
    }

    public WaveConfiguration GetCurrentWave()
    {
        return m_waveConfig[m_waveIdx];
    }

    public bool TryMoveNextWave()
    {
        if (m_waveIdx++ < m_waveConfig.Length)
        {
            return true;
        }

        return false;
    }
}