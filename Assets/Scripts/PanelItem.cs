using UnityEngine;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour
{
    public Button btnPanel;
    public Image imgPanel;
    public ArrowDirectionType ArrowDirectionType;
    
    private bool isClickable;
    private PlayerController playerController;
    private PanelCreater panelCreater;

    public void SetupPanel(PlayerController playerController, PanelCreater panelCreater) {
        isClickable = true;

        this.playerController = playerController;
        this.panelCreater = panelCreater;
        
        FetchArrowDirection();

        btnPanel.onClick.AddListener(OnClickPanel);

        isClickable = false;
    }

    /// <summary>
    /// 矢の方向情報とイメージの方向を一致させる
    /// </summary>
    private void FetchArrowDirection() {
        // 矢の方向をランダムで決定
        int randomDirection = Random.Range(0, (int)ArrowDirectionType.Count);

        // 矢のイメージと方向の情報を一致
        imgPanel.transform.rotation = Quaternion.Euler(0, 0, randomDirection * 45);
        ArrowDirectionType = (ArrowDirectionType)randomDirection;
    }

    private void OnClickPanel() {
        if (isClickable) {
            return;
        }
        isClickable = true;
        btnPanel.interactable = false;
        playerController.JudgeMove(ArrowDirectionType);
        panelCreater.NextPanels();
    }
}
