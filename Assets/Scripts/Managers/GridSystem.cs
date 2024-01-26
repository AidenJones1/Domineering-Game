using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Grid { get; private set; }

    [Header("Dimensions")]
    [SerializeField] [Range(2, 10)] private int _width = 5;
    [SerializeField] [Range(2, 10)] private int _height = 5;

    [Header("Tile Prefab")]
    [SerializeField] private GameObject _tilePrefab;

    [Header("Tile Colors")]
    [SerializeField] private Color _baseColor = Color.black;
    [SerializeField] private Color _altColor = Color.white;

    private Tile[,] _tileArray;

    private UnityEvent onTileClaim;

    private void Start()
    {
        if (Grid != null)
            Destroy(this.gameObject);
        else
            Grid = this;

        GenerateGrid();

        onTileClaim = new UnityEvent();
        onTileClaim.AddListener(new UnityAction(GameManager.Game.NextTurn));
    }

    public void GenerateGrid()
    {
        ClearBoard();

        _tileArray = new Tile[_width, _height];

        float xOffset = FindOffset(_width);
        float yOffset = FindOffset(_height);

        for (int y = 0; y < _tileArray.GetLength(1); y++)
        {
            for (int x = 0; x < _tileArray.GetLength(0); x++)
            {
                GameObject tileObject = Instantiate(_tilePrefab, new Vector2(x - xOffset, y - yOffset), Quaternion.identity, this.transform);
                tileObject.name = string.Format("Tile [{0}][{1}]", x, y);

                if ((x + y) % 2 == 0)
                    tileObject.GetComponent<SpriteRenderer>().color = _baseColor;
                else
                    tileObject.GetComponent <SpriteRenderer>().color = _altColor;

                Tile tile = tileObject.GetComponent<Tile>();
                tile.SetPosition(new Vector2(x, y));
                tile.SetAvailability(true);

                _tileArray[x, y] = tile;
            }
        }
    }

    private void ClearBoard()
    {
        for (int i = transform.childCount - 1; i >= 0 ; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private float FindOffset(int dimensionLength)
    {
        if (dimensionLength % 2 == 0)
            return dimensionLength / 2 - 0.5f;
        else
            return Mathf.Floor(dimensionLength / 2);
    }

    public void SelectedTile(Tile tile)
    {
        Vector2 tilePosition = tile.position;

        Tile baseTile = tile;
        Tile adjTile;
        Color color;

        if (GameManager.Game.CurrentTurn == GameManager.ColorTurn.Blue)
        {
            if (tilePosition.y + 1 < _tileArray.GetLength(1))
            {
                adjTile = _tileArray[(int)tilePosition.x, (int)tilePosition.y + 1];
                color = Color.blue;
            }
            else return;
        }

        else
        {
            if (tilePosition.x + 1 < _tileArray.GetLength(0))
            {
                adjTile = _tileArray[(int)tilePosition.x + 1, (int)tilePosition.y];
                color = Color.red;
            }
            else return;
        }

        if (adjTile.isAvailable)
        {
            ClaimTile(baseTile, color);
            ClaimTile(adjTile, color);

            onTileClaim.Invoke();            
        }

    }

    private void ClaimTile(Tile tile, Color color)
    {
        tile.SetAvailability(false);
        tile.gameObject.GetComponent<SpriteRenderer>().color = color;
        tile.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CheckForPlayableTiles(GameManager.ColorTurn turn)
    {
        if (turn == GameManager.ColorTurn.Blue)
        {
            for (int y = 0; y < _tileArray.GetLength(1) - 1; y++)
            {
                for (int x = 0; x < _tileArray.GetLength(0); x++)
                {
                    //Checks if this tile and the tile above it is available
                    if (_tileArray[x, y].isAvailable && _tileArray[x, y + 1].isAvailable)
                        return;
                }
            }

            //Ends game with 'Red' as the winner
            GameManager.Game.EndGame(GameManager.ColorTurn.Red);
        }

        else
        {
            for (int x = 0; x < _tileArray.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < _tileArray.GetLength(1); y++)
                {
                    //Checks if this tile and the tile to the right is available
                    if (_tileArray[x, y].isAvailable && _tileArray[x + 1, y].isAvailable)
                        return;
                }
            }

            //Ends game with 'Blue' as the winner
            GameManager.Game.EndGame(GameManager.ColorTurn.Blue);
        }
    }
}
