using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.ShootingGallery;

namespace VRStandardAssets.Common
{
    // This class is used to keep score during a game and save
    // the highscores to PlayerPrefs.
    public static class SessionData
    {
        // This enum shows all the types of games that use scores.
        public enum GameType
        {
            FLYER,
            SHOOTER180,
            SHOOTER360
        };

        private const string k_LastWave ="waveData";
        private const string k_LastLevel = "levelData";

        private static GameScore s_Score = new GameScore();                                 // Used to store the current game's score.

        // Wave and level information
        private static int s_Wave = 1;
        private static int s_NumberOfWaves = 2;
        private static int s_Level = 1;
        private static GameScore s_MinScoreToPassWave = new GameScore();

        public static GameScore Score { get { return s_Score; } }

        public static int WaveCount
        {
            get
            {
                return s_Wave;
            }

            set
            {
                s_Wave = value;
            }
        }

        public static int NumberOfWaves
        {
            get
            {
                return s_NumberOfWaves;
            }

            set
            {
                s_NumberOfWaves = value;
            }
        }

        public static int Level
        {
            get
            {
                return s_Level;
            }

            set
            {
                s_Level = value;
            }
        }

        public static GameScore MinScoreToPassWave
        {
            get
            {
                return s_MinScoreToPassWave;
            }

            set
            {
                s_MinScoreToPassWave = value;
            }
        }

        public static void Restart()
        {
            // Reset the current score and get the highscore from player prefs.
            s_Score.Reset();
        }


        public static void AddScore(TargetType type)
        {
            // Add to the current score and check if the high score needs to be set.
            s_Score.AddToScore(type);
        }

        public static void SaveGame()
        {
            // Set the high score for the current game's name and save it.
            PlayerPrefs.SetInt(k_LastLevel, s_Level);
            PlayerPrefs.SetInt(k_LastWave, s_Wave);
            PlayerPrefs.Save();
        }

        public static void RestoreLastGameData()
        {
            s_Level = PlayerPrefs.GetInt(k_LastLevel, 1);
            s_Wave = PlayerPrefs.GetInt(k_LastWave, 1);
        }
    }
}