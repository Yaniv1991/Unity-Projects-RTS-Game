using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utils
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }


}


public class RTSController : MonoBehaviour
{

    public LayerMask layermask;
    public LayerMask Enemy;

    public List<FriendlyUnit> FriendlyUnits;
    List<FriendlyUnit> SelectedUnits = new List<FriendlyUnit>();

    bool isSelecting;
    Vector3 mousePosition1, mousePosition2;
    RaycastHit hitinfo;
    private Ray ray
    {
        get
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                SelectedUnits[i].Deselect();
            }
            SelectedUnits.Clear();
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            for (int i = 0; i < FriendlyUnits.Count; i++)
            {
                if (IsWithinSelectionBounds(FriendlyUnits[i].gameObject))
                {
                    FriendlyUnits[i].Select();
                    SelectedUnits.Add(FriendlyUnits[i]);
                }
                else
                {
                    FriendlyUnits[i].Deselect();
                }
            }

            if (Physics.Raycast(ray, out hitinfo, 10000f, layermask))
            {

                if (hitinfo.transform.gameObject.layer == 8)
                {
                    SelectedUnits.Add(hitinfo.transform.gameObject.GetComponent<FriendlyUnit>());
                    foreach (var unit in SelectedUnits)
                    {
                        unit.Select();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

            if (Physics.Raycast(ray, out hitinfo, 10000f, Enemy))
            {
                Debug.Log("Attacking");
                foreach (var unit in SelectedUnits)
                {
                    unit.Attack(hitinfo.transform.gameObject);
                }
            }
            else
            {
                float distance;
                Plane plane = new Plane(Vector3.up, Vector3.up);
                plane.Raycast(ray, out distance);
                foreach (var unit in SelectedUnits)
                {
                    unit.MoveTo(ray.GetPoint(distance));
                }
            }
        }





    }

    bool IsWithinSelectionBounds(GameObject gameObject)
    {
        Camera camera = Camera.main;
        Bounds viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}




