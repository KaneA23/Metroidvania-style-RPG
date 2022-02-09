using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : StateChange
{
    public GameObject m_Player;

    public void Enter(EnemyAgent agent)
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Exit(EnemyAgent agent)
    {
        
    }

    public EnemyState GetId()
    {
        return EnemyState.Chasing;
    }

    public void Update(EnemyAgent agent)
    {
       // if(Physics.Linecast(m_Player.transform.position, ))
    }
}
