using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager UI { get; private set; }

    [SerializeField] private GameObject[] EndGameUI;

    [SerializeField] private TextMeshProUGUI winnersText;

    private void Start()
    {
        if (UI != null)
            Destroy(this.gameObject);
        else
            UI = this;

        SetEndGameUIActive(false);
    }

    public void SetEndGameUIActive(bool active)
    {
        foreach (GameObject ui in EndGameUI)
            ui.SetActive(active);
    }

    public void UpdateWinnersText(GameManager.ColorTurn winner)
    {
        winnersText.text = string.Format("{0} WINS", winner.ToString().ToUpper());
    }
}
