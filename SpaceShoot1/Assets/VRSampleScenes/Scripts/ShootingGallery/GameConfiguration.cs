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
            result.Message = "Good job!";
            result.IsGameEnd = !MoveToNextPhase();
        }
        else
        {
            result.Pass = false;
            result.Message = "Try again!";
            result.IsGameEnd = false;
        }

        return result;
    }

    private bool MoveToNextPhase()
    {
        if (!GetCurrentLevel().TryMoveNextWave())
        {
            m_levelIdx++;
        }

        if (m_levelIdx >= m_Levels.Count)
        {
            // We don't have more levels! Return back to the last level to avoid crashes
            m_levelIdx--;
            return false;
        }

        return true;
    }
}
