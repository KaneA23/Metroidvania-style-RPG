using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernardAnimationSystem : MonoBehaviour
{
    private PolygonCollider2D[] colliders;
    private EdgeCollider2D headCollider;
    private Animator animator;
    private AnimatorClipInfo[] clipInfo;

    private SpriteRenderer spriteRenderer;

    private Vector2[] headColliderPoints;

    private int currentAnimFrame;
    private int collNum = 0;

    public string currentAnimName;
    public string currentAnimNameNew;

    private void Awake()
    {
        colliders = GetComponents<PolygonCollider2D>();
        headCollider = GetComponent<EdgeCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //clipInfo = animator.GetCurrentAnimatorClipInfo(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        headColliderPoints = headCollider.points;
    }

    void FixedUpdate()
    {
        //currentAnimFrame = (int)(clipInfo[0].weight * (clipInfo[0].clip.length * clipInfo[0].clip.frameRate));

        currentAnimNameNew = spriteRenderer.sprite.name;

        if (collNum > colliders.Length - 1)
        {
            collNum = 0;
        }

        if (currentAnimNameNew == currentAnimName + "_0")
        {
            headColliderPoints[0] = new Vector2(-0.9252879f, 0.27f);
            headColliderPoints[1] = new Vector2(-0.4612837f, 0.4746238f);

            headCollider.points = headColliderPoints;
        }
        else if (currentAnimNameNew == currentAnimName + "_1")
        {
            headColliderPoints[0] = new Vector2(-0.9252879f, 0.36f);
            headColliderPoints[1] = new Vector2(-0.4612837f, 0.4746238f);

            headCollider.points = headColliderPoints;
        }
        else if (currentAnimNameNew == currentAnimName + "_2")
        {
            headColliderPoints[0] = new Vector2(-0.9252879f, 0.45f);
            headColliderPoints[1] = new Vector2(-0.4612837f, 0.4746238f);

            headCollider.points = headColliderPoints;
        }
        else
        {
            headColliderPoints[0] = new Vector2(-0.9252879f, 0.45f);
            headColliderPoints[1] = new Vector2(-0.4612837f, 0.4746238f);

            headCollider.points = headColliderPoints;
        }

        collNum++;

        //for (int i = 0; i < colliders.Length; i++)
        //{

        //}

        //switch (currentAnimName)
        //{
        //    case "LizardIdle":
        //        if (currentAnimNameNew == currentAnimName + "_0")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 0)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_1")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 1)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_2")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 2)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_3")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 3)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_4")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 4)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_5")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 5)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_6")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 6)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_7")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 7)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_8")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 8)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_9")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 9)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_10")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 10)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_11")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 11)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        else if (currentAnimNameNew == currentAnimName + "_12")
        //        {
        //            for (int i = 0; i < colliders.Length; i++)
        //            {
        //                if (i == 12)
        //                {
        //                    colliders[i].enabled = true;
        //                }
        //                else
        //                {
        //                    colliders[i].enabled = false;
        //                }
        //            }
        //        }
        //        break;
        //    case "LizardRun":

        //        break;
        //}


    }
}
