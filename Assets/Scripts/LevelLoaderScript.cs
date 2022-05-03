using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadLevel(int a_levelIndex)
	{
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
	}
}
