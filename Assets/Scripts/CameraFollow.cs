using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Settings")]
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        if (player == null)
        {
            // Cari player otomatis jika belum di-assign
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            return;
        }

        // Posisi target dengan offset
        Vector3 targetPosition = player.position + offset;

        // Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
