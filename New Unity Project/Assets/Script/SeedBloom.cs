using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBloom : MonoBehaviour
{
    public List<GameObject> start_end;

    private SpriteRenderer sprite;
    private BoxCollider2D bc2d;
    private bool findBottom = false;
    private float delayTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        bc2d.enabled = false;
        SeedBlooming();
    }

    // Update is called once per frame
    void Update()
    {
        if (!findBottom)
            SeedBlooming();
        else if(findBottom)
        {
            if(delayTime>3)
            {
                bc2d.enabled = true;
            }
            else
            {
                sprite.color = new Color(255, 255, 255, delayTime / 3f);
                delayTime += Time.deltaTime;
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
