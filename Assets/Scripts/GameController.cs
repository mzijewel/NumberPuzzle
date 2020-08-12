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
        if (chunk.pos % 4 == 1)
        {
            if (emptyPos == chunk.pos - 1)
                return;
        }
        if (chunk.pos % 4 == 0)
        {
            if (emptyPos == chunk.pos + 1)
                return;
        }
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
            //if (i <= 15)
            numbers.Add(i);

        }
        numbers = Shuffle(numbers);



        for (int i = 0; i < chunks.Count; i++)
        {
            int id = numbers[i];
            int pos = i + 1;
            chunks[i].Setup(id, pos);
        }
    }



    private List<int> Shuffle(List<int> numbers)
    {
        int zeroIndex = 15;

        for (int i = 0; i < 200; i++)
        {


            int next = 0;
            if (zeroIndex >= 12 && zeroIndex <= 15)
            {
                if (zeroIndex == 12)
                {
                    int rnd = Random.Range(0, 2);
                    next = zeroIndex + (rnd == 0 ? -4 : 1);
                    //-4,1
                }
                else if (zeroIndex == 13 || zeroIndex == 14)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                        next = zeroIndex - 1;
                    else if (rnd == 1)
                        next = zeroIndex + 1;
                    else
                        next = zeroIndex - 4;
                    //-1,+1,-4
                }
                else if (zeroIndex == 15)
                {
                    int rnd = Random.Range(0, 2);
                    next = zeroIndex + (rnd == 0 ? -1 : -4);
                    //-1,-4
                }
            }
            else if (zeroIndex >= 8 && zeroIndex <= 11 || zeroIndex >= 4 && zeroIndex <= 7)
            {
                if (zeroIndex == 8 || zeroIndex == 4)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                        next = zeroIndex + 1;
                    else if (rnd == 1)
                        next = zeroIndex + 4;
                    else
                        next = zeroIndex - 4;
                    //+1,+4,-4
                }
                else if (zeroIndex == 9 || zeroIndex == 10 || zeroIndex == 5 || zeroIndex == 6)
                {
                    int rnd = Random.Range(0, 4);
                    if (rnd == 0)
                        next = zeroIndex - 1;
                    else if (rnd == 1)
                        next = zeroIndex + 1;
                    else if (rnd == 2)
                        next = zeroIndex - 4;
                    else
                        next = zeroIndex + 4;
                    //-1,+1,-4,+4
                }
                else if (zeroIndex == 11 || zeroIndex == 7)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                        next = zeroIndex - 1;
                    else if (rnd == 1)
                        next = zeroIndex - 4;
                    else
                        next = zeroIndex + 4;
                    //-1,-4,+4
                }
            }
            else if (zeroIndex >= 0 && zeroIndex <= 3)
            {
                if (zeroIndex == 0)
                {
                    //+1,4
                    int rnd = Random.Range(0, 2);
                    next = zeroIndex + (rnd == 0 ? 1 : 4);
                }
                else if (zeroIndex == 1 || zeroIndex == 2)
                {
                    //+1,-1,+4
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                        next = zeroIndex + 1;
                    else if (rnd == 1)
                        next = zeroIndex - 1;
                    else
                        next = zeroIndex + 4;
                }
                else if (zeroIndex == 3)
                {
                    //-1,+4
                    int rnd = Random.Range(0, 2);
                    next = zeroIndex + (rnd == 0 ? -1 : 4);
                }
            }

            int a = numbers[zeroIndex];
            int b = numbers[next];

            numbers[zeroIndex] = b;
            numbers[next] = a;

            zeroIndex = next;
        }

        return numbers;
    }




    public void OnRestart()
    {
        SceneManager.LoadScene(0);
    }
}
