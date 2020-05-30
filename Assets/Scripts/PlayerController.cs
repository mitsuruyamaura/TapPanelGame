using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class PlayerController : MonoBehaviour
{
    public BoxCollider2D boxCol;
    public int comboCount;
    public int totalScore;
    private int currentComboNo = 0;
    private Vector3 startPos;
    private float movePos;

    public UIController uIController;

    void Start() {
        startPos = transform.position;
        movePos = GetComponent<RectTransform>().sizeDelta.x;
    }

    public void ResetPosition() {
        transform.position = startPos;
    }


    public void Move(ArrowDirectionType arrowDirectionType) {
        // Rayで壁判定

        // 壁でないなら８方向移動
        switch (arrowDirectionType) {
            case ArrowDirectionType.Right_Middle:
                transform.localPosition += new Vector3(movePos, 0, 0);
                break;
            case ArrowDirectionType.Right_Top:
                transform.localPosition += new Vector3(movePos, movePos, 0);
                break;
            case ArrowDirectionType.Center_Top:
                transform.localPosition += new Vector3(0, movePos, 0);
                break;
            case ArrowDirectionType.Left_Top:
                transform.localPosition += new Vector3(-movePos, movePos, 0);
                break;
            case ArrowDirectionType.Left_Middle:
                transform.localPosition += new Vector3(-movePos, 0, 0);
                break;
            case ArrowDirectionType.Left_Bottom:
                transform.localPosition += new Vector3(-movePos, -movePos, 0);
                break;
            case ArrowDirectionType.Center_Bottom:
                transform.localPosition += new Vector3(0, -movePos, 0);
                break;
            case ArrowDirectionType.Right_Bottom:
                transform.localPosition += new Vector3(movePos, -movePos, 0);
                break;
        }
        boxCol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.TryGetComponent(out AnimalPanelItem animalPanelItem)) {
            if (animalPanelItem.isGetPanel) {
                return;
            }

            if (currentComboNo == animalPanelItem.animalNo) {
                comboCount++;
            } else {
                comboCount = 1;
            }
            totalScore += animalPanelItem.score * comboCount;
            uIController.UpdateDisplayScore(totalScore);
            animalPanelItem.GetPanel();
        }
        boxCol.enabled = false;
    }
}
