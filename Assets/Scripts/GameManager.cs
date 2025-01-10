using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Piece selectedPiece;
    private bool isWhiteTurn = true; // Tracks the current player's turn
    public static Vector2 whiteKingPosition;
    public static Vector2 blackKingPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsCurrentPlayerWhite()
    {
        return isWhiteTurn;
    }

    public bool IsTurnValid(Piece piece)
    {
        return piece.isWhite == isWhiteTurn; // Check if the piece belongs to the current player
    }

    public void SelectPiece(Piece piece)
    {
        if (selectedPiece == piece)
        {
            selectedPiece = null; // Deselect if the same piece is selected again
            return;
        }

        selectedPiece = piece;
    }

    public void MoveSelectedPiece(Vector2 targetPos)
    {
        if (selectedPiece == null)
            return;

        if (selectedPiece.IsValidMove(targetPos))
        {
            selectedPiece.Move(targetPos);

            if (selectedPiece.name.Contains("King"))
            {
                if (selectedPiece.isWhite)
                    whiteKingPosition = targetPos;
                else
                    blackKingPosition = targetPos;
            }

            if (IsKingInCheck(!isWhiteTurn) && IsCheckmate(!isWhiteTurn))
            {
                Debug.Log($"{(isWhiteTurn ? "White" : "Black")} wins by checkmate!");
            }

            selectedPiece = null; // Deselect piece after the move
            Invoke(nameof(SwitchTurn), 0.5f); // Delay for animations
        }
        else
        {
            selectedPiece.ResetPosition(); // Reset to original position if the move is invalid
        }
    }

    private void SwitchTurn()
    {
        isWhiteTurn = !isWhiteTurn; // Switch turn to the other player
    }

    public bool IsKingInCheck(bool checkWhiteKing)
    {
        Vector2 kingPosition = checkWhiteKing ? whiteKingPosition : blackKingPosition;
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isWhite != checkWhiteKing && piece.IsValidMove(kingPosition, true))
                return true;
        }
        return false;
    }

    public bool IsCheckmate(bool checkWhiteKing)
    {
        Vector2 kingPosition = checkWhiteKing ? whiteKingPosition : blackKingPosition;

        // Check if the king can move to a safe square
        Vector2[] possibleMoves = {
            new(0, 1), new(1, 0), new(0, -1), new(-1, 0),
            new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)
        };

        foreach (var move in possibleMoves)
        {
            if (IsPositionSafe(kingPosition + move, checkWhiteKing))
                return false;
        }

        // Check if any piece can block or capture the threatening piece
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isWhite == checkWhiteKing)
            {
                Vector2 originalPos = piece.transform.position;

                foreach (var move in piece.GetValidMoves())
                {
                    piece.Move(move);
                    if (!IsKingInCheck(checkWhiteKing))
                    {
                        piece.Move(originalPos); // Reset piece
                        return false;
                    }
                    piece.Move(originalPos);
                }
            }
        }
        return true;
    }

    private bool IsPositionSafe(Vector2 position, bool checkWhiteKing)
    {
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isWhite != checkWhiteKing && piece.IsValidMove(position))
                return false;
        }
        return true;
    }
}
