using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using Unity.VisualScripting;
using System.Xml.Linq;

[System.Serializable]
public class MinerList
{
    public List<RecieverList> minerRecieverList= new List<RecieverList>();
}

[System.Serializable]
public class RecieverList
{
    public List<Receiver> recieverList=new List<Receiver>();
}

[System.Serializable]
public class Receiver
{
    public string recieverName;
    public int signalCount;
}
public class ReadData : MonoBehaviour
{
    
   
    public List<GameObject> signalList=new List<GameObject>();
    //public List<Receiver> tamplist = new List<Receiver>();

    public PathCreator path;

    private float firstPosZ, firstPosX, firstPosY, secondPosZ, secondPosX, secondPosY,bigSignal = 0, secondBigSignal = 0, firstSignal1 = 0, secondSignal1 = 0, firstSignal = 0, secondSignal = 0,w;
    private  int countBig,countSmall;
    private string bigSignalName,secondBigSignalName;
    private GameObject firstPos,secondPos,secondBigPos,bigPos;

    private void Start()
    {
      
    }

    public MinerList ReadJson()
    {
       string JsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/RecieverList.json");
       MinerList minerList = JsonUtility. FromJson<MinerList>(JsonData);
       return minerList;
    }

    public void MoveBySignal(GameObject miner, int minerNumber)
    {
       TagManager.Instance.UpdateMinersRecieverListCount();    

       GetTwoBigRecieverSignal(minerNumber);

       //for (int i = 0; i < signalList.Count; i++)
       //{
       //    if(signalList[i].name==bigSignalName)
       //    {
       //        bigPos=signalList[i];
       //        countBig=i;
       //    }
       //    else if(signalList[i].name==secondBigSignalName)
       //    {
       //        secondBigPos=signalList[i];
       //        countSmall=i;
       //    }
       //}
        
       //if(countBig>countSmall)
       //{
       //     firstPos = bigPos;
       //     secondPos = secondBigPos;

       //}
       //else
       //{
       //     firstPos = secondBigPos;
       //     secondPos = bigPos;
       // }
        var A1 = signalList.Find(i => i.name == bigSignalName);
        var A2 = signalList.Find(i => i.name == secondBigSignalName);
        float x1 = A1.transform.position.x;
        float x2 = A2.transform.position.x;
        float y1 = A1.transform.position.y;
        float y2 = A2.transform.position.y;
        float z1 = A1.transform.position.z;
        float z2 = A2.transform.position.z;
        if (x1 < x2)
        {
            firstPosX = x1;
            firstSignal = bigSignal;
            secondPosX = x2;
            secondSignal = secondBigSignal;
        }
        else if (x1 > x2)
        {
            firstPosX = x2;
            firstSignal = secondBigSignal;
            secondPosX = x1;
            secondSignal = bigSignal;
        }
        if (y1 < y2)
        {
            firstPosY = y1;
            firstSignal = bigSignal;
            secondPosY = y2;
            secondSignal = secondBigSignal;
        }
        else if (y1 > y2)
        {
            firstPosY = y2;
            firstSignal = secondBigSignal;
            secondPosY = y1;
            secondSignal = bigSignal;
        }
        if (z1 < z2)
        {
            firstPosZ = z1;
            firstSignal = bigSignal;
            secondPosZ = z2;
            secondSignal = secondBigSignal;
        }
        else if (z1 > z2)
        {
            firstPosZ = z2;
            firstSignal = secondBigSignal;
            secondPosZ = z1;
            secondSignal = bigSignal;
        }

        Debug.Log(bigSignal); 
       Debug.Log(secondBigSignal);
       Debug.Log(bigPos);
       Debug.Log(secondBigPos);
        var desPosz = ((Math.Abs(firstPosZ - secondPosZ)) * (secondSignal / (firstSignal + secondSignal)) + firstPosZ);
        var desPosx = ((Math.Abs(firstPosX - secondPosX)) * (secondSignal / (firstSignal + secondSignal)) + firstPosX);
        var desPosy = ((Math.Abs(firstPosY - secondPosY)) * (firstSignal / (firstSignal + secondSignal)) + firstPosY);

        //var desPosZ=((Math.Abs(firstPos.transform.position.z-secondPos.transform.position.z))*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.z);
        // var desPos=new Vector3(
        //Math.Abs(firstPos.transform.position.x-secondPos.transform.position.x)*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.x,
        //Math.Abs(firstPos.transform.position.y-secondPos.transform.position.y)*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.y,
        //Math.Abs(firstPos.transform.position.z-secondPos.transform.position.z)*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.z
        //);
        var desPos = new Vector3(desPosx, desPosy, desPosz);
       Debug.Log(desPos);
       desPos=path.path.GetClosestPointOnPath(desPos);
       Debug.Log(desPos);
       LeanTween.move(miner,desPos,.5f).setEaseInCubic().setOnComplete(()=>
       {
           bigSignal=0;
           secondBigSignal=0;
       });
        

   }


    private void GetTwoBigRecieverSignal(int minerNumber)
    {
        
        var recieverList=ReadJson().minerRecieverList[minerNumber].recieverList;
       var tempSignal=recieverList[0];

       for (int i = 0; i < recieverList.Count ; i++)
       {
           var tempName=recieverList[i].recieverName;
            
           if(recieverList[i].signalCount>bigSignal)
           {
               bigSignal=recieverList[i].signalCount;
               bigSignalName=tempName;
               tempSignal=recieverList[i];
               firstSignal=i;
                
           }
       }
       recieverList.Remove(tempSignal);
       for (int i = 0; i < recieverList.Count ; i++)
       {
           var tempName=recieverList[i].recieverName;
            
           if(recieverList[i].signalCount>secondBigSignal)
           {
               secondBigSignal=recieverList[i].signalCount;
               secondBigSignalName=tempName;
               secondSignal=i;
                
           }
       }
       recieverList.Add(tempSignal);

       if(firstSignal>secondSignal)
       {
           firstSignal=secondBigSignal;
           secondSignal=bigSignal;
       }
       else
       {
           firstSignal=bigSignal;
           secondSignal=secondBigSignal;
       }

       Debug.Log(bigSignal);
       Debug.Log(secondBigSignal);
        
    }

}
