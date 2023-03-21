using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.IO;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainProgram;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject showValuePanel;

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private List<TextMeshProUGUI> gridTexts;

    [SerializeField]
    private List<TextMeshPro> stationTexts;

    [SerializeField]
    private GameObject camHolder;

    [SerializeField]
    private double timer;

    [SerializeField]
    private bool timerIsActive = true;

    private bool isShowPanelActive=false;

    public class data : IComparable<data>
    {
        public int id;
        public string ad;
        public int deger;

        public int CompareTo(data other)
        {
            if (this.id > other.id)
            {
                return 1;
            }else if (this.id < other.id)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    private List<data> dataList = new List<data>();

    // Start is called before the first frame update
    void Awake()
    {
        CloseAllPanel();
        loginPanel.SetActive(true);

        

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetRequest("http://127.0.0.1:8080/sql/sql4.php"));
        FillGrid();
        FillTables();
    }


    public void OpenShowValue()
    {
        var posShowPanelPos=showValuePanel.transform.position;
        var mainMenuPanelPos=mainMenuPanel.transform.position;
        if(!isShowPanelActive)
        {
            showValuePanel.SetActive(true);
            showValuePanel.LeanMoveLocalX(701f,1f);
            mainMenuPanel.LeanMoveLocalX(375f,1f);
            isShowPanelActive=true;
        }
        else
        {
            mainMenuPanel.LeanMoveLocalX(mainMenuPanelPos.x-446,1f);
            showValuePanel.LeanMoveX(posShowPanelPos.x+523,1f).setOnComplete(()=>
            {
                showValuePanel.SetActive(false);

            });
            isShowPanelActive=false;
            
        }
        
    }

    private void WriteGridTexts(float timer)
    {
        
        timer-=Time.deltaTime;
        if(timer<=0)
        {
            StartCoroutine(GetRequest("http://localhost/sql/sql4.php"));
            FillGrid();
            ColorChange();
            //bu methodu update de timera 4 sn vererek çalıştır. WriteGridTexts(4f)
            //aynı zamanda yeni yaptığım uı ekranını butonla çalıştırıyorsun. Sen Editörden Setactivini aç bi incele.
            //Yeni eklenen kısımda ise elemanları göreceksin, SQL den çekeceğin değerleri burada gridTexts[0,1,3].text= değer; şeklinde yazdırabilirsin.
            //sonrasında yeni aldığımız stationtext[i]=gridtexts[i] şeklinde eşitleyeceksin.
        }
    }

    private void CloseAllPanel()
    {
        showValuePanel.SetActive(false);
        mainProgram.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void Login()
    {
        //buraya veritabanı kontrolü eklenecek;
        loginPanel.SetActive(false);
        mainProgram.SetActive(true);
        mainMenuPanel.SetActive(true);
        camHolder.transform.position=new Vector3(1.5199f,30.520f,-16.65f);

    }
    void LateUpdate()
    {
        if(timerIsActive)
        {
            timer-=Time.deltaTime;
            if(timer<0)
            {
                TagManager.Instance.UpdateMinersRecieverListCount();

                timerIsActive = false;


            }
            TagManager.Instance.UpdateMinersRecieverListCount();


        }

    }

    public void Register()
    {
        //buraya veritabanına kayıt koyulacak;
    }
    private void FillGrid()
    {
        int count = 0;
        int nowid = dataList[0].id;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (nowid > dataList[i].id)
            {
                nowid = dataList[i].id;
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            while (nowid-1 < dataList[i].id && dataList[i].id < (nowid + 3))
            {
                if (dataList[i].ad.ToLower() == "ch4")
                {
                    gridTexts[(count *8)+ 1].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "co")
                {
                    gridTexts[(count * 8)+ 2].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "o2")
                {
                    gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "h2s")
                {
                    gridTexts[(count * 8) + 4].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "hh")
                {
                    gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "ısı")
                {
                    gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "basınc")
                {
                    gridTexts[(count * 8) + 7].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "nem")
                {
                    gridTexts[(count * 8) + 8].GetComponent<TextMeshProUGUI>().text = dataList[i].deger.ToString();
                }
                i++;
            }
            nowid = dataList[i].id;
            i--;
            count++;
        }
    }
    private void FillTables()
    {
        int count = 0;
        int nowid = dataList[0].id;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (nowid > dataList[i].id)
            {
                nowid = dataList[i].id;
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            while (nowid - 1 < dataList[i].id && dataList[i].id < (nowid + 3))
            {
                if (dataList[i].ad.ToLower() == "ch4")
                {
                    stationTexts[(count * 8) + 1].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "co")
                {
                    stationTexts[(count * 8) + 2].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "o2")
                {
                    stationTexts[(count * 8) + 3].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "h2s")
                {
                    stationTexts[(count * 8) + 4].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "hh")
                {
                    stationTexts[(count * 8) + 5].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "ısı")
                {
                    stationTexts[(count * 8) + 6].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "basınc")
                {
                    stationTexts[(count * 8) + 7].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                else if (dataList[i].ad.ToLower() == "nem")
                {
                    stationTexts[(count * 8) + 8].GetComponent<TextMeshPro>().text = dataList[i].deger.ToString();
                }
                i++;
            }
            nowid = dataList[i].id;
            i--;
            count++;
        }
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);

                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string raw = webRequest.downloadHandler.text;
                    string[] qwe = raw.Split(",");
                    dataList.Clear();
                    for (int i = 0; i < qwe.Length - 1; i++)
                    {
                        string[] ert = qwe[i].Split("/");
                        data wer = new data();
                        wer.id = Convert.ToInt16(ert[0]);
                        wer.ad = ert[1];
                        wer.deger = Convert.ToInt16(ert[2]);
                        dataList.Add(wer);
                        dataList.Sort();
                    }
                    break;
            }
        }
    }
    private void ColorChange()
    {
        Debug.Log(Convert.ToInt16(stationTexts[0].text));
        Debug.Log(Convert.ToInt16(stationTexts[1].text));
        Debug.Log(Convert.ToInt16(stationTexts[2].text));
        Debug.Log(Convert.ToInt16(stationTexts[3].text));
        Debug.Log(Convert.ToInt16(stationTexts[4].text));
        Debug.Log(Convert.ToInt16(stationTexts[5].text));
        if (Convert.ToInt16(stationTexts[0].text) >= 2)//ch4
        {
            stationTexts[0].GetComponent<TextMeshPro>().color = Color.red;
        }
        else if (Convert.ToInt16(stationTexts[0].text) >= 1.5)
        {
            stationTexts[0].GetComponent<TextMeshPro>().color = Color.yellow;
        }
        else
        {
            stationTexts[0].GetComponent<TextMeshPro>().color = Color.white;
        }


        if (Convert.ToInt16(stationTexts[1].text) >= 30)//co
        {
            stationTexts[1].GetComponent<TextMeshPro>().color = Color.red;
        }
        else if (Convert.ToInt16(stationTexts[1].text) >= 20)
        {
            stationTexts[1].GetComponent<TextMeshPro>().color = Color.yellow;
        }
        else
        {
            stationTexts[1].GetComponent<TextMeshPro>().color = Color.white;
        }


        if (Convert.ToInt16(stationTexts[3].text) >= 20)//h2s
        {
            stationTexts[3].GetComponent<TextMeshPro>().color = Color.red;
        }
        else if (Convert.ToInt16(stationTexts[3].text) >= 10)
        {
            stationTexts[3].GetComponent<TextMeshPro>().color = Color.yellow;
        }
        else
        {
            stationTexts[3].GetComponent<TextMeshPro>().color = Color.white;
        }


        if (Convert.ToInt16(stationTexts[2].text) <= 19)//o2
        {
            stationTexts[2].GetComponent<TextMeshPro>().color = Color.red;
        }
        else if (Convert.ToInt16(stationTexts[2].text) >= 23)
        {
            stationTexts[2].GetComponent<TextMeshPro>().color = Color.red;
        }
        else
        {
            stationTexts[2].GetComponent<TextMeshPro>().color = Color.white;
        }


        if (Convert.ToInt16(stationTexts[4].text) >= 40)//ısı
        {
            stationTexts[4].GetComponent<TextMeshPro>().color = Color.red;
        }
        else if (Convert.ToInt16(stationTexts[4].text) >= 30)
        {
            stationTexts[4].GetComponent<TextMeshPro>().color = Color.yellow;
        }
        else
        {
            stationTexts[4].GetComponent<TextMeshPro>().color = Color.white;
        }


        if (Convert.ToInt16(stationTexts[5].text) >= 10)//hh
        {
            stationTexts[5].GetComponent<TextMeshPro>().color = Color.red;
        }
        else if (Convert.ToInt16(stationTexts[5].text) >= 5)
        {
            stationTexts[5].GetComponent<TextMeshPro>().color = Color.yellow;
        }
        else
        {
            stationTexts[5].GetComponent<TextMeshPro>().color = Color.white;
        }
    }
}
