using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCreater : MonoBehaviour
{
    public int maxArrow;
    public int waitTime;

    public int maxColumn;
    public int maxRow;

    public int maxWaveCount;
    private int currentWaveCount = 0;

    //private float timer;
    public int oneWaveTimeLimit;
    private float waveTimer;

    public AnimalPanelItem animalPanelItemPrefab;
    public Transform animalPanelGenerateTran;

    public PanelItem panelItemPrefab;
    public Transform panelGenerateTran;

    public List<PanelItem> panelItemList = new List<PanelItem>();
    public List<AnimalPanelItem> animalPanelList = new List<AnimalPanelItem>();

    public PlayerController playerController;
    public UIController uIController;

    public enum GameState {
        Wait,
        Play,
        Game_Up,
    }
    public GameState gameState;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        gameState = GameState.Wait;

        StartCoroutine(NextWave());
    }

    void Update()
    {
        if (gameState == GameState.Game_Up) {
            return;
        }

        if (gameState == GameState.Wait) {
            return;
        }

        waveTimer -= Time.deltaTime;
        uIController.txtWaveTimer.text = waveTimer.ToString("F0");

        if (waveTimer <= 0) {
            if (currentWaveCount >= maxWaveCount) {
                gameState = GameState.Game_Up;
                uIController.DisplayaGameUp();
                return;
            }
            gameState = GameState.Wait;
            StartCoroutine(NextWave());
        }
    }

    public void NextPanels() {
        DestroyPanels();
        GenerateArrowPanels();
    }

    /// <summary>
    /// PanelItemの破棄
    /// </summary>
    private void DestroyPanels() {
        if (panelItemList.Count <= 0) {
            return;
        }

        for (int i = 0; i < panelItemList.Count; i++) {
            Destroy(panelItemList[i].gameObject);
        }
        panelItemList.Clear();
    }

    /// <summary>
    /// PanelItemの生成
    /// </summary>
    private void GenerateArrowPanels() {
        for (int i = 0; i < maxArrow; i++) {
            PanelItem panelItem = Instantiate(panelItemPrefab, panelGenerateTran, false);
            panelItem.SetupPanel(playerController, this);
            panelItemList.Add(panelItem);
        }
    }

    private IEnumerator NextWave() {
        if (currentWaveCount != 0) {
            DestroyAnimalPanels();

            DestroyPanels();

            playerController.ResetPosition();
        }
        uIController.ActivateReloadButton(false);

        yield return StartCoroutine(GenerateAnimalPanaels());

        waveTimer = oneWaveTimeLimit;
        currentWaveCount++;

        StartCoroutine(uIController.DisplayWaveStart(currentWaveCount));

        yield return new WaitForSeconds(1.5f);

        GenerateArrowPanels();

        uIController.ActivateReloadButton(true);

        gameState = GameState.Play;
    }

    /// <summary>
    /// AnimalPanelItemの破棄
    /// </summary>
    private void DestroyAnimalPanels() {
        for (int i = 0; i < animalPanelList.Count; i++) {
            Destroy(animalPanelList[i].gameObject);
        }
        animalPanelList.Clear();
    }

    /// <summary>
    /// AnimalPanelItemの生成
    /// </summary>
    private IEnumerator GenerateAnimalPanaels() {
        for (int x = 0; x < maxColumn; x++) {
            for (int y = 0; y < maxRow; y++) {
                AnimalPanelItem animalPanelItem = Instantiate(animalPanelItemPrefab, animalPanelGenerateTran, false);
                animalPanelItem.SetUpAnimalPanel(Random.Range(0,7), 100);
                animalPanelList.Add(animalPanelItem);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
