using UnityEngine;

public class PerspectiveGridGizmo : MonoBehaviour
{
    public int halfSize = 10;
    public float spacing = 1f;
    public Color gridColor = new Color(1f, 1f, 1f, 0.2f);
    public Color axisXColor = new Color(1f, 0.4f, 0.4f, 0.9f);
    public Color axisZColor = new Color(0.4f, 0.7f, 1f, 0.9f);

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        for (int i = -halfSize; i <= halfSize; i++)
        {
            Gizmos.color = i == 0 ? axisZColor : gridColor;
            Gizmos.DrawLine(
                new Vector3(i * spacing, 0f, -halfSize * spacing),
                new Vector3(i * spacing, 0f, halfSize * spacing)
            );

            Gizmos.color = i == 0 ? axisXColor : gridColor;
            Gizmos.DrawLine(
                new Vector3(-halfSize * spacing, 0f, i * spacing),
                new Vector3(halfSize * spacing, 0f, i * spacing)
            );
        }
    }
}
