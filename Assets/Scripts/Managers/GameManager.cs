using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Game { get; private set; }

    public enum ColorTurn { Blue, Red }
    public ColorTurn CurrentTurn { get; private set; } = ColorTurn.Blue;

    [SerializeField] private GameObject endGameUI;
    private void Start()
    {
        if (Game != null)
            Destroy(this.gameObject);
        else
            Game = this;
    }

    public void NextTurn()
    {
        if (CurrentTurn == ColorTurn.Blue)
            CurrentTurn = ColorTurn.Red;
        else
            CurrentTurn = ColorTurn.Blue;

        GridSystem.Grid.CheckForPlayableTiles(CurrentTurn);
    }

    public void EndGame(ColorTurn winner)
    {
        UIManager.UI.SetEndGameUIActive(true);
        UIManager.UI.UpdateWinnersText(winner);
    }

    public void ResetGame()
    {
        UIManager.UI.SetEndGameUIActive(false);
        GridSystem.Grid.GenerateGrid();
        CurrentTurn = ColorTurn.Blue;
    }
}
