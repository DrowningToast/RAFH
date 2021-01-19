using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using level;
using Player;
using System;

namespace level {

    public class TilesetsController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static TilesetsController instance;

        [SerializeField] public GridController[] grids = new GridController[5];
        [SerializeField] public bool[] states = new bool[5];
        [SerializeField] public int tilesetPointer;
        [SerializeField] GameObject debugTile;
        [SerializeField] public bool debugMode = false;

        // event ที่จะ raise เมื่อต้องการที่จะหา tileset ตัวใหม่
        public event Action<int> onLoadNewGrid;
        // event ที่จะ raise เมื่อต้องการให้ tileset เก่านั้น disable ไป
        public event Action<int> onDisableOldGrid;

        [SerializeField] public float speed = 3;

        void Awake ()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        void Start()
        {


            // Need cleaning later



            if (debugMode)
            {
                // เมื่อตรวจจับพบว่าอยู่ใน debug tile ให้ไม่ต้องรัน setup tiles ที่เหลือ
                debugTile.GetComponent<GridController>().firstGrid();
                tilesetPointer = 0;
                PlayerController.instance.onLoadNewTileset += chooseNewTile;
                PlayerController.instance.onDeleteOldTileset += findOldTile;
                return;
            }
            else
            {
                // หากไม่ได้รัน debug tile ให้ทำการปิดมันทิ้งซะ
                debugTile.SetActive(false);
            }
            // References Child Grids
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i] = transform.GetChild(i).GetComponent<GridController>();
            }
            // print(grids);

            // Choose and activate first tileset

            tilesetPointer = 0;
            print("First tilemap is : " + tilesetPointer);
            grids[tilesetPointer].firstGrid();

            PlayerController.instance.onLoadNewTileset += chooseNewTile;
            PlayerController.instance.onDeleteOldTileset += findOldTile;

        }

        // จะถูกเรียกเมื่อได้รับ event มาว่าให้ load tilemap ใหม่
        void chooseNewTile (int bannedTile = -1)
        {
            // ก่อนที่จะสุ่ม tileset อันถัดไป จะทำการเช็คก่อนว่าอยู่ใน debug mode รึเปล่า
            if (debugMode)
            {
                onLoadNewGrid.Invoke(-1);
                return;
            }


            tilesetPointer = UnityEngine.Random.Range(0, grids.Length);
            if (bannedTile == -1)
            {
                onLoadNewGrid.Invoke(tilesetPointer);
            }
            else
            {
                if (tilesetPointer == bannedTile)
                {
                    print($"{tilesetPointer} : is the same tile as old one, oops! rerolling!");
                    chooseNewTile(bannedTile);
                }
                else
                {
                    onLoadNewGrid.Invoke(tilesetPointer);
                    print("selected : " + tilesetPointer);
                }
            }
        }

        // จะถูกเรียกเมื่อได้รับ event มาว่าให้ลบอันเก่าออก
        void findOldTile(int target)
        {
            onDisableOldGrid.Invoke(target);
        }

        // Update the state of moving
        private void Update()
        {

        }

    }
}