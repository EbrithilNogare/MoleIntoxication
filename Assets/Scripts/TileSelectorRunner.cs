using UnityEngine;

public class TileSelectorRunner : MonoBehaviour
{
    public GameObject selector;
    void Start()
    {
        selector.transform.position = new Vector3(-1, 0, 0);
    }
    void Update()
    {
        float cameraSize = Camera.main.orthographicSize;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraSize));

        int x = (int)System.Math.Round(worldMousePosition.x) + (int)(Constants.MAP_WIDTH / 2);
        int y = (int)System.Math.Round(worldMousePosition.y + .5);

        // Debug.Log("hover: " + x + " " + y);

        if (y > 0 || x < 0 || x >= Constants.MAP_WIDTH)
            selector.transform.localPosition = new Vector3(-1, 0, 0);
        else
            selector.transform.localPosition = new Vector3(x, y, 0);
    }
}
