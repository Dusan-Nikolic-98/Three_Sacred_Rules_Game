using UnityEngine;

public class CameraTriggerBack : MonoBehaviour
{
    public CameraShift cameraShift;
    public int lvl = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            cameraShift.ShiftCameraLeft(lvl);
        }
    }

}
