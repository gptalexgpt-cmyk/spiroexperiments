using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spirograph3DGenerator : MonoBehaviour
{
    [Header("Spirograph Parameters")]
    [Min(0.01f)] public float R = 6f;
    [Min(0.01f)] public float r = 2f;
    [Min(0.01f)] public float d = 1.5f;

    [Header("3D Projection")]
    [Tooltip("How much the curve rises/falls along Y.")]
    public float verticalAmplitude = 1.25f;
    [Tooltip("Frequency multiplier for Y oscillation.")]
    public float verticalFrequency = 0.5f;
    [Tooltip("Scale applied to final XYZ points.")]
    public float worldScale = 0.5f;

    [Header("Sampling")]
    [Range(128, 10000)] public int sampleCount = 3000;
    [Tooltip("Higher = more loops.")]
    [Range(1f, 30f)] public float tMaxMultiplier = 12f;

    [Header("Rendering")]
    public float lineWidth = 0.035f;
    public Gradient colorGradient;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = false;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.alignment = LineAlignment.View;

        if (colorGradient == null || colorGradient.colorKeys.Length == 0)
        {
            colorGradient = BuildDefaultGradient();
        }

        lineRenderer.colorGradient = colorGradient;
    }

    private void Start()
    {
        Regenerate();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.widthMultiplier = lineWidth;
            }
        }
    }
#endif

    [ContextMenu("Regenerate Spirograph")]
    public void Regenerate()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.colorGradient = colorGradient;

        List<Vector3> points = new List<Vector3>(sampleCount);
        float tMax = Mathf.PI * 2f * tMaxMultiplier;

        for (int i = 0; i < sampleCount; i++)
        {
            float t = Mathf.Lerp(0f, tMax, i / (float)(sampleCount - 1));
            points.Add(Epitrochoid3D(t) * worldScale);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private Vector3 Epitrochoid3D(float t)
    {
        // Epitrochoid in XZ plane:
        // x = (R + r) cos(t) - d cos((R + r)/r * t)
        // z = (R + r) sin(t) - d sin((R + r)/r * t)
        float k = (R + r) / r;

        float x = (R + r) * Mathf.Cos(t) - d * Mathf.Cos(k * t);
        float z = (R + r) * Mathf.Sin(t) - d * Mathf.Sin(k * t);

        // Extend into Y to study perspective depth.
        float y = verticalAmplitude * Mathf.Sin(t * verticalFrequency);

        return new Vector3(x, y, z);
    }

    private Gradient BuildDefaultGradient()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.2f, 0.8f, 1f), 0f),
                new GradientColorKey(new Color(0.9f, 0.3f, 1f), 0.5f),
                new GradientColorKey(new Color(1f, 0.9f, 0.2f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );

        return gradient;
    }
}
