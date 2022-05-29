using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnimEnd : MonoBehaviour
{
    private EnemyHealth EH;

    // Start is called before the first frame update
    void Start()
    {
        EH = transform.parent.GetComponent<EnemyHealth>();
    }

    public void AnimationEnd()
    {
        EH.animFinished = true;
    }
}
