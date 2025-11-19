using UnityEngine;

public class BuildStretchZone : MonoBehaviour
{
    [Header("要建造的 StretchZone 预制体")]
    public GameObject stretchZonePrefab;

    [Header("进入/退出建造模式的按键")]
    public KeyCode buildKey = KeyCode.Q;

    [Header("滚轮旋转速度（每滚一格旋转多少度）")]
    public float rotationStep = 15f;

    [Header("预览的透明度（0~1）")]
    [Range(0f, 1f)]
    public float previewAlpha = 0.5f;

    private bool isBuilding = false;
    private GameObject previewInstance;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("BuildStretchZone：场景中找不到 MainCamera，记得给相机打上 MainCamera 标签！");
        }
    }

    void Update()
    {
        // 切换建造模式
        if (Input.GetKeyDown(buildKey))
        {
            if (!isBuilding)
            {
                EnterBuildMode();
            }
            else
            {
                ExitBuildMode(false); // false = 不建造，直接退出
            }
        }

        if (!isBuilding) return;

        if (previewInstance == null)
        {
            // 万一被销毁了，自动退出建造模式
            ExitBuildMode(false);
            return;
        }

        UpdatePreviewPositionAndRotation();

        // 左键在当前位置正式建造 StretchZone
        if (Input.GetMouseButtonDown(0))
        {
            ConfirmBuild();
        }
    }

    void EnterBuildMode()
    {
        if (stretchZonePrefab == null)
        {
            Debug.LogError("BuildStretchZone：stretchZonePrefab 没有设置！");
            return;
        }

        isBuilding = true;

        // 创建预览物体（用 StretchZone 的 prefab）
        previewInstance = Instantiate(stretchZonePrefab);
        previewInstance.name = "StretchZone_Preview";

        // 让预览的 collider 不参与物理（可选）
        var col = previewInstance.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // 让预览半透明一点
        var sprite = previewInstance.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = previewAlpha;
            sprite.color = c;
        }

        var particleSystem = previewInstance.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    /// <summary>
    /// 退出建造模式
    /// </summary>
    /// <param name="built">是否是因为建造完成而退出</param>
    void ExitBuildMode(bool built)
    {
        isBuilding = false;

        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }

        // 这里你也可以根据 built 做一些 UI 提示等
    }

    void UpdatePreviewPositionAndRotation()
    {
        if (mainCam == null) return;

        // 鼠标位置 → 世界坐标
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        previewInstance.transform.position = mouseWorldPos;

        // 滚轮旋转（鼠标滚轴上下）
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            float angleDelta = -scroll * rotationStep;  // 方向看手感，可以换成 +scroll
            previewInstance.transform.Rotate(0f, 0f, angleDelta);
        }
    }

    void ConfirmBuild()
    {
        if (stretchZonePrefab == null || previewInstance == null)
            return;

        // 在预览位置正式生成一个 StretchZone
        Vector3 pos = previewInstance.transform.position;
        Quaternion rot = previewInstance.transform.rotation;

        Instantiate(stretchZonePrefab, pos, rot);

        // 退出建造模式（建造成功）
        ExitBuildMode(true);
    }
}
