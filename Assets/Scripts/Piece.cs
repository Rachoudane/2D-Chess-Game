using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite; // White or Black piece
    private Vector2 startPos; // Track the original position

    private void Start()
    {
        startPos = transform.position; // Store initial position
    }

    private void OnMouseDown()
    {
        // Ensure GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        // Ensure the turn is valid
        if (!GameManager.Instance.IsTurnValid(this))
        {
            Debug.Log("Not your turn.");
            return;
        }

        // Select the piece
        GameManager.Instance.SelectPiece(this);
    }

    public bool IsValidMove(Vector2 targetPos)
    {
        Vector2 currentPos = (Vector2)transform.position;

        // PAWN MOVEMENT
        if (name.Contains("Pawn"))
        {
            int direction = isWhite ? 1 : -1; // Move up for white, down for black
            if (targetPos == currentPos + Vector2.up * direction) // Move forward
            {
                return true;
            }
            if ((currentPos.y == 1 && isWhite || currentPos.y == 6 && !isWhite) && // First move: 2 steps
                targetPos == currentPos + Vector2.up * direction * 2)
            {
                return true;
            }
        }

        // ROOK MOVEMENT
        if (name.Contains("Rook"))
        {
            return targetPos.x == currentPos.x || targetPos.y == currentPos.y; // Straight lines
        }

        // KNIGHT MOVEMENT
        if (name.Contains("Knight"))
        {
            Vector2[] moves = { new(1, 2), new(2, 1), new(-1, 2), new(-2, 1), new(1, -2), new(2, -1), new(-1, -2), new(-2, -1) };
            foreach (var move in moves)
            {
                if (targetPos == currentPos + move)
                    return true;
            }
        }

        // BISHOP MOVEMENT
        if (name.Contains("Bishop"))
        {
            return Mathf.Abs(targetPos.x - currentPos.x) == Mathf.Abs(targetPos.y - currentPos.y); // Diagonals
        }

        // QUEEN MOVEMENT
        if (name.Contains("Queen"))
        {
            return (targetPos.x == currentPos.x || targetPos.y == currentPos.y) || // Rook-like
                   Mathf.Abs(targetPos.x - currentPos.x) == Mathf.Abs(targetPos.y - currentPos.y); // Bishop-like
        }

        // KING MOVEMENT
        if (name.Contains("King"))
        {
            return Mathf.Abs(targetPos.x - currentPos.x) <= 1 && Mathf.Abs(targetPos.y - currentPos.y) <= 1; // One step any direction
        }

        return false; // Invalid move
    }

    public void Move(Vector2 targetPos)
    {
        transform.position = targetPos; // Update position
        startPos = targetPos;
    }

    public void ResetPosition()
    {
        transform.position = startPos; // Reset to starting position
    }
}
