using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingNew : MonoBehaviour
{
    public float m_Speed;
    public float m_AttackDistance;
    public float HitForce;
    float minDistance = Mathf.Infinity;

    private bool m_MovingToTarget;

    private Transform m_Player;
    //private List<Transform> m_EnemyTransforms;
    //private Transform[] m_EnemyTransformsArr;
    public GameObject[] m_Enemies;

    //private Transform m_ClosestEnemy;

    private Vector3 m_TargetPos;
    private Vector2 m_TargetDir;
    //private Vector3 m_ClosestEnemyPos;
    private Vector3 m_MovementDirection;
    private Vector3 m_NewDestination;

    private float m_OrigPos;

    private SpriteRenderer m_SpriteRenderer;

    private Rigidbody2D rb;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_Player = GameObject.Find("Player").GetComponent<Transform>();

        m_OrigPos = transform.position.x;

        rb = GetComponent<Rigidbody2D>();
        //m_Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //m_EnemyTransforms = new List<Transform>(m_Enemies.Length);

        //GetEnemies();
    }

    //private void GetEnemies()
    //{
    //    foreach (GameObject enemy in m_Enemies)
    //    {
    //        m_EnemyTransforms.Add(enemy.GetComponent<Transform>());
    //    }

    //    m_EnemyTransformsArr = m_EnemyTransforms.ToArray();
    //}

    Transform GetClosestEnemy(Transform[] Enemies)
    {
        Transform tMin = null;

        foreach (Transform t in Enemies)
        {
            float distance = Vector3.Distance(t.position, transform.position);
            if (distance < minDistance)
            {
                tMin = t;
                minDistance = distance;
            }
        }
        return tMin;
    }

    private void EnemyFacing()
    {
        //if (collisionCount == 0)
        //{

        if (m_TargetDir.x > 0)
        {
            m_SpriteRenderer.flipX = false;
        }

        if (m_TargetDir.x < 0)
        {
            m_SpriteRenderer.flipX = true;
        }

        //m_OrigPos = transform.position.x;
        //}
    }

    #region __CHECK_COLLISIONS__

    [SerializeField] private int collisionCount = 0;

    public bool NotColliding
    {
        get { return collisionCount == 0; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        if (collider.name != "Floor" || collider.tag != "Enemy")
        {
            //Debug.Log(collider.name);
            collisionCount++;
        }

        if (collider.name == "Player")
        {
            Debug.Log("collision");

            if ((transform.position.x - collider.transform.position.x) < 0)
            {
                Debug.Log("Left Hit");
                rb.AddForce(new Vector2(-1f, 0.5f) * HitForce);
            }
            else if ((transform.position.x - collider.transform.position.x) > 0)
            {
                Debug.Log("Right Hit");
                rb.AddForce(new Vector2(1f, 0.5f) * HitForce);
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisionCount--;
    }

    #endregion __CHECK_COLLISIONS_END__

    void ChangeDestination()
    {
        //m_NewDestination = new Vector2(Random.Range())
    }

    private void Update()
    {

        //m_ClosestEnemy = GetClosestEnemy(m_EnemyTransformsArr);
        //m_ClosestEnemyPos = new Vector3(m_ClosestEnemy.position.x, -3.48f, m_ClosestEnemy.position.z);

        m_TargetPos = new Vector3(m_Player.position.x, -3.48f, m_Player.position.z);
        m_TargetDir = (m_TargetPos - transform.position).normalized;

        EnemyFacing();

        if (m_MovementDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(m_MovementDirection.y, m_MovementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        if (Vector2.Distance(transform.position, m_TargetPos) > m_AttackDistance /*|| Vector2.Distance(transform.position, m_ClosestEnemyPos) > m_AttackDistance*/)
        {
            rb.AddForce(m_TargetDir * m_Speed);
            m_MovingToTarget = true;
            //transform.position = Vector2.MoveTowards(transform.position, m_TargetPos, m_Speed * Time.deltaTime);
        }
        else
        {
            m_MovingToTarget = false;
        }
    }
}
