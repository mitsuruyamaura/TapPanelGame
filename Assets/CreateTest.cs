using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTest : MonoBehaviour
{
    public GameObject parentPrefab;
    public GameObject blockPrefab;

    private GameObject parent;

    public List<int[]> intList = new List<int[]>();
    public Transform [] blockStartPos;
    public Transform canvasTransform;
    public List<GameObject> parentList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        intList.Add(new int[3] { 0, 0, 1 });
        intList.Add(new int[3] { 0, 1, 1 });
        intList.Add(new int[3] { 2, 2, 2 });

        CreateBlocks();
    }

    private void CreateBlocks() {
        int value = 0;

        for (int i = 0; i < 2; i ++) {
            parentList.Add(Instantiate(parentPrefab, canvasTransform));
        }

        for (int i = 0; i < intList.Count; i++) {
            for (int j = 0; j < intList[i].Length; j++) {
                value = intList[i][j];
                if (value == 0) {
                    // 空のブロックの場合、親子は作らないでインスタンスする
                    Instantiate(blockPrefab, new Vector3(blockStartPos[0].position.x + i * 30, blockStartPos[0].position.y + j * 30, 0), Quaternion.identity);

                } else {
                    // ブロックを生成して、親子にする 
                    GameObject block = Instantiate(blockPrefab, new Vector3(blockStartPos[0].position.x + i * 30, blockStartPos[0].position.y + j * 30, 0), Quaternion.identity);
                    block.transform.SetParent(parentList[value - 1].transform);
                }
            }
        }
    }

    // 
}
