using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    // Start is called before the first frame update
    void Awake()
    {
        CloseAllPanel();
        loginPanel.SetActive(true);

        

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
