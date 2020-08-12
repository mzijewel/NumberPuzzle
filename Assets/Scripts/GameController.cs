using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    public static GameController i;

    [SerializeField]
    private List<Level> levels;

    private List<Chunk> chunks;
    [SerializeField]
    private GameObject prefChunk;
    [SerializeField]
    private Transform chunkContainer;
    [SerializeField] private TextMeshProUGUI txtMoveCount, txtWin, txtBest, txtTotal;

    private int moveCount;
    private List<int> numbers;
    private int bestMove, totalWin;

    private const string BEST_MOVE = "bestMove";
    private const string TOTAL_WIN = "totalWin";

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        bestMove = PlayerPrefs.GetInt(BEST_MOVE, 0);
        totalWin = PlayerPrefs.GetInt(TOTAL_WIN, 0);
        txtMoveCount.text = moveCount.ToString();
        txtWin.text = "";
        txtBest.text = "Best-" + bestMove;
        txtTotal.text = "Won-" + totalWin;

        Generate();
    }
    public void OnChunkClick(Chunk chunk)
    {
        Chunk emptyChunk = chunks.Find((c) => c.id == 16);
        int dif = Mathf.Abs(chunk.pos - emptyChunk.pos);
        int emptyPos = emptyChunk.pos;
        if (dif == 1 || dif == 4)
        {
            emptyChunk.Replace(chunk.id);
            chunk.Replace(16);
            CheckWin();
            moveCount++;
            txtMoveCount.text = moveCount.ToString();
        }

    }

    private void CheckWin()
    {
        bool isWin = true;
        foreach (Chunk chunk in chunks)
        {
            if (chunk.id != chunk.pos)
            {
                isWin = false;
                break;
            }

        }
        if (isWin)
        {
            totalWin++;
            txtWin.text = "You won!!";
            txtTotal.text = "Won-" + totalWin;
            PlayerPrefs.SetInt(TOTAL_WIN, totalWin);
            if (moveCount < bestMove || bestMove == 0)
            {
                bestMove = moveCount;
                PlayerPrefs.SetInt("bestMove", bestMove);
                txtBest.text = "Best-" + bestMove;
            }
        }
    }
    private void Generate()
    {
        chunks = new List<Chunk>();
        numbers = new List<int>();
        for (int i = 1; i <= 16; i++)
        {

            Chunk chunk = Instantiate(prefChunk, chunkContainer).GetComponent<Chunk>();
            chunks.Add(chunk);
            if (i <= 15)
                numbers.Add(i);

        }
        numbers = Shuffle(numbers);
        for (int i = 0; i < chunks.Count; i++)
        {
            int id = i == 15 ? 16 : numbers[i];
            int pos = i + 1;
            chunks[i].Setup(id, pos);
        }
    }



    private List<T> Shuffle<T>(List<T> list)
    {
        List<T> randomList = new List<T>();


        int rndIndex = 0;

        while (list.Count > 0)
        {
            rndIndex = Random.Range(0, list.Count);
            randomList.Add(list[rndIndex]);
            list.RemoveAt(rndIndex);
        }

        return randomList;
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(0);
    }
}
