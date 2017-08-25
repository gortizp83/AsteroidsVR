using UnityEngine;

public class TargetDefinition : MonoBehaviour
{
    [SerializeField] GameObject m_Prefab;
    [SerializeField] Transform m_InitialPosition;

    public GameObject Prefab
    {
        get
        {
            return m_Prefab;
        }
    }
}