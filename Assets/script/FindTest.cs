using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTest : MonoBehaviour
{
    public Canvas canvas;
    public Camera cam;
    public Image find; //��ͷUI
    public GameObject player; //���
    public GameObject target; //����Ŀ��
    List<Vector3> points = new List<Vector3>(); //canvas�߽��
    bool isHaveIntersection = false; //�ж���û�н�����������ͷ
    public float targetOffset; //����Ŀ���ƫ�ƣ��������Ƽ�ͷ��ǰ�����Ӻ�����


    void Start()
    {
        //�洢canvas�߽��
        points.Add(new Vector3(-canvas.GetComponent<RectTransform>().rect.width / 2f, -canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
        points.Add(new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2f, -canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
        points.Add(new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2f, canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
        points.Add(new Vector3(-canvas.GetComponent<RectTransform>().rect.width / 2f, canvas.GetComponent<RectTransform>().rect.height / 2f, 0));
    }

    void Update()
    {
        Vector3 pos = Vector3.zero;
        Vector3 pos1 = Camera.main.WorldToScreenPoint(target.transform.position - targetOffset * (target.transform.position - player.transform.position).normalized); //Ŀ����Ļ����
        Vector3 pos2 = Camera.main.WorldToScreenPoint(player.transform.position); //�����Ļ����
        Vector2 worldPoint1;
        Vector2 worldPoint2;

        //���Ŀ������ҵ�UI����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos1, cam, out worldPoint1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos2, cam, out worldPoint2);

        isHaveIntersection = false; //Ĭ��û����

        //�����Ŀ��UI����������Canvas�߽���д���㼴Ϊ��ͷλ��
        for (int i = 0; i < points.Count; i++)
        {
            //�����if��Ϊ�������һ���߽�Ϊ ps[Count - 1]��ps[0]����
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

        //�ж���û�н�����������ͷ
        if (isHaveIntersection)
        {
            find.gameObject.SetActive(true);

            //��ͷ����Ŀ��
            UILookAt(find.transform, worldPoint1 - worldPoint2, Vector3.up);
        }
        else
        {
            find.gameObject.SetActive(false);
        }
    }

    //�����ֱ�Ϊ��1.UI��Transform 2.�������� 3.��ʼ����
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

    //�󽻵�
    public static bool SegmentsInterPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref Vector3 IntrPos)
    {

        //v1��v2=x1y2-y1x2 
        //���߶�abΪ׼���Ƿ�c��d��ͬһ��
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        float abXac = Cross(ab, ac);

        Vector3 ad = d - a;
        float abXad = Cross(ab, ad);

        if (abXac * abXad >= 0)
        {
            return false;
        }

        //���߶�cdΪ׼���Ƿ�ab��ͬһ��
        Vector3 cd = d - c;
        Vector3 ca = a - c;
        Vector3 cb = b - c;

        float cdXca = Cross(cd, ca);
        float cdXcb = Cross(cd, cb);
        if (cdXca * cdXcb >= 0)
        {
            return false;
        }
        //���㽻������  
        float t = Cross(a - c, d - c) / Cross(d - c, b - a);
        float dx = t * (b.x - a.x);
        float dy = t * (b.y - a.y);

        IntrPos = new Vector3() { x = a.x + dx, y = a.y + dy };
        return true;
    }
}
