using UnityEngine;

public class BoundaryDestroy : MonoBehaviour
{
    private Camera cameraMain;
    private Vector3 worldPointMax;
    private Vector3 worldPointMin;

    private void Start()
    {
        cameraMain = Camera.main;
        worldPointMax = cameraMain.ScreenToWorldPoint(new Vector3(Screen.width + 100, Screen.height + 100, 0));
        worldPointMin = cameraMain.ScreenToWorldPoint(new Vector3(-100, -100, 0));
    }

    private void Update()
    {
        if (transform.position.x > worldPointMax.x || transform.position.x < worldPointMin.x ||
            transform.position.y > worldPointMax.y || transform.position.y < worldPointMin.y)
        {
            Destroy(gameObject);
        }
    }
}
