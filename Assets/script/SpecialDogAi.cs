using UnityEngine;
using Pathfinding;

public class SpecialDogAi : MonoBehaviour
{
    public GameObject go;
    public GameObject targetGo;
    public AIPath AiPath;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Patrol patrol;

    private bool IsAttack;
    private bool isHide;

    private float distance;
    private AIDestinationSetter AiChase;
    float WaitTime = 0.5f;
    float Wait = 4f;
    bool IsFrozen;
    private Color color;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        patrol = GetComponent<Patrol>();
        AiChase = GetComponent<AIDestinationSetter>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = (go.transform.position - targetGo.transform.position).sqrMagnitude;
        if (!IsFrozen)
        {
            if (!IsAttack)
            {
                if (distance <= 50 & !isHide)
                {
                    patrol.enabled = false;
                    if (WaitTime <= 0)
                    {
                        AiPath.maxSpeed = 1.85f;
                        AiPath.maxAcceleration = 10f;
                        AiChase.enabled = true;
                    }
                    else
                    {
                        WaitTime -= Time.deltaTime;
                    }

                }
                else
                {
                    AiPath.maxSpeed = 1f;
                    AiPath.maxAcceleration = 5f;
                    patrol.enabled = true;
                    AiChase.enabled = false;
                    WaitTime = 0.7f;
                }
            }
            else
            {

                if (Wait >= 0)
                {
                    AiPath.maxSpeed = 0f;
                    Wait -= Time.deltaTime;
                }
                else
                {
                    AiPath.maxSpeed = 1f;
                    AiPath.maxAcceleration = 5f;
                    patrol.enabled = true;
                    AiChase.enabled = false;
                    IsAttack = false;
                    Wait = 4f;

                }
            }
        }
        else
        {
            AiPath.maxSpeed = 0.8f;
        }
    }
    void FixedUpdate()
    {
        SwitchAnim();
    }
    void SwitchAnim()
    {
        anim.SetFloat("Horizontal", AiPath.desiredVelocity.x);
        anim.SetFloat("Vertical", AiPath.desiredVelocity.y);
        anim.SetFloat("magnitude", 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsAttack = true;
        }
    }
    public void setForzen(bool forzen)
    {
        IsFrozen = forzen;
        if (IsFrozen)
        {
            SetColor();
        }
        else
        {
            ResetColor();
        }
    }
    public void setHide(bool hide)
    {
        isHide = hide;
    }
    void SetColor()
    {
        sr.color = new Color(0.5f, 0.5f, 1);
    }
    void ResetColor()
    {
        sr.color = color;
    }
}
