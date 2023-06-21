using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMasterBaseTest : MonoBehaviour
{
    public class CellInfo
    {
        public int index;
        public Vector2Int pos;
        public List<string> buffs = new List<string>();
        public GameObject ins;
    }
    protected Dictionary<GameObject, CellInfo> board = new Dictionary<GameObject, CellInfo>();
    protected enum Status { WAIT_INPUT, WAIT_TARGET, OPERATE, };
    protected Status status = Status.WAIT_INPUT;
    public TMPro.TextMeshPro text; 
    protected CellInfo target;



    public virtual void Start()
    {
        this.init();
        this.initBoard();

    }
    public virtual void init() {
        //这里读json
        text.text = "click tool";
        status = Status.WAIT_INPUT;
    }
    public void useTool(int index=0)
    {
        text.text = "choose cell"; 
        if (status != Status.WAIT_INPUT) return; 
        status = Status.WAIT_TARGET;
        
    }
    public virtual void clickCell()
    {
        text.text = "wait"; 
        status = Status.OPERATE;
        //operate
        
    } 
    public void startTurn()
    {
        text.text = "click tool"; 
        status = Status.WAIT_INPUT;
    }

    public virtual void initBoard() {
           for(var i = 0; i < width; i++)
        {
            for(var j = 0; j < height; j++)
            {
                var ins = Instantiate(cell, holder.transform);
                ins.transform.localPosition = new Vector2(i, j);
                var c = new CellInfo();
                c.index = Random.Range(0, 9);//这个怎么办。。你初始化完了的时候再加个delegate递进来改。
                ins.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = c.index.ToString();
                ins.name = i + "/" + j.ToString();
                c.ins = ins;
                board[ins] = c;
            }
        }
    }
    public GameObject cell,holder;
    public int width=3, height=3; 


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ( status ==  Status.WAIT_TARGET)
            {
                this.tryClickCell();
            }

        }
    }
    void tryClickCell() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            print("Find collider");
            target = board[hit.collider.gameObject];
            clickCell();// hit.collider.gameObject);
        }
    }
    #region methods
    public void renderCell(CellInfo cell)
    {
        cell.ins.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = cell.index.ToString();
    }
    protected delegate void DoAfter();
    protected void DoSomethingAfter(float f, DoAfter _doAfter)
    {
        StartCoroutine(after(f, _doAfter));
    }
    IEnumerator after(float f, DoAfter _doAfter)
    {
        yield return new WaitForSeconds(f);
        _doAfter();
    }

    #endregion
}
