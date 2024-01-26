using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    public Vector2 position { get; private set; } = Vector2.zero;
    public bool isAvailable { get; private set; } = false;

    public void SetPosition(Vector2 newPosition) { position = newPosition; }
    public void SetAvailability(bool availability) { isAvailable = availability; }

    private void OnMouseEnter()
    {
        if (isAvailable)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (isAvailable)
        {
            GridSystem.Grid.SelectedTile(this);
        }
    }
}
