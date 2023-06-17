using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTest : MonoBehaviour
{
    public Canvas canvas;
    public Camera cam;
    public Image find; //箭头UI
    public GameObject player; //玩家
    public GameObject target; //跟踪目标
    List<Vector3> points = new List<Vector3>(); //canvas边界点
    bool isHaveIntersection = false; //判断有没有交点来显隐箭头
    public float targetOffset; //设置目标点偏移，用来控制箭头提前或者延后显隐


    void Start()
    {
        //存储canvas边界点
        points.Add(new Vector3(-canvas.GetComponent<RectTransform>().rect.width / 2f, -canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
        points.Add(new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2f, -canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
        points.Add(new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2f, canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
        points.Add(new Vector3(-canvas.GetComponent<RectTransform>().rect.width / 2f, canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
    }

    void Update()
    {
        Vector3 pos = Vector3.zero;
        Vector3 pos1 = Camera.main.WorldToScreenPoint(target.transform.position - targetOffset * (target.transform.position - player.transform.position).normalized); //目标屏幕坐标
        Vector3 pos2 = Camera.main.WorldToScreenPoint(player.transform.position); //玩家屏幕坐标
        Vector2 worldPoint1;
        Vector2 worldPoint2;

        //求出目标与玩家的UI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos1, cam, out worldPoint1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos2, cam, out worldPoint2);

        isHaveIntersection = false; //默认没交点

        //玩家与目标UI坐标连线与Canvas边界连写交点即为箭头位置
        for (int i = 0; i < points.Count; i++)
        {
            //这里的if是为了让最后一条边界为 ps[Count - 1]与ps[0]连线
            if (i < points.Count - 1)
            {
                if (SegmentsInterPoint(worldPoint1, worldPoint2, points[i + 1], points[i], ref pos))
                {
                    find.rectTransform.anchoredPosition = pos;
                    isHaveIntersection = true;
                    break;
                }
            }
            else
            {
                if (SegmentsInterPoint(worldPoint1, worldPoint2, points[i], points[0], ref pos))
                {
                    find.rectTransform.anchoredPosition = pos;
                    isHaveIntersection = true;
                    break;
                }
            }
        }

        //判断有没有交点来显隐箭头
        if (isHaveIntersection)
        {
            find.gameObject.SetActive(true);

            //箭头朝向目标
            UILookAt(find.transform, worldPoint1 - worldPoint2, Vector3.up);
        }
        else
        {
            find.gameObject.SetActive(false);
        }
    }

    //参数分别为：1.UI的Transform 2.朝向向量 3.起始向量
    public void UILookAt(Transform transform, Vector3 dir, Vector3 lookAxis)
    {
        Quaternion q = Quaternion.identity;
        q.SetFromToRotation(lookAxis, dir);
        transform.rotation = q;
    }

    public static float Cross(Vector3 a, Vector3 b)
    {
        return a.x * b.y - b.x * a.y;
    }

    //求交点
    public static bool SegmentsInterPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref Vector3 IntrPos)
    {

        //v1×v2=x1y2-y1x2 
        //以线段ab为准，是否c，d在同一侧
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        float abXac = Cross(ab, ac);

        Vector3 ad = d - a;
        float abXad = Cross(ab, ad);

        if (abXac * abXad >= 0)
        {
            return false;
        }

        //以线段cd为准，是否ab在同一侧
        Vector3 cd = d - c;
        Vector3 ca = a - c;
        Vector3 cb = b - c;

        float cdXca = Cross(cd, ca);
        float cdXcb = Cross(cd, cb);
        if (cdXca * cdXcb >= 0)
        {
            return false;
        }
        //计算交点坐标  
        float t = Cross(a - c, d - c) / Cross(d - c, b - a);
        float dx = t * (b.x - a.x);
        float dy = t * (b.y - a.y);

        IntrPos = new Vector3() { x = a.x + dx, y = a.y + dy };
        return true;
    }
}
