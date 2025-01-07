using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton
    private Piece selectedPiece;
    private bool isWhiteTurn = true;

    void Awake()
    {
        Instance = this;
    }

    public bool IsCurrentPlayerWhite()
    {
        return isWhiteTurn;
    }

    public void SelectPiece(Piece piece)
    {
        if (selectedPiece != null)
        {
            selectedPiece.ResetPosition();
        }
        selectedPiece = piece;
    }

    public void MoveSelectedPiece(Vector2 newPosition)
    {
        if (selectedPiece != null)
        {
            selectedPiece.Move(newPosition);
            isWhiteTurn = !isWhiteTurn; // Switch turn
            selectedPiece = null;
        }
    }
}
