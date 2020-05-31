using UnityEngine;
using UnityEngine.UI;

public class AnimalPanelItem : MonoBehaviour
{
    public Image imgAnimal;
    public int animalNo;
    public int score;
    public bool isGetPanel;
    public BoxCollider2D boxCollider2D;

    public void SetUpAnimalPanel(int no, int score)
    {
        animalNo = no;
        this.score = score;
        imgAnimal.sprite = Resources.Load<Sprite>("animal_" + animalNo);
    }

    public void GetPanel() {
        imgAnimal.color = new Color(1, 1, 1, 0.25f);
        isGetPanel = true;
        boxCollider2D.enabled = false;
    }
}
