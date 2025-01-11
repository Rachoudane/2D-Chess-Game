using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Piece selectedPiece;
    private bool isWhiteTurn = true; // Tracks the current player's turn
    public static Vector2 whiteKingPosition;
    public static Vector2 blackKingPosition;

    // Unified TextMeshPro element for all UI messages
    public TextMeshProUGUI gameStatusText; // This will show turn, message, and countdown information

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Find the initial positions of both kings
#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.name.Contains("King"))
            {
                if (piece.isWhite)
                    whiteKingPosition = piece.transform.position;
                else
                    blackKingPosition = piece.transform.position;
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete
        UpdateGameStatus(); // Initialize the game status display
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

    private bool gameOver = false;

    public void MoveSelectedPiece(Vector2 targetPos)
    {
        if (gameOver) return; // Prevent further moves after game over

        if (selectedPiece == null) return;

        if (selectedPiece.IsValidMove(targetPos))
        {
            selectedPiece.Move(targetPos);

            if (IsKingInCheck(!isWhiteTurn))
            {
                DisplayMessage($"{(isWhiteTurn ? "White" : "Black")} puts {(isWhiteTurn ? "Black" : "White")} in check!");
                if (IsCheckmate(!isWhiteTurn))
                {
                    DisplayMessage($"{(isWhiteTurn ? "White" : "Black")} wins by checkmate!");
                    gameOver = true; // Set game over state
                    StartCoroutine(RestartGameCountdown());
                    return;
                }
            }
            selectedPiece = null;
            SwitchTurn();
        }
        else
        {
            selectedPiece.ResetPosition();
            Debug.Log("Move invalid due to check!");
        }
    }



    private void SwitchTurn()
    {
        isWhiteTurn = !isWhiteTurn; // Switch turn to the other player
        UpdateGameStatus(); // Update the display after the turn switch
    }

    private void UpdateGameStatus()
    {
        if (gameStatusText != null)
        {
            // Update the text based on the current game state
            if (gameStatusText.gameObject.activeSelf)
            {
                gameStatusText.text = $"Turn: {(isWhiteTurn ? "Gold" : "Purple")}";
            }
        }
    }

    public bool IsKingInCheck(bool checkWhiteKing)
    {
        Vector2 kingPosition = checkWhiteKing ? whiteKingPosition : blackKingPosition;
        Debug.Log($"Checking threats against {(checkWhiteKing ? "White" : "Black")} King at {kingPosition}");

#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isWhite != checkWhiteKing && piece.IsValidMove(kingPosition, true))
            {
                Debug.Log($"{piece.name} threatens the King at {kingPosition}");
                return true;
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete
        return false;
    }




    public bool IsCheckmate(bool checkWhiteKing)
    {
        if (!IsKingInCheck(checkWhiteKing)) return false;

        // Check if any piece can block the check or move the king to safety
#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isWhite == checkWhiteKing)
            {
                Vector2 originalPos = piece.transform.position;
                foreach (var move in piece.GetValidMoves())
                {
                    piece.Move(move);
                    bool isSafe = !IsKingInCheck(checkWhiteKing);
                    piece.Move(originalPos); // Undo move

                    if (isSafe)
                        return false;
                }
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete

        return true;
    }


    private bool IsPositionSafe(Vector2 position, bool checkWhiteKing)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isWhite != checkWhiteKing && piece.IsValidMove(position, true))
            {
                return false;
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete
        return true;
    }


    private void DisplayMessage(string message)
    {
        if (gameStatusText != null)
        {
            gameStatusText.text = message;
            gameStatusText.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        gameStatusText.gameObject.SetActive(false);
    }

    private IEnumerator RestartGameCountdown()
    {
        for (int i = 5; i > 0; i--)
        {
            if (gameStatusText != null)
            {
                gameStatusText.text = $"Restarting in {i}...";
            }
            yield return new WaitForSeconds(1f);
        }

        // Restart the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
