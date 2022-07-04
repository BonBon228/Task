using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSort : MonoBehaviour
{
    public TMP_InputField Width;
    public TMP_InputField Height;

    private int N;
    private int M;

    public GameObject letter;
    public RectTransform rectTransform;
    public Transform parent;

    private string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private float speed = 0;

    private List <GameObject> letterList = new List <GameObject>();
    private List <Vector2> posList = new List <Vector2>(); 
    private List <int> intList = new List <int>();
    private List <int> randomizer = new List <int>();

    private bool isMixing;
    
    void FixedUpdate() 
    {
        if(isMixing == true)
        {
            speed += 0.06f * Time.deltaTime;
            for (int i = 0; i < letterList.Count; i++)
            {
                letterList[i].transform.position = Vector2.Lerp(letterList[i].transform.position, posList[randomizer[i]], speed);
                if(letterList[i].transform.position == (Vector3)posList[i])
                {
                    letterList[randomizer[i]].transform.position = Vector2.Lerp(letterList[randomizer[i]].transform.position, posList[i], speed);
                }
            }
        }
    }

    public void OnBtnClick_Generate()
    {
        if(isMixing == false)
        {
            rectTransform.sizeDelta = new Vector2(1000f, 1000f);

            N = int.Parse(Width.text);
            M = int.Parse(Height.text);

            if(posList != null)
            {
                posList.Clear();
            }

            if(intList != null)
            {
                intList.Clear();
            }

            if(letterList != null)
            {
                for (int i = 0; i < letterList.Count; i++)
                {
                    Destroy(letterList[i].gameObject);
                }
                letterList.Clear();
            }


            RectTransform letterRT = letter.GetComponent<RectTransform>();

            if(N > M)
            {
                letterRT.sizeDelta = new Vector2(rectTransform.sizeDelta.x/N, rectTransform.sizeDelta.y/N);
                rectTransform.sizeDelta = new Vector2(letterRT.sizeDelta.x*N, letterRT.sizeDelta.y*M);
            }
            else if(M > N)
            {
                letterRT.sizeDelta = new Vector2(rectTransform.sizeDelta.x/M, rectTransform.sizeDelta.y/M);
                rectTransform.sizeDelta = new Vector2(letterRT.sizeDelta.x*N, letterRT.sizeDelta.y*M);
            }
            else if(M == N)
            {
                letterRT.sizeDelta = new Vector2(rectTransform.sizeDelta.x/N, rectTransform.sizeDelta.y/M);
                rectTransform.sizeDelta = new Vector2(letterRT.sizeDelta.x*N, letterRT.sizeDelta.y*M);
            }

            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < M; j++)
                {
                    GameObject letterInst = Instantiate(letter, rectTransform.anchoredPosition, Quaternion.identity, parent);
                    TMP_Text mText = letterInst.GetComponent<TMP_Text>();
                    mText.text = st[Random.Range(0, st.Length)].ToString();
                    if(N > M || N == M)
                    {
                        mText.fontSize = letterRT.sizeDelta.x * 0.9f;
                    }
                    else if(M > N)
                    {
                        mText.fontSize = letterRT.sizeDelta.y * 0.9f;
                    }
                    RectTransform letterInstRT = letterInst.GetComponent<RectTransform>();
                    letterInstRT.anchoredPosition = new Vector2(i*letterInstRT.sizeDelta.x, j*letterInstRT.sizeDelta.y);
                    posList.Add(letterInst.transform.position);
                    letterList.Add(letterInst);
                }
            }

            for(int i = 0; i < N*M; i++)
            {
                intList.Add(i);
            }
        }
    }

    public void OnBtnClick_Mix()
    {
        if(isMixing == false && letterList != null)
        {
            if(randomizer != null)
            {
                randomizer.Clear();
            }

            //for (int i = 0; i < letterList.Count; i++)
            //{
            //    int intToAdd = Random.Range(0, intList.Count);
            //    randomizer.Add(intToAdd);
            //    intList.RemoveAt(intToAdd);
            //}

            for (int i = 0; i < letterList.Count; i++)
            {
                int intToAdd = Random.Range(0, intList.Count);
                while(randomizer.Contains(intToAdd))
                intToAdd = Random.Range(0, intList.Count);
                randomizer.Add(intToAdd);
            }
            
            StartCoroutine(mixEnumer());
        }
    }

    IEnumerator mixEnumer()
    {
        speed = 0f;
        isMixing = true;
        yield return new WaitForSecondsRealtime(2f);
        speed = 1f;
        isMixing = false;
    }
}