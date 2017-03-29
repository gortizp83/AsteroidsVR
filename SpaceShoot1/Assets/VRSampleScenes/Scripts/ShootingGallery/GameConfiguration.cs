using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.ShootingGallery;

public class GameConfiguration : MonoBehaviour {

    List<LevelConfiguration> m_Levels;

    private int m_levelIdx = 0;

    private void Awake()
    {
        m_Levels = GameStatics.GetLevels();
    }

    public LevelConfiguration GetCurrentLevel()
    {
        return m_Levels[m_levelIdx];
    }

    internal PhaseResult FinishPhase(GameScore score)
    {
        PhaseResult result = new PhaseResult();

        result.MinScoreToPass = GetCurrentLevel().GetCurrentWave().MinScoreToPass;

        if (score >= GetCurrentLevel().GetCurrentWave().MinScoreToPass)
        {
            result.Pass = true;
            result.IsGameEnd = !MoveToNextPhase();

            if (result.IsGameEnd)
            {
                result.Message = "Game Over!";
            }
            else
            {
                result.Message = string.Format("Wave {0}", GetCurrentLevel().GetCurrentWave().WaveNumber.ToString());
            }
        }
        else
        {
            result.Pass = false;
            result.Message = string.Format("Wave {0} <b>FAILED</b>!\r\n Try Again!", GetCurrentLevel().GetCurrentWave().WaveNumber.ToString()); ;
            result.IsGameEnd = false;
        }

        return result;
    }

    /// <summary>
    /// Move to the next wave or level.
    /// </summary>
    /// <returns>false if we have reached the last level and wave</returns>
    private bool MoveToNextPhase()
    {
        // Try moving to the next level once we have reached the maximum wave count
        if (!GetCurrentLevel().TryMoveNextWave())
        {
            m_levelIdx++;
        }

        if (m_levelIdx >= m_Levels.Count)
        {
            // We don't have more levels! Return back to the first level to avoid crashes
            m_levelIdx = 0;
            return false;
        }

        return true;
    }
}
