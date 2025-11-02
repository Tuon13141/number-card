using UnityEngine;

public class BGScaler : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera targetCamera;
    [SerializeField, Min(0.01f)] private float distanceFromCamera = 10f;

    [Header("Options")]
    [Tooltip("Tự động cập nhật khi đang chạy hoặc thay đổi trong Editor.")]
    [SerializeField] private bool autoUpdate = true;

    private float _lastAspect;
    private float _lastFov;
    private float _lastOrthoSize;
    private Vector2 _lastResolution;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        UpdateScale();
        CacheCameraState();
    }

    private void Update()
    {
        if (!autoUpdate || targetCamera == null)
            return;

        // Chỉ cập nhật khi có thay đổi thật sự để tránh tốn hiệu năng
        if (HasCameraChanged())
        {
            UpdateScale();
            CacheCameraState();
        }
    }

#if UNITY_EDITOR
    // Tự động cập nhật trong Editor khi bạn thay đổi giá trị
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (targetCamera == null)
                targetCamera = Camera.main;

            UpdateScale();
        }
    }
#endif

    // ============================================================
    // === CORE SCALING LOGIC =====================================
    // ============================================================
    public void UpdateScale()
    {
        if (targetCamera == null)
            return;

        if (targetCamera.orthographic)
            ScaleForOrthographic();
        else
            ScaleForPerspective();
    }

    private void ScaleForOrthographic()
    {
        float height = targetCamera.orthographicSize * 2f;
        float width = height * targetCamera.aspect;

        transform.localScale = new Vector3(width / 10, 1f, height / 10);
        transform.position = targetCamera.transform.position + targetCamera.transform.forward * distanceFromCamera;
    }

    private void ScaleForPerspective()
    {
        float fovRad = targetCamera.fieldOfView * Mathf.Deg2Rad;
        float height = 2f * Mathf.Tan(fovRad * 0.5f) * distanceFromCamera;
        float width = height * targetCamera.aspect;

        transform.localScale = new Vector3(width, 1f, height);
        transform.position = targetCamera.transform.position + targetCamera.transform.forward * distanceFromCamera;
    }

    // ============================================================
    // === CAMERA STATE TRACKING ==================================
    // ============================================================
    private bool HasCameraChanged()
    {
        if (_lastAspect != targetCamera.aspect ||
            _lastFov != targetCamera.fieldOfView ||
            _lastOrthoSize != targetCamera.orthographicSize ||
            _lastResolution.x != Screen.width ||
            _lastResolution.y != Screen.height)
            return true;

        return false;
    }

    private void CacheCameraState()
    {
        _lastAspect = targetCamera.aspect;
        _lastFov = targetCamera.fieldOfView;
        _lastOrthoSize = targetCamera.orthographicSize;
        _lastResolution = new Vector2(Screen.width, Screen.height);
    }

}
