
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField]
    private int minerNumber;

    
    public ReadData readData;

    [SerializeField]
    private float timer;

    [SerializeField]
    public bool timerIsActive=false;
   
    private float timerStartValue;

    private bool InDanger=false;
    private SpriteRenderer sprite;
    public int MinerNumber { get{return minerNumber;} set{ minerNumber=value;} }

    void Start()
    {
        
        timerIsActive = false;
        timerStartValue=timer;
        sprite=transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    
    void LateUpdate()
    {
        if(timerIsActive)
        {
            timer-=Time.deltaTime;
            if(timer<0)
            {
                
                readData.MoveBySignal(gameObject,minerNumber);
                timer=timerStartValue;
            }

        }
        
    }

    private void IsInDanger()
    {
        sprite.color=Color.red;
        if(!InDanger)
        {
            sprite.color=Color.white;
        }
    }

    
}
