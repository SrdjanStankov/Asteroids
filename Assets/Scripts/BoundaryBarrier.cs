using UnityEngine;

public class BoundaryBarrier : MonoBehaviour
{
    private Camera cameraMain;

    private void Start()
    {
        cameraMain = Camera.main;
    }

    private void Update()
    {
        var negativeBoundary = cameraMain.ScreenToWorldPoint(new Vector3(50, 50, transform.position.z));
        var positiveBoundary = cameraMain.ScreenToWorldPoint(new Vector3(Screen.width - 50, Screen.height - 50, transform.position.z));

        if (transform.position.x < negativeBoundary.x)
        {
            transform.position = new Vector3(negativeBoundary.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < negativeBoundary.y)
        {
            transform.position = new Vector3(transform.position.x, negativeBoundary.y, transform.position.z);
        }

        if (transform.position.x > positiveBoundary.x)
        {
            transform.position = new Vector3(positiveBoundary.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y > positiveBoundary.y)
        {
            transform.position = new Vector3(transform.position.x, positiveBoundary.y, transform.position.z);
        }
    }
}
