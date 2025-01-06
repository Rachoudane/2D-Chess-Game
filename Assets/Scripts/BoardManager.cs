using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab; // Assign a square tile prefab in Unity
    public int boardSize = 8; // Chessboard size

    void Start()
    {
        GenerateBoard();
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
                bool isWhite = (x + y) % 2 == 0;
                tile.GetComponent<SpriteRenderer>().color = isWhite ? White : Black;
            }
        }
    }
}   
