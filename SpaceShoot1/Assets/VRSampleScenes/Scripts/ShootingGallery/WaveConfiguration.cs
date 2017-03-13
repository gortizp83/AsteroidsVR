using UnityEngine;

public class WaveConfiguration : MonoBehaviour
{
    [SerializeField] private int m_WaveNumber;
    [SerializeField] private string m_WaveGoals;
    [SerializeField] private int m_MinScoreToPass = 10;

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

    public int MinScoreToPass
    {
        get
        {
            return m_MinScoreToPass;
        }
    }
}