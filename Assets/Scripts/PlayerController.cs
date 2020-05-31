using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentAnimalNo = 0;     // 現在のAnimalPanelのNo。コンボ判定に使用する
    private int comboCount;              // 現在のコンボ数。異なるAnimalNoのAnimalPanelを通過するとアップ。同じNoを通過するとリセット
    private int totalScore;              // 現在のトータルスコア。Wave共通
    private Vector3 startPos;            // Playerのスタート地点登録用。Waveが切り替わる度にこの地点に戻る
    private float moveAmount;            // Playerのパネルの移動量

    public BoxCollider2D boxCol;         // PlayerのBoxCollier2Dをアサインする
    public UIController uIController;    // ヒエラルキー上にあるUIControlerをアサインする
    public LayerMask wallLayer;          // Wallレイヤーをアサインする

    Dictionary<ArrowDirectionType, Vector3> moveDirection;   // Dictionaryの宣言

    void Start() {
        // スタート地点を登録
        startPos = transform.position;

        // Playerの移動量をPlayerオブジェクトの大きさから取得
        moveAmount = GetComponent<RectTransform>().sizeDelta.x;

        // 移動判定用のDictionayを用意
        SetUpMoveDirection();
    }

    /// <summary>
    /// 向きのEnumと、それに対応する方向情報をセットしてDictionayに順次登録
    /// </summary>
    private void SetUpMoveDirection() {
        // 初期化
        moveDirection = new Dictionary<ArrowDirectionType, Vector3>();

        // 向きのEnumと方向情報を１セット単位で登録
        moveDirection.Add(ArrowDirectionType.Right_Middle, new Vector3(moveAmount, 0, 0));
        moveDirection.Add(ArrowDirectionType.Right_Top, new Vector3(moveAmount, moveAmount, 0));
        moveDirection.Add(ArrowDirectionType.Center_Top, new Vector3(0, moveAmount, 0));
        moveDirection.Add(ArrowDirectionType.Left_Top, new Vector3(-moveAmount, moveAmount, 0));
        moveDirection.Add(ArrowDirectionType.Left_Middle, new Vector3(-moveAmount, 0, 0));
        moveDirection.Add(ArrowDirectionType.Left_Bottom, new Vector3(-moveAmount, -moveAmount, 0));
        moveDirection.Add(ArrowDirectionType.Center_Bottom, new Vector3(0, -moveAmount, 0));
        moveDirection.Add(ArrowDirectionType.Right_Bottom, new Vector3(moveAmount, -moveAmount, 0));
    }

    /// <summary>
    /// Wave切り替え時にPlayerをスタート地点に戻す
    /// </summary>
    public void ResetPosition() {
        transform.position = startPos;
    }

    /// <summary>
    /// 矢印パネルの方向に移動可能かどうかを判定
    /// </summary>
    /// <param name="arrowDirectionType"></param>
    public void JudgeMove(ArrowDirectionType arrowDirectionType) {

        // 戻り値を利用して、作成したDictionaryの中から、向きのEnumから方向情報を取得
        Vector3 judgeDirection = SearchDirectionFromDictionary(arrowDirectionType);

        // Playerの位置と方向を取得
        Vector3 playerPos = transform.position;

        // Rayを使って移動先に壁があるか判定
        RaycastHit2D raycastHit2D = Physics2D.Raycast(playerPos, judgeDirection, 1, wallLayer);

        // RayをSceneプレビュー内に可視化
        Debug.DrawRay(playerPos, judgeDirection, Color.red, 1);

        // Wallであった場合、Debugとして内容を表示する
        Debug.Log(raycastHit2D.collider);

        // 移動先が壁の場合、移動失敗の判定をする（raycast2D変数内にゲームオブジェクトが格納されるので、移動処理をしない）
        if (raycastHit2D.collider != null) {
            // 移動失敗のSE鳴らす
            return;
        }

        // 移動判定に成功したのでPlayerを移動
        ActionMove(judgeDirection);
    }

    /// <summary>
    /// DicitonayであるmoveDirectionを検索して、向きのEnumからVector3の方向情報を取得して戻す
    /// </summary>
    /// <param name="arrowDirectionType"></param>
    /// <returns></returns>
    private Vector3 SearchDirectionFromDictionary(ArrowDirectionType arrowDirectionType) {
        // 変数を用意
        Vector3 direction = new Vector3(0, 0, 0);

        // Dicitonayを検索し、引数のEnumとKeyのEnumが一致したら、その方向情報（Value）を取得
        foreach (KeyValuePair<ArrowDirectionType, Vector3> item in moveDirection) {
            if (item.Key == arrowDirectionType) {
                // 値を代入して戻す（ここで処理は終了）
                return direction = item.Value;
            }
        }
        return direction;
    }

    /// <summary>
    /// Playerを移動する
    /// </summary>
    /// <param name="currentMoveDirection"></param>
    private void ActionMove(Vector3 currentMoveDirection) {
        // Playerを移動
        transform.localPosition += currentMoveDirection;
        // Playerの当たり判定をオンにする(移動後にオンにすることで、その間は当たり判定を取らないようにする)
        boxCol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // 当たり判定に入った移動先のパネルがAnimalPanelItemを持っているか確認して変数に代入
        if (col.gameObject.TryGetComponent(out AnimalPanelItem animalPanelItem)) {

            // AnimalPanelItemクラスを持っていないパネルなら処理を止める
            if (animalPanelItem.isGetPanel) {            
                return;
            }

            // コンボ判定を行う。今までいたパネルのAnimalNoと新しく移動した先のパネルのAnimalNoを比べて違う場合にはコンボ成功
            if (currentAnimalNo != animalPanelItem.animalNo) {                
                comboCount++;
            } else {
                // 同じAnimalNoならコンボ失敗。次のコンボに向けて初期化する
                currentAnimalNo = animalPanelItem.animalNo;
                comboCount = 1;
            }

            // スコアを更新(TODO　アニメ処理する)
            totalScore += animalPanelItem.score * comboCount;

            // スコアの画面表示を更新
            uIController.UpdateDisplayScore(totalScore);

            // 通過したパネルは重複して通過できないように処理をする
            animalPanelItem.GetPanel();
        }

        // Playerの当たり判定をオフ
        boxCol.enabled = false;
    }
}
