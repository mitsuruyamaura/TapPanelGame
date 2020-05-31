using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public Text txtScore;
    public Text txtWaveCount;
    public Text txtWaveTimer;
    public Text txtInfo;

    public Button btnReload;
    public PanelCreater panelCreater;

    public bool isClickable;
    private float waitTime = 0.5f;

    private Sequence sequence;

    void Start() {
        sequence = DOTween.Sequence();
        btnReload.onClick.AddListener(() => StartCoroutine(OnClickReloadArrowPanels(waitTime)));
    }

    public void DisplayaGameUp() {
        txtInfo.text = "Game Up!";
        sequence.Append(txtInfo.transform.DOLocalMoveX(0, 1.0f)).SetEase(Ease.Linear).SetRelative();
    }

    public IEnumerator DisplayWaveStart(int waveCount) {
        txtWaveCount.text = waveCount.ToString();
        txtInfo.text = "Wave : " + waveCount + " Start!";
        sequence.Append(txtInfo.transform.DOLocalMoveX(0, 1.0f)).SetEase(Ease.Linear).SetRelative();
        yield return new WaitForSeconds(2.0f);
        sequence.Append(txtInfo.transform.DOLocalMoveX(700, 1.0f)).SetEase(Ease.Linear).SetRelative();
        yield return new WaitForSeconds(1.0f);
        txtInfo.text = "";
        sequence.Append(txtInfo.transform.DOLocalMoveX(-700, 1.0f)).SetEase(Ease.Linear).SetRelative();
    }

    public void UpdateDisplayScore(int score) {
        // アニメさせる
        txtScore.text = score.ToString();
    }

    private IEnumerator OnClickReloadArrowPanels(float waitTime) {
        if (isClickable) {
            yield break;
        }
        isClickable = true;
        ActivateReloadButton(false);
        panelCreater.NextPanels();
        yield return new WaitForSeconds(waitTime);
        isClickable = false;
        ActivateReloadButton(true);
    }

    public void ActivateReloadButton(bool isSwitch) {
        btnReload.interactable = isSwitch;
    }
}
