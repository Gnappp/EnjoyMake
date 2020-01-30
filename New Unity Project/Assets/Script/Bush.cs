using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bush : MonoBehaviour
{
    class BushInObject
    {
        public Vector3Int objectPos { get; set; }
        public GameObject obj { get; set; }
        public BushInObject(Vector3Int pos,GameObject gameobject)
        {
            objectPos = pos;
            obj = gameobject;
        }
    }

    public GameObject mushroom;

    private HashSet<BushInObject> hideBushObjects;
    private Tilemap tileMap;
    private Vector3Int monsterPos = Vector3Int.zero;
    private Vector3Int playerPos = Vector3Int.zero;
    private float enterBushTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tileMap = gameObject.GetComponent<Tilemap>();
        hideBushObjects = new HashSet<BushInObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        EnterBush(collision);
        //if (collision.gameObject.layer == 8)
        //{
        //    if (enterBushTime > 4f)
        //    {
        //        GameObject inst = Instantiate(mushroom) as GameObject;
        //        inst.transform.position = collision.transform.position;
        //        enterBushTime = 0f;
        //    }
        //    else
        //        enterBushTime += Time.deltaTime;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitBush(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            hideBushObjects.Add(new BushInObject(Vector3Int.zero, collision.gameObject));
        }
        if(collision.gameObject.tag == "Player")
        {
            if(collision.GetType()==typeof(CapsuleCollider2D))
            {
                hideBushObjects.Add(new BushInObject(Vector3Int.zero, collision.gameObject));
            }
        }
    }

    void EnterBush(Collider2D collision)
    {
        foreach (BushInObject hidebush in hideBushObjects)
        {
            if (hidebush.obj.gameObject == collision.gameObject)
            {
                Vector3Int cellPos = tileMap.WorldToCell(collision.transform.position);
                if (hidebush.objectPos == Vector3Int.zero && tileMap.GetTile(cellPos) != null)
                {
                    hidebush.objectPos = cellPos;
                }
                if (hidebush.objectPos == Vector3Int.zero && tileMap.GetTile(cellPos) == null)
                {
                    if (cellPos.y == -1)
                    {
                        Vector3Int ptr_cellPos = cellPos;
                        Vector3Int ptr_cellPos1 = cellPos;
                        ptr_cellPos.x -= 1;
                        ptr_cellPos1.x += 1;

                        if (tileMap.GetTile(ptr_cellPos) != null)
                        {
                            tileMap.SetTileFlags(ptr_cellPos, TileFlags.None);
                            tileMap.SetColor(ptr_cellPos, Color.green);
                            hidebush.objectPos = ptr_cellPos;
                        }
                        else if (tileMap.GetTile(ptr_cellPos1) != null)
                        {
                            tileMap.SetTileFlags(ptr_cellPos1, TileFlags.None);
                            tileMap.SetColor(ptr_cellPos1, Color.green);
                            hidebush.objectPos = ptr_cellPos1;
                        }
                    }
                }

                if (Mathf.Abs(hidebush.objectPos.x - cellPos.x) == 0 && tileMap.GetTile(cellPos) != null)
                {
                    tileMap.SetTileFlags(cellPos, TileFlags.None);
                    tileMap.SetColor(cellPos, Color.green);
                    hidebush.objectPos = cellPos;
                }
                else if (Mathf.Abs(hidebush.objectPos.x - cellPos.x) > 1 && tileMap.GetTile(cellPos) != null)
                {
                    tileMap.SetTileFlags(hidebush.objectPos, TileFlags.None);
                    tileMap.SetColor(hidebush.objectPos, new Color(255f, 255f, 255f, 1f));

                    tileMap.SetTileFlags(cellPos, TileFlags.None);
                    tileMap.SetColor(cellPos, Color.green);
                    hidebush.objectPos = cellPos;
                }
            }

        }
    }

    void ExitBush(Collider2D collision)
    {
        if (collision.transform.tag == "Monster")
        {
            tileMap.SetTileFlags(monsterPos, TileFlags.None);
            tileMap.SetColor(monsterPos, new Color(255f, 255f, 255f, 1f));
            monsterPos = Vector3Int.zero;
        }
        if (collision.transform.tag == "Player")
        {
            tileMap.SetTileFlags(playerPos, TileFlags.None);
            tileMap.SetColor(playerPos, new Color(255f, 255f, 255f, 1f));
            playerPos = Vector3Int.zero;
            enterBushTime = 0f;
        }
        foreach (BushInObject hidebush in hideBushObjects)
        {
            if (hidebush.obj == collision.gameObject)
            {
                tileMap.SetTileFlags(hidebush.objectPos, TileFlags.None);
                tileMap.SetColor(hidebush.objectPos, new Color(255f, 255f, 255f, 1f));
                hideBushObjects.Remove(hidebush);
                break;
            }
        }
    }

}
