using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecieverManager : MonoBehaviour
{
   [SerializeField]
    private GameObject recieverPref;

    [SerializeField]
    private GameObject recieverNamePanel;

    [SerializeField]
    private TMP_InputField recieverNameInput;

    [SerializeField]
    private GameObject recieversParent;

    [SerializeField]
    private ReadData readData;

    private Receiver tempReciever=new Receiver();
    private string currentRecieverName;
    private int addedReciever=0;

    private void Awake() 
    {
    
        tempReciever.recieverName="A";
        tempReciever.signalCount=0;
        recieverNamePanel.SetActive(false);
        
        for (int i = 1; i <= PlayerPrefs.GetInt("currentSaveReciever"); i++)
        {
            var x=PlayerPrefs.GetFloat("posx"+i);
            var y=PlayerPrefs.GetFloat("posy"+i);
            var z=PlayerPrefs.GetFloat("posz"+i);
            var desPos= new Vector3(x,y,z);

           var newReciever= Instantiate(recieverPref,desPos,Quaternion.identity,recieversParent.transform);
           newReciever.GetComponent<AddedReciever>().IsBreak=true;
           newReciever.name=PlayerPrefs.GetString("RecieverName"+i);

           readData.signalList.Add(newReciever);
          
        }
    }

    //anten ekleme

    public void ProduceNewReciever()
    {
        currentRecieverName=recieverNameInput.text;
        recieverNamePanel.SetActive(false);
        var newReciever=Instantiate(recieverPref);
        PlayerPrefs.SetInt("currentSaveReciever",PlayerPrefs.GetInt("currentSaveReciever")+1);
        newReciever.name=currentRecieverName;

        PlayerPrefs.SetString("RecieverName"+PlayerPrefs.GetInt("currentSaveReciever"),currentRecieverName);
        readData.signalList.Add(newReciever);
    }

    public void AddNameReciever()
    {
        recieverNamePanel.SetActive(true);
    
        

    }


    public void _JsonSave(int maxTagCount)
    {

        MinerList miner=new MinerList();
        for (int i = 0; i < maxTagCount; i++)
        {
            RecieverList recieverList=new RecieverList();
        
            for (int j = 0; j < readData.signalList.Count; j++)
            {
                recieverList.recieverList.Add(tempReciever);
                
            }

            miner.minerRecieverList.Add(recieverList);
            
        }

        string convertJson=JsonUtility.ToJson(miner);
        System.IO.File.WriteAllText(Application.persistentDataPath+"/RecieverList.json",convertJson);
    }

    
    public void UpdateJsonSave(List<Receiver> jsonSaveFile2,int newSignalCount, int listNumber,int minerNumber)
    {
          var jsonSaveFile=readData.ReadJson();
         
          jsonSaveFile.minerRecieverList[minerNumber].recieverList[listNumber].signalCount=newSignalCount;
          jsonSaveFile.minerRecieverList[minerNumber].recieverList[listNumber].recieverName=readData.signalList[listNumber].name;
        //jsonSaveFile[listNumber].signalCount=newSignalCount;

        string convertJson=JsonUtility.ToJson(jsonSaveFile);
         System.IO.File.WriteAllText(Application.persistentDataPath+"/RecieverList.json",convertJson);

    }

    

}
