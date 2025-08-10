using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraShift cameraShift;
    public int lvl = 1;
    //public bool shiftRight = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraShift.ShiftCameraRight(lvl);
            
        }
    }

}
