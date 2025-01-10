using System.Collections.Generic;
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
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        if (!GameManager.Instance.IsTurnValid(this))
        {
            Debug.Log($"Cannot select {name}: either not your turn or move already completed.");
            return;
        }

        GameManager.Instance.SelectPiece(this);
        Debug.Log($"{name} selected.");
    }



    public bool IsValidMove(Vector2 targetPos, bool suppressLogs = false)
    {
        Vector2 currentPos = (Vector2)transform.position;

        Piece targetPiece = GetPieceAtPosition(targetPos);

        // Cannot move to a square occupied by a piece of the same color
        if (targetPiece != null && targetPiece.isWhite == this.isWhite)
        {
            if (!suppressLogs)
                Debug.Log("Cannot move to a square occupied by your own piece.");
            return false;
        }

        // PAWN MOVEMENT
        if (name.Contains("Pawn"))
        {
            int direction = isWhite ? 1 : -1;
            if (targetPos == currentPos + Vector2.up * direction && targetPiece == null)
            {
                return true;
            }
            if ((currentPos.y == 1 && isWhite || currentPos.y == 6 && !isWhite) &&
                targetPos == currentPos + Vector2.up * direction * 2 && targetPiece == null)
            {
                return true;
            }
            if (Mathf.Abs(targetPos.x - currentPos.x) == 1 && targetPos.y - currentPos.y == direction)
            {
                if (targetPiece != null && targetPiece.isWhite != this.isWhite) return true;
            }
        }

        // ROOK MOVEMENT
        if (name.Contains("Rook"))
        {
            if (targetPos.x == currentPos.x || targetPos.y == currentPos.y)
                return IsPathClear(currentPos, targetPos);
        }

        // KNIGHT MOVEMENT
        if (name.Contains("Knight"))
        {
            Vector2[] moves = { new(1, 2), new(2, 1), new(-1, 2), new(-2, 1), new(1, -2), new(2, -1), new(-1, -2), new(-2, -1) };
            foreach (var move in moves)
            {
                if (targetPos == currentPos + move)
                {
                    return targetPiece == null || targetPiece.isWhite != this.isWhite;
                }
            }
        }

        // BISHOP MOVEMENT
        if (name.Contains("Bishop"))
        {
            if (Mathf.Abs(targetPos.x - currentPos.x) == Mathf.Abs(targetPos.y - currentPos.y))
                return IsPathClear(currentPos, targetPos);
        }

        // QUEEN MOVEMENT
        if (name.Contains("Queen"))
        {
            if (targetPos.x == currentPos.x || targetPos.y == currentPos.y ||
                Mathf.Abs(targetPos.x - currentPos.x) == Mathf.Abs(targetPos.y - currentPos.y))
                return IsPathClear(currentPos, targetPos);
        }

        // KING MOVEMENT
        if (name.Contains("King"))
        {
            if (Mathf.Abs(targetPos.x - currentPos.x) <= 1 && Mathf.Abs(targetPos.y - currentPos.y) <= 1)
            {
                return true;
            }
        }

        return false; // Invalid move
    }

    private bool IsPathClear(Vector2 currentPos, Vector2 targetPos)
    {
        Vector2 direction = new Vector2(
            Mathf.Clamp(targetPos.x - currentPos.x, -1, 1),
            Mathf.Clamp(targetPos.y - currentPos.y, -1, 1)
        );

        Vector2 nextPos = currentPos + direction;
        while (nextPos != targetPos)
        {
            if (GetPieceAtPosition(nextPos) != null)
            {
                return false;
            }
            nextPos += direction;
        }
        return true;
    }

    private Piece GetPieceAtPosition(Vector2 position)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Piece[] allPieces = FindObjectsOfType<Piece>();
#pragma warning restore CS0618 // Type or member is obsolete
        foreach (var piece in allPieces)
        {
            if (piece.transform.position == (Vector3)position)
            {
                return piece;
            }
        }
        return null;
    }

    public Vector2[] GetValidMoves()
    {
        // Generate all valid moves for this piece based on its movement logic
        List<Vector2> validMoves = new List<Vector2>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector2 targetPos = new Vector2(x, y);
                if (IsValidMove(targetPos))
                {
                    validMoves.Add(targetPos);
                }
            }
        }
        return validMoves.ToArray();
    }


    public void Move(Vector2 targetPos)
    {
        Piece targetPiece = GetPieceAtPosition(targetPos);

        // Capture the opponent's piece
        if (targetPiece != null && targetPiece.isWhite != this.isWhite)
        {
            Destroy(targetPiece.gameObject);
            Debug.Log($"{name} captured {targetPiece.name}");
        }

        transform.position = targetPos;
        startPos = targetPos;
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }
}
