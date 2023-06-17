using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAi : MonoBehaviour
{
    public bool IsFrozen;
    void Start()
    {

    }

    
    void Update()
    {
        
    }
    public bool GetFrozen()
    {
        return IsFrozen;
    }

    public void SetForzen(bool forzen)
    {
        IsFrozen = forzen;
    }
}
