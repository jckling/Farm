using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    #region Event Functions

    private void Start()
    {
        SwitchConfinerShape();
    }

    #endregion

    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape =
            GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = confinerShape;
        confiner.InvalidatePathCache();
    }
}