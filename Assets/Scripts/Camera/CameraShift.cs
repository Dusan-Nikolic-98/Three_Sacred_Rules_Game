using System.Collections;
using UnityEngine;

public class CameraShift : MonoBehaviour
{

    public Transform leftCamPos;
    public Transform rightCamPos;

    public Transform leftCamPosL2;
    public Transform rightCamPosL2;

    public Transform leftCamPosL3;
    public Transform rightCamPosL3;

    public float moveDuration = 1f;
    private bool isMoving = false;
    private bool isOnRight = false;

    public void ShiftCameraRight(int lvl)
    {
        if (!isMoving && !isOnRight)
        {
            switch (lvl) 
            {
                case 1:
                    StartCoroutine(MoveCamera(rightCamPos.position, true));
                    break;
                case 2:
                    StartCoroutine(MoveCamera(rightCamPosL2.position, true));
                    break;
                case 3:
                    StartCoroutine(MoveCamera(rightCamPosL3.position, true));
                    break;

            }
            
        }
    }
    public void ShiftCameraLeft(int lvl)
    {
        if (!isMoving && isOnRight)
        {
            switch (lvl)
            {
                case 1:
                    StartCoroutine(MoveCamera(leftCamPos.position, false));
                    break;
                case 2:
                    StartCoroutine(MoveCamera(leftCamPosL2.position, false));
                    break;
                case 3:
                    StartCoroutine(MoveCamera(leftCamPosL3.position, false));
                    break;
            }
        }
    }

    private IEnumerator MoveCamera(Vector3 targetPos, bool goingRight)
    {
        isMoving = true;
        //friz svega
        Time.timeScale = 0f;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration) 
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;        
        }
        transform.position = targetPos;

        //anfriz
        Time.timeScale = 1f;

        isOnRight = goingRight;
        isMoving = false;
    }
}
