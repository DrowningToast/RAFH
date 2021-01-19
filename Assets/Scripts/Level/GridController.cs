using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Player;


namespace level
{
    public class GridController : MonoBehaviour
    {
        // แต่ละ tileset จะยาว "50" หน่วย แต่ว่าจะขึ้นฉากใหม่ตัวที่สิ่งไปได้ "50" หน่วย โดยตัว tilemap จะมีเพียงแค่ด้านขวาของตัว grid เท่านั้น
        // เพราะฉะนั้นการที่จะโหลด tileset ใหม่ก็จะต้อง

        // rigidbody2D ของตัวเองที่เอาใช้ตั้งค่า velocity ของฉาก
        [SerializeField] Rigidbody2D rigidbody2D;

        // Reference ถึง tilemap ใน grid ของตัวเอง
        [SerializeField] public GameObject tilemap;

        // Reference ถึง Collider ที่เมื่อชนแล้วจะเริ่มการโหลด tileset ใหม่ขึ้นมาต่อ
        // ตำแหน่งของ Collider ควรจะอยู่ตอนครึ่งทางของ 50 หน่วย 
        [SerializeField] Collider2D triggerLoad;

        // Reference ถึง Collider ที่เมื่อชนแล้วจะทำการปิด tileset ของตัวเองเนื่องจากวิ่งจบ tileset ตัวเองแล้ว
        // ตำแหน่งของ Collider ควรจะอยู่ตอนที่ 50 แต่บวกไปอีก 5 ของ tileset
        [SerializeField] Collider2D triggerDelete;

        // เมื่อได้เรียก firstGrid แล้วจะทราบได้ว่า grids[] นั้นพร้อมใช้งานจากภายนอกแล้ว
        // เราจึงสามารถให้ Grid เช็คด้วยตัวเองได้ว่าตนนั้นอยุ่ที่ index อะไรของ array
        public static event Action onAfterFirstGrid;

        // Index ของ Grid ใน array ของ grids ของ TilesetController
        public int index;

        // Only true when this grid is the first grid
        bool isFirstGrid = false;

        // Set to true if this is debug plane
        [SerializeField] bool isDebug = false;

        // Only true when its tileset is actived and enable
        [SerializeField] bool moving = false;

        void Awake()
        {
            GridController.onAfterFirstGrid += checkSelfIndex;
        }

        void Start()
        {
            // References
            triggerLoad = transform.GetChild(1).GetComponent<Collider2D>();
            triggerDelete = transform.GetChild(2).GetComponent<Collider2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();

            // Disable Load & Delete collider to prevent dupes
            triggerLoad.gameObject.SetActive(isFirstGrid);
            triggerDelete.gameObject.SetActive(isFirstGrid);

            // ผูก method เข้ากับตัว event
            TilesetsController.instance.onLoadNewGrid += whenLoad;
            TilesetsController.instance.onDisableOldGrid += whenDelete;

            // ผูก method ที่เอาไว้เช็ค index ของตัวเองเข้ากับ event ที่จะถูก raise เมื่อ firstGrid เสร็จ

            // ทำการปิด tilemap ไปเลยเผื่อในกรณีที่เผลอเปิดไว้ โดยจะต้องเช็คด้วยว่า ฉันไม่ใช่ firstGrid
            if (!moving)
            {
                tilemap.SetActive(false);
            }
            
            print($"Starto! {gameObject.name}");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (moving)
            {
                rigidbody2D.velocity = new Vector2(TilesetsController.instance.speed * -1,0);
            }
            else
            {
                rigidbody2D.velocity = new Vector2(0, 0);
            }
        }

