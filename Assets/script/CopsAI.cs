using UnityEngine;
using Pathfinding;

public class CopsAI : MonoBehaviour
{
    public GameObject go;
    public GameObject targetGo;
    public GameObject Trap;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Patrol patrol;
    private bool IsAttack;
    private bool isHide;

    public AIPath AiPath;

    private float distance;
    public float SetDistance;
    private AIDestinationSetter AiChase;
    float WaitTime = 0.5f;
    float Wait = 3.5f;
    float WaitPlace = 1.5f;
    float time;
    
    float CD;

    bool IsFrozen;
    bool ChaseStart;

    public Transform TrapPoint;

    private Color color;
    private SpriteRenderer sr;
    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        patrol = GetComponent<Patrol>();
        AiChase = GetComponent<AIDestinationSetter>();
        CD = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        distance = (go.transform.position - targetGo.transform.position).sqrMagnitude;
        if (!IsFrozen)
            {
                if (!IsAttack)
                {
                    if (distance <= SetDistance & !isHide)
                    {
                        patrol.enabled = false;
                        if (WaitTime <= 0)
                        {
                            AiPath.maxSpeed = 1.65f;
                            AiPath.maxAcceleration = 10f;
                            AiChase.enabled = true;
                            ChaseStart = true;
                            
                        }
                        else
                        {
                            WaitTime -= Time.deltaTime;
                        }

                    }
                    else
                    {
                        ChaseStart = false;
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
                        ChaseStart = false;
                        Wait = 4f;
                    }
                }
            }
            else
            {
                AiPath.maxSpeed = 0f;
                rb.velocity =new Vector2(0,0);
            
            }

        Skill();
    }
    private void FixedUpdate()
    {
        SwitchAnim();
    }
    void SwitchAnim()
    {
        anim.SetFloat("Horizontal", AiPath.desiredVelocity.x);
        anim.SetFloat("Vertical", AiPath.desiredVelocity.y);
        if(AiPath.desiredVelocity.x==0& AiPath.desiredVelocity.y == 0)
        {
            anim.SetFloat("magnitude", 0);
        }
        else
        {
            anim.SetFloat("magnitude", 1);
        }
        
    }
    void Skill()
    {
        if (ChaseStart)
        {
            if (CD <= 0)
            {
                if (WaitPlace <= 0)
                {
                    Instantiate(Trap, TrapPoint.position, TrapPoint.rotation);
                    WaitPlace = 1.5f;
                    CD = 30f;
                }
                else
                {
                    WaitPlace -= Time.deltaTime;
                }
                IsAttack = true;
            }
            else
            {
                CD -= Time.deltaTime;
            }
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
