using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    public EHand m_Hand;

    public GameObject m_object;
    public float m_Scalar;
    public Transform referenceModel;




    Vector3 m_LastPos;

    bool m_bTranslating = false;


    void Start()
    {

    }

    void Update()
    {
        m_Scalar = referenceModel.localScale.x / 20f;
        if (m_Scalar < 2)
            m_Scalar = 2.0f;
        if (m_Scalar > 6)
            m_Scalar = 6.0f;




        if ( m_bTranslating && DetectionManager.Get().IsHandSet(m_Hand))
        {
            Vector3 newPos = GetPosition();

            Vector3 ChangeInDistance = newPos - m_LastPos;

            m_object.transform.position += ChangeInDistance * m_Scalar;

            m_LastPos = newPos;
        }
    }

    Vector3 GetPosition()
    {
        return DetectionManager.Get().GetHand(m_Hand).GetHandPosition();
    }

    public void StartTranslation()
    {
        m_LastPos = GetPosition();
        m_bTranslating = true;
    }

    public void EndTranslation()
    {
        m_bTranslating = false;
    }
}