        public void firstGrid ()
        {
            // moving => true
            moving = true;

            // เนื่องจากว่า reference tilemap ไม่ทัน จึงต้อง reference สดตรงนี้
            tilemap = transform.GetChild(0).gameObject;
            // Set offset
            transform.position = new Vector2(-5, 0);
            tilemap.SetActive(true);
            // ทำการ enable component ทุกอย่างภายใน tileset โดยให้ paremeter ของ isFirstGrid เป็น true เพื่อป้องกันการให้ offset-x ของ grid เป้น 25
            tilemap.GetComponent<TilemapController>().enableTileSet(true);

            // References
            triggerLoad = transform.GetChild(1).GetComponent<Collider2D>();
            triggerDelete = transform.GetChild(2).GetComponent<Collider2D>();

            // เมื่อทราบแล้วว่าถูกตัวก็ให้
            // Load & Delete Collider turns on
            isFirstGrid = true;
            triggerLoad.gameObject.SetActive(isFirstGrid);
            triggerDelete.gameObject.SetActive(isFirstGrid);

            // จะทราบได้ว่า grids นั้นพร้อมแล้วสำหรับการใช้งาน จึงให้ทุก grid เช็คตัวเองว่าอยู่ที่ index ใด
            // ผูก method ที่เอาไว้เช็ค index ของตัวเองเข้ากับ event ที่จะถูก raise เมื่อ firstGrid เสร็จ
            GridController.onAfterFirstGrid += checkSelfIndex;
            GridController.onAfterFirstGrid.Invoke();
        }

        void whenLoad(int index)
        {
            // Debug Case
            if (isDebug && transform.parent.GetComponent<TilesetsController>().debugMode)
            {
                // ขยับ Tile ไปที่จุดเดิม
                tilemap.GetComponent<TilemapController>().enableTileSet(true);
                // เปิด triggerLoad ตัวเดิมอีกรอบหลังจากโดน PlayerConroller.cs ปิดไปเพราะกลัวโดนซ้ำ
                triggerLoad.gameObject.SetActive(true);
                triggerDelete.gameObject.SetActive(true);
                return;
            }

            // เช็คว่า index ของตัวเองตรงกับ index ที่ได้เรียกจาก event รึเปล่า
            // print($"{gameObject.name} : Target Index = {index} : My index = {this.index}");
            if (index != this.index)
            {
                return;
            }
            // เมื่อทราบแล้วว่าถูกตัวก็ให้
            // Load & Delete Collider turns on
            triggerLoad.gameObject.SetActive(true);
            triggerDelete.gameObject.SetActive(true);
            // เปิดตัว tilemap ของ grid ตัวเอง
            tilemap.SetActive(true);
            tilemap.GetComponent<TilemapController>().enableTileSet();
            // เริ่มขยับ
            moving = true;
        }

        void whenDelete (int index)
        { 
            // เช็คว่า index ของตัวเองตรงกับ index ที่ได้เรียกจาก event รึเปล่า
            if (index != this.index)
            {
                return;
            }

            // เมื่อทราบแล้วว่าถูกตัวก็ให้
            // หยุดขยับก่อน
            moving = false;
            // Disable Load & Delete Collider
            triggerLoad.gameObject.SetActive(false);
            triggerDelete.gameObject.SetActive(false);

            tilemap.GetComponent<TilemapController>().disableTileSet();
            // Reset Position back to 0
            transform.position = new Vector2(0, 0);
            tilemap.SetActive(false);
        }

        // มีหน้าที่ check ว่าตัวเองนั้นอยู่ index ที่เท่าไหร่ของ grids[]
        // โดย method ตัวนี้ควรจะผูกอยู่กับ event onAfterFirstGrid ที่ควรถูกเรียกหลังจาก firstGrid
        void checkSelfIndex()
        {
            // เช็คว่าตัวเองอยู่ index ที่เท่าไหร่ใน Grids ของ TilesetController
            for (int i = 0; i < TilesetsController.instance.grids.Length; i++)
            {
                // Debug : Check self-finding grid
                // print($"Check self-index on {gameObject.name} : I'm : {this} :  And I'm checking for : {TilesetsController.instance.grids[i]}");
                if (this == TilesetsController.instance.grids[i])
                {
                    index = i;
                    print("Found index : " + this.index);
                    break;
                }
            }
        }
    }
}



// Goals next weeks
//  - Slide obstacles
//  - Debug Randomizer