using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public Piece selectedPiece; // The currently selected piece
    private bool isWhiteTurn = true; // White starts the game

    void Awake()
    {
        // Ensure Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate GameManagers
        }
    }

    // Check if it's the turn of the given piece's color
    public bool IsTurnValid(Piece piece)
    {
        return isWhiteTurn == piece.isWhite;
    }

    // Select a piece if it's the correct turn
    public void SelectPiece(Piece piece)
    {
        // Check if the piece matches the current turn
        if (!IsTurnValid(piece))
        {
            Debug.Log("Not your turn.");
            return;
        }

        // Deselect the currently selected piece, if any
        if (selectedPiece != null && selectedPiece != piece)
        {
            selectedPiece.ResetPosition();
        }

        selectedPiece = piece; // Set the new selected piece
    }

    // Move the currently selected piece to a target position
    public void MoveSelectedPiece(Vector2 targetPos)
    {
        if (selectedPiece == null) return; // No piece selected

        // Validate and execute the move
        if (selectedPiece.IsValidMove(targetPos))
        {
            selectedPiece.Move(targetPos); // Move the piece
            isWhiteTurn = !isWhiteTurn; // Switch turns
            Debug.Log($"Turn changed to {(isWhiteTurn ? "White" : "Black")}");
            selectedPiece = null; // Deselect the piece after moving
        }
        else
        {
            Debug.Log("Invalid move.");
            selectedPiece.ResetPosition(); // Reset the piece if move is invalid
        }
    }
}
