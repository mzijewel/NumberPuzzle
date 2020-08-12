using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Chunk : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtNumber;
    public int id;
    public int pos;
    [SerializeField] private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        //txtNumber.text = id.ToString();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void Setup(int id, int pos)
    {
        this.id = id;
        this.pos = pos;
        
        if (id == 16)
        {
            Color c = img.color;
            c.a = 0;
            img.color = c;
            txtNumber.text = "";
        }
        else
        {
            img.color = Color.grey;
            txtNumber.text = id.ToString();
        }
            
    }
    public void Replace(int id)
    {
        this.id = id;
        txtNumber.text = id.ToString();
        if (id == 16)
        {
            Color c = img.color;
            c.a = 0;
            img.color = c;
            txtNumber.text = "";
        }
        else
            img.color = Color.grey;
    }
    public void OnClick()
    {
        GameController.i.OnChunkClick(this);
    }
}
