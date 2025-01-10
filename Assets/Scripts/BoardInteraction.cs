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
                Piece piece = hit.GetComponent<Piece>();
                if (GameManager.Instance.selectedPiece != null) // If a piece is selected, handle move
                {
                    Vector2 targetPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));
                    GameManager.Instance.MoveSelectedPiece(targetPos);
                }
                else if (piece != null) // If the clicked object is a piece
                {
                    if (GameManager.Instance.IsTurnValid(piece))
                    {
                        GameManager.Instance.SelectPiece(piece);
                    }
                }
            }
        }
    }
}
