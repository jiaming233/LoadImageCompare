using UnityEngine;

/// <summary>
/// ����FPS������
/// </summary>
public class FpsCounter
{
    /// <summary>
    /// ��ǰFPS
    /// </summary>
    public float CurrentFps
    {
        get
        {
            return m_CurrentFps;
        }
    }
    /// <summary>
    /// ˢ��FPSʱ��(��)
    /// </summary>
    public float UpdateInterval
    {
        get
        {
            return m_UpdateInterval;
        }
        set
        {
            if (value <= 0f)
            {
                Debug.LogError("Update interval is invalid.");
                return;
            }

            m_UpdateInterval = value;
            Reset();
        }
    }
    /// <summary>
    /// ˢ��FPSʱ��(��)
    /// </summary>
    private float m_UpdateInterval;
    /// <summary>
    /// ��ǰFPS
    /// </summary>
    private float m_CurrentFps;
    /// <summary>
    /// ˢ��ʱ����֡��
    /// </summary>
    private int m_Frames;
    /// <summary>
    /// ˢ��ʱ����֡����Ӧʱ��(��)
    /// </summary>
    private float m_Accumulator;
    /// <summary>
    /// ��ʱ��
    /// </summary>
    private float m_TimeLeft;

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="updateInterval">ˢ��FPSʱ��(��)</param>
    public FpsCounter(float updateInterval)
    {
        if (updateInterval <= 0f)
        {
            Debug.LogError("Update interval is invalid.");
            return;
        }

        m_UpdateInterval = updateInterval;
        Reset();
    }
    /// <summary>
    /// ����FPS������ÿһ֡������unity Update�е���
    /// </summary>
    /// <param name="realElapseSeconds">����һ֡����ǰ֡��ʱ����������Ϊ��λ</param>
    public void Update(float realElapseSeconds)
    {
        m_Frames++;
        m_Accumulator += realElapseSeconds;
        m_TimeLeft -= realElapseSeconds;

        if (m_TimeLeft <= 0f)
        {
            m_CurrentFps = m_Accumulator > 0f ? m_Frames / m_Accumulator : 0f;
            m_Frames = 0;
            m_Accumulator = 0f;
            m_TimeLeft += m_UpdateInterval;
        }
    }

    /// <summary>
    /// ���ü�����������
    /// </summary>
    private void Reset()
    {
        m_CurrentFps = 0f;
        m_Frames = 0;
        m_Accumulator = 0f;
        m_TimeLeft = 0f;
    }
}
