using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bush : MonoBehaviour
{
    public GameObject mushroom;

    private Tilemap tileMap;
    private Vector3Int monsterPos = Vector3Int.zero;
    private Vector3Int playerPos = Vector3Int.zero;
    private float enterBushTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tileMap = gameObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
    }
       

    private void OnTriggerStay2D(Collider2D collision)
    {
        EnterBush(collision);
        if(collision.gameObject.layer==8)
        {
            if (enterBushTime > 4f)
            {
                GameObject inst = Instantiate(mushroom) as GameObject;
                inst.transform.position = collision.transform.position;
                enterBushTime = 0f;
            }
            else
                enterBushTime += Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitBush(collision);
    }

    void EnterBush(Collider2D collision)
    {
        if (collision.transform.tag == "Monster")
        {
            Vector3Int cellPos = tileMap.WorldToCell(collision.transform.position);
            if (monsterPos == Vector3Int.zero && tileMap.GetTile(cellPos) != null)
            {
                monsterPos = cellPos;
            }
            if (monsterPos == Vector3Int.zero && tileMap.GetTile(cellPos) == null)
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
                        monsterPos = ptr_cellPos;
                    }
                    else if (tileMap.GetTile(ptr_cellPos1) != null)
                    {
                        tileMap.SetTileFlags(ptr_cellPos1, TileFlags.None);
                        tileMap.SetColor(ptr_cellPos1, Color.green);
                        monsterPos = ptr_cellPos1;
                    }
                }
            }

            if (Mathf.Abs(monsterPos.x - cellPos.x) == 0 && tileMap.GetTile(cellPos) != null)
            {
                tileMap.SetTileFlags(cellPos, TileFlags.None);
                tileMap.SetColor(cellPos, Color.green);
                monsterPos = cellPos;
            }
            else if (Mathf.Abs(monsterPos.x - cellPos.x) > 1 && tileMap.GetTile(cellPos) != null)
            {
                tileMap.SetTileFlags(monsterPos, TileFlags.None);
                tileMap.SetColor(monsterPos, new Color(255f, 255f, 255f, 1f));

                tileMap.SetTileFlags(cellPos, TileFlags.None);
                tileMap.SetColor(cellPos, Color.green);
                monsterPos = cellPos;
            }
        }
        if (collision.transform.tag == "Player")
        {
            Vector3Int cellPos = tileMap.WorldToCell(collision.transform.position);
            if (playerPos == Vector3Int.zero && tileMap.GetTile(cellPos) != null)
            {
                playerPos = cellPos;
            }
            if (playerPos == Vector3Int.zero && tileMap.GetTile(cellPos) == null)
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
                        playerPos = ptr_cellPos;
                    }
                    else if (tileMap.GetTile(ptr_cellPos1) != null)
                    {
                        tileMap.SetTileFlags(ptr_cellPos1, TileFlags.None);
                        tileMap.SetColor(ptr_cellPos1, Color.green);
                        playerPos = ptr_cellPos1;
                    }
                }
            }

            if (Mathf.Abs(playerPos.x - cellPos.x) == 0 && tileMap.GetTile(cellPos) != null)
            {
                tileMap.SetTileFlags(cellPos, TileFlags.None);
                tileMap.SetColor(cellPos, Color.green);
                playerPos = cellPos;
            }
            else if (Mathf.Abs(playerPos.x - cellPos.x) > 1 && tileMap.GetTile(cellPos) != null)
            {
                tileMap.SetTileFlags(playerPos, TileFlags.None);
                tileMap.SetColor(playerPos, new Color(255f, 255f, 255f, 1f));

                tileMap.SetTileFlags(cellPos, TileFlags.None);
                tileMap.SetColor(cellPos, Color.green);
                playerPos = cellPos;
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
    }

}
