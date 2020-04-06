using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBloom : MonoBehaviour
{
    public List<GameObject> start_end;
    public GameObject cloneBoss;
    public float creationTime;
    public float createDelay;

    private SpriteRenderer sprite;
    private BoxCollider2D bc2d;
    private bool findBottom = false;
    private float createDelayTime = 0f;
    private bool createObject = false;
    private float creationBossTime = 0f;
    private int hp = 3;


    // Start is called before the first frame update
    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        bc2d.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        FindingBottom();
        CreationObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            Set_hp(bullet.Get_Damage());
        }
    }

    public void Set_hp(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            DestroyObject(this.gameObject);
        }
    }

    void CreationObject()
    {
        if(createObject)
        {
            if(creationBossTime>creationTime)
            {
                GameObject inst = Instantiate(cloneBoss) as GameObject;
                inst.gameObject.transform.position = gameObject.transform.position;
                DestroyObject(this.gameObject);
            }
            else
            {
                creationBossTime += Time.deltaTime;
            }
        }
    }

    void FindingBottom()
    {
        if (!findBottom)
            SeedBlooming();
        else if (findBottom)
        {
            if (createDelayTime > createDelay)
            {
                bc2d.enabled = true;
                createObject = true;
            }
            else
            {
                sprite.color = new Color(255, 255, 255, createDelayTime / 3f);
                createDelayTime += Time.deltaTime;
            }
        }
    }

    public void SeedBlooming()
    {
        float randamPosX = Random.Range(start_end[0].transform.position.x, start_end[1].transform.position.x);
        float randomPosY = Random.Range(start_end[0].transform.position.y, start_end[1].transform.position.y);
        Vector2 randomPos = new Vector2(randamPosX, randomPosY - 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(randomPos, Vector2.down, 5f);
        
        if (hit.collider!=null)
        {
            if (hit.collider.name == "Bottom")
            {
                Debug.Log(hit.point);
                Vector2 pointVector = new Vector2(hit.point.x, hit.point.y+0.15f);
                gameObject.transform.position = pointVector;
                findBottom = true;
            }
        }
    }
}
