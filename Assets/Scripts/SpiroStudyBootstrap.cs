using UnityEngine;

public class SpiroStudyBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitIfEmptyScene()
    {
        if (Object.FindObjectOfType<Spirograph3DGenerator>() != null)
        {
            return;
        }

        GameObject curve = new GameObject("SpirographCurve");
        curve.transform.position = Vector3.zero;

        LineRenderer lineRenderer = curve.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        curve.AddComponent<Spirograph3DGenerator>();
        curve.AddComponent<PerspectiveGridGizmo>();

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            mainCamera = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera";
        }

        mainCamera.transform.position = new Vector3(0f, 8f, -20f);
        mainCamera.transform.LookAt(Vector3.zero);

        OrbitCameraController orbit = mainCamera.gameObject.GetComponent<OrbitCameraController>();
        if (orbit == null)
        {
            orbit = mainCamera.gameObject.AddComponent<OrbitCameraController>();
        }

        orbit.target = curve.transform;

        if (Object.FindObjectOfType<Light>() == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }
    }
}
