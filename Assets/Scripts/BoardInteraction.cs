using UnityEngine;

public class BoardInteraction : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                Debug.Log($"Clicked on: {hit.name}"); // Log the name of the clicked object

                if (hit.GetComponent<Piece>() != null) // If it's a piece
                {
                    GameManager.Instance.SelectPiece(hit.GetComponent<Piece>());
                }
                else if (GameManager.Instance.selectedPiece != null) // If a piece is selected
                {
                    Vector2 targetPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));
                    Debug.Log($"Tile clicked at: {targetPos}");
                    GameManager.Instance.MoveSelectedPiece(targetPos);
                }
            }
            else
            {
                Debug.Log("Nothing was clicked.");
            }
        }
    }
}
