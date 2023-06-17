using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public GameObject go;
    public GameObject targetGo;

    public Image labelUI_img;
    
    private float labelUI_HalfWidth;
    private float labelUI_HalfHeight;
    private Vector2 v3;
    private Vector2 v4;
    private float distance;

    private void Awake()
    {
        labelUI_HalfWidth = labelUI_img.GetComponent<RectTransform>().rect.width / 2;
        labelUI_HalfHeight = labelUI_img.GetComponent<RectTransform>().rect.height / 2;
    }

    private void Update()
    {
        distance = (go.transform.position - targetGo.transform.position).sqrMagnitude;
        // Determine whether the markup UI is displayed or hidden
        if (IsInView(targetGo.transform.position))
        {
            labelUI_img.enabled = false;
        }
        else
        {
            if (distance <= 80)
            {
                labelUI_img.enabled = true;
            }
            else
            {
                labelUI_img.enabled = false;
            }
            
        }

        // Sets the display position of the markup UI
        GetV3AndV4(out v3, out v4);
        labelUI_img.transform.localPosition = CalculateCrossPoint(go.transform.position, targetGo.transform.position, v3, v4);
    }

    
    private void GetV3AndV4(out Vector2 v3, out Vector2 v4)
    {
        Vector2 v1 = Camera.main.WorldToScreenPoint(go.transform.position);
        Vector2 v2 = Camera.main.WorldToScreenPoint(targetGo.transform.position);
        Vector2 offset = v2 - v1;
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            if (offset.x > 0)
            {
                v3 = new Vector2(Screen.width - labelUI_HalfWidth, 0);
                v4 = new Vector2(Screen.width - labelUI_HalfWidth, Screen.height);                
            }
            else
            {
                v3 = new Vector2(0 + labelUI_HalfWidth, 0);
                v4 = new Vector2(0 + labelUI_HalfWidth, Screen.height);                
            }
        }
        else
        {
            if (offset.y > 0)
            {
                v3 = new Vector2(0, Screen.height - labelUI_HalfHeight);
                v4 = new Vector2(Screen.width, Screen.height - labelUI_HalfHeight);               
            }
            else
            {
                v3 = new Vector2(0, 0 + labelUI_HalfHeight);
                v4 = new Vector2(Screen.width, 0 + labelUI_HalfHeight);
            }
        }
    }
    // Calculate Cross Point
    private Vector2 CalculateCrossPoint(Vector3 v1, Vector3 v2, Vector2 v3, Vector2 v4)
    {
        v1 = Camera.main.WorldToScreenPoint(v1);
        v2 = Camera.main.WorldToScreenPoint(v2);

        float a1 = 0, b1 = 0, a2 = 0, b2 = 0;
        Vector2 crossPoint = Vector2.zero;

        if (v1.x != v2.x)
        {
            a1 = (v1.y - v2.y) / (v1.x - v2.x);
        }
        if (v1.y != v2.y)
        {
            b1 = v1.y - v1.x * (v1.y - v2.y) / (v1.x - v2.x);
        }

        if (v3.x != v4.x)
        {
            a2 = (v3.y - v4.y) / (v3.x - v4.x);
        }
        if (v3.y != v4.y)
        {
            b2 = v3.y - v3.x * (v3.y - v4.y) / (v3.x - v4.x);
        }

        if (a1 == a2 && b1 == b2)
        {
            return Vector2.zero;
        }
        else if (a1 == a2)
        {
            return Vector2.zero;
        }
        else
        {
            float x = 0;
            float y = 0;
            if (v3.x == v4.x)
            {
                if (v3.x == 0)
                {
                    x = 0;
                    y = b1;
                }
                else
                {
                    x = v3.x;
                    y = x * a1 + b1;
                }
            }
            else if (v3.y == v4.y)
            {
                if (v3.y == 0)
                {
                    y = 0;
                    x = -b1 / a1;
                }
                else
                {
                    y = v3.y;
                    x = (y - b1) / a1;
                }
            }
            else
            {
                x = (b2 - b1) / (a1 - a2);
                y = a1 * (b2 - b1) / (a1 - a2) + b1;
            }

            x = Mathf.Clamp(x, 0, Screen.width);
            y = Mathf.Clamp(y, 0, Screen.height);

            crossPoint = new Vector2(x, y);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").GetComponent<RectTransform>(),
                crossPoint, null, out crossPoint);
            return crossPoint;
        }
    }

    public bool IsInView(Vector3 worldPos)
    {
        Transform camTransform = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);

        // Determine whether the object is in front of the camera
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);

        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
