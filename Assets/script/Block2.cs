using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block2 : MonoBehaviour
{
    public float speed;
    public Vector2[] movement;
    public float waitTime;
    private int flag;

    // Start is called before the first frame update
    void Start()
    {
        flag = 1;
        movement[1].x = transform.position.x;
        movement[1].y = transform.position.y + movement[1].y;
        movement[0].x = transform.position.x;
        movement[0].y = transform.position.y - movement[0].y;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, movement[flag], speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, movement[flag]) < 0.01f)
        {
            if (waitTime < 0.0f)
            {
                if (flag == 0)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
                waitTime = 7f;

            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

    }
}

