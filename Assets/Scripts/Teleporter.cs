using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destination;  // Lokasi tujuan teleport
    public float fadeDuration = 0.5f;  // Durasi fade in/out
    
    [Header("Camera Zone Tujuan (Opsional)")]
    [Tooltip("Jika di-assign, kamera akan ganti zone saat teleport")]
    public CameraZone destinationZone;

    [Header("Fade Panel (Auto-created if null)")]
    public Image fadePanel;

    private bool isTeleporting = false;

    void Start()
    {
        // Buat fade panel otomatis jika belum ada
        if (fadePanel == null)
        {
            CreateFadePanel();
        }
    }

    void CreateFadePanel()
    {
        // Cari atau buat Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("FadeCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
            canvasObj.AddComponent<CanvasScaler>();
        }

        // Buat panel hitam
        GameObject panelObj = new GameObject("FadePanel");
        panelObj.transform.SetParent(canvas.transform, false);
        fadePanel = panelObj.AddComponent<Image>();
        fadePanel.color = new Color(0, 0, 0, 0);  // Transparan
        fadePanel.raycastTarget = false;

        // Stretch ke seluruh layar
        RectTransform rect = fadePanel.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected: " + other.gameObject.name + " | Tag: " + other.tag);
        
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Object bukan Player! Pastikan Player punya Tag 'Player'");
            return;
        }
        
        if (destination == null)
        {
            Debug.LogError("Destination belum di-assign!");
            return;
        }
        
        if (isTeleporting)
        {
            Debug.Log("Masih teleporting...");
            return;
        }
        
        Debug.Log("Mulai teleport!");
        StartCoroutine(TeleportWithFade(other.transform));
    }

    IEnumerator TeleportWithFade(Transform player)
    {
        isTeleporting = true;

        // Fade ke hitam
        yield return StartCoroutine(Fade(0, 1));

        // Teleport player
        player.position = destination.position;

        // Ganti camera zone jika ada
        if (destinationZone != null && CameraFollow.Instance != null)
        {
            CameraFollow.Instance.SetZone(destinationZone);
        }

        // Fade ke terang
        yield return StartCoroutine(Fade(1, 0));

        isTeleporting = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadePanel.color = color;
    }
}
