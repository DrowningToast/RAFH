using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using level;


namespace level {

    public class TilemapController : MonoBehaviour
    {
        [SerializeField] Tilemap tilemap;
        [SerializeField] TilemapRenderer tilemapRenderer;
        [SerializeField] TilemapCollider2D tilemapCollider2D;
    
        // Start is called before the first frame update
        void Awake()
        {
            print($"{gameObject.name} : Tilemap Awake()");

            // Reference ตัวเองเข้าไปใน ช่อง GameObject ของ parent
            transform.parent.GetComponent<GridController>().tilemap = gameObject;

            // References
            tilemap = GetComponent<Tilemap>();
            tilemapRenderer = GetComponent<TilemapRenderer>();
            tilemapCollider2D = GetComponent<TilemapCollider2D>();

            // Disable all Components
            tilemap.enabled = false;
            tilemapRenderer.enabled = false;
            tilemapCollider2D.enabled = false;

            // Set tag to => "Floor"
            tag = "ground";
            
            // ในกรณีที่เป็นตัวแรก จะไม่ต้องปิดตัวเองหลังจากที่ GridController พยายามเปิด
            /*if (transform.parent.parent.GetChild(0).GetComponent<GridController>() == transform.parent.GetComponent<GridController>())
            {
                gameObject.SetActive(true);
            }
            else
            {
                // ปิดไว้ก่อน ในกรณีที่ไม่ใช่ตัวแรกและเผลอเปิด GameObject ไว้
                gameObject.SetActive(false);
            }*/



        }

        public void enableTileSet (bool isFirstGrid = false)
        {
            if (tilemap == null)
            {
                // References
                tilemap = GetComponent<Tilemap>();
                tilemapRenderer = GetComponent<TilemapRenderer>();
                tilemapCollider2D = GetComponent<TilemapCollider2D>();
            }
            // isFirstGrid ในกรณีที่เป็นตัวแรกจะไม่ทำการตั้ง offset เผื่อ tilemap 
            transform.parent.position = isFirstGrid ? new Vector2(0, 0) : new Vector2(25, 0);
            // Enable all components
            tilemap.enabled = true;
            tilemapRenderer.enabled = true;
            tilemapCollider2D.enabled = true;
        }
        public void disableTileSet()
        {
            // Disable all components
            tilemap.enabled = false;
            tilemapRenderer.enabled = false;
            tilemapCollider2D.enabled = false;
        }

    }

}