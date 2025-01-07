using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite; // White or Black piece
    public bool isKing;  // Check for King
    private Vector2 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.IsCurrentPlayerWhite() == isWhite)
        {
            GameManager.Instance.SelectPiece(this);
        }
    }

    public void Move(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }
}
