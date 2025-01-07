using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject whitePawn, whiteKnight, whiteBishop, whiteRook, whiteQueen, whiteKing;
    public GameObject blackPawn, blackKnight, blackBishop, blackRook, blackQueen, blackKing;

    public GameObject tilePrefab; // Assign a square tile prefab in Unity
    public int boardSize = 8; // Chessboard size


    void Start()
    {
        GenerateBoard();
        PlacePieces();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tile.transform.parent = transform;

                Color Black = ColorUtility.TryParseHtmlString("#1b0e3b", out Color BlackColor) ? BlackColor : Color.white;
                Color White = ColorUtility.TryParseHtmlString("#D5B85A", out Color WhiteColor) ? WhiteColor : Color.white;
                // Alternate colors
                bool isWhite = (x + y - 1) % 2 == 0;
                tile.GetComponent<SpriteRenderer>().color = isWhite ? White : Black;
            }
        }
    }

    void PlacePieces()
    {
        // Place pawns
        for (int i = 0; i < boardSize; i++)
        {
            Instantiate(whitePawn, new Vector2(i, 1), Quaternion.identity); // Row 1 for white pawns
            Instantiate(blackPawn, new Vector2(i, 6), Quaternion.identity); // Row 6 for black pawns
        }

        // Place other pieces
        PlaceMajorPieces(0, whiteRook, whiteKnight, whiteBishop, whiteQueen, whiteKing);
        PlaceMajorPieces(7, blackRook, blackKnight, blackBishop, blackQueen, blackKing);
    }

    void PlaceMajorPieces(int row, GameObject rook, GameObject knight, GameObject bishop, GameObject queen, GameObject king)
    {
        Instantiate(rook, new Vector2(0, row), Quaternion.identity);
        Instantiate(rook, new Vector2(7, row), Quaternion.identity);

        Instantiate(knight, new Vector2(1, row), Quaternion.identity);
        Instantiate(knight, new Vector2(6, row), Quaternion.identity);

        Instantiate(bishop, new Vector2(2, row), Quaternion.identity);
        Instantiate(bishop, new Vector2(5, row), Quaternion.identity);

        Instantiate(queen, new Vector2(3, row), Quaternion.identity);
        Instantiate(king, new Vector2(4, row), Quaternion.identity);
    }
}   
