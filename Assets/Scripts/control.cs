using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class control : MonoBehaviour
{

    public KnobController knob1;
    public KnobController knob2;
    public KnobController knob3;

    public float timeToNext = 2f;

    public enum States {  g_2020, g_2030, g_2040, g_2050, g_2060, g_2070, g_2080, g_2090, g_2100,
     o_2020, o_2030, o_2040, o_2050, o_2060, o_2070, o_2080, o_2090, o_2100,
     r_2020, r_2030, r_2040, r_2050, r_2060, r_2070, r_2080, r_2090, r_2100,};
    

    float[] knob1Values = {0,1,2,3,4,5,6,7,8,0,1,2,3,4,5,6,7,8,0,1,2,3,4,5,6,7,8};
    float[] knob23Values = {2020,2030,2040,2050,2060,2070,2080,2090,2100,
    2020,2030,2040,2050,2060,2070,2080,2090,2100,
    2020,2030,2040,2050,2060,2070,2080,2090,2100};

    bool isGreenDown = false;
    bool isOrangeDown = false;
    bool isRedDown = false;
    
    bool greenWasReleased = false;
    bool orangeWasReleased = false;
    bool redWasReleased = false;
    


    float timer = 0;
    States currentState = States.g_2020;

    // for a slider based on images
    public Sprite [] sprites;
    public Image sliderSprite;


    void Awake()
    {
        setToGreen();
        sliderSprite.sprite = sprites[4];
    }
    void Update()
    {
        sliderSprite.sprite = sprites[(int)knob1Values[(int)currentState]];
        // State machine
        switch (currentState){
            case States.g_2020: case States.g_2030: case States.g_2040: case States.g_2050: case States.g_2060: case States.g_2070: case States.g_2080: case States.g_2090: case States.g_2100:
                knob1.SetValue(knob1Values[(int)currentState]);
                knob2.SetValue(knob23Values[(int)currentState]);
                knob3.SetValue(knob23Values[(int)currentState] + timer*10/timeToNext);
                if (isGreenDown) {
                    timer += Time.deltaTime;
                    if (timer >= timeToNext) {
                        currentState = (currentState != States.g_2100) ? currentState+1 : States.g_2020;
                        timer = 0;
                    } 
                } 
                if (greenWasReleased) {
                    currentState = (currentState != States.g_2100) ? currentState+1 : States.g_2020;
                    timer = 0;
                    greenWasReleased = false;
                }
                if (isOrangeDown) {
                    currentState+=9;
                    timer = 0;
                    setToOrange();
                }
                if (isRedDown) {
                    currentState+=18;
                    timer = 0;
                    setToRed();
                }
                break;
           
            case States.o_2020: case States.o_2030: case States.o_2040: case States.o_2050: case States.o_2060: case States.o_2070: case States.o_2080: case States.o_2090: case States.o_2100:
                knob1.SetValue(knob1Values[(int)currentState]);
                knob2.SetValue(knob23Values[(int)currentState]);
                knob3.SetValue(knob23Values[(int)currentState] + timer*10/timeToNext);
                if (isOrangeDown) {
                    timer += Time.deltaTime;
                    if (timer >= timeToNext) {
                        currentState = (currentState != States.o_2100) ? currentState+1 : States.o_2020;
                        timer = 0;
                    } 
                } 
                if (orangeWasReleased) {
                    currentState = (currentState != States.o_2100) ? currentState+1 : States.o_2020;
                    timer = 0;
                    orangeWasReleased = false;
                }
                if (isGreenDown) {
                    currentState-=9;
                    timer = 0;
                    setToGreen();
                }
                if (isRedDown) {
                    currentState+=9;
                    timer = 0;
                    setToRed();
                }
                break;
            
            case States.r_2020: case States.r_2030: case States.r_2040: case States.r_2050: case States.r_2060: case States.r_2070: case States.r_2080: case States.r_2090: case States.r_2100: 
                knob1.SetValue(knob1Values[(int)currentState]);
                knob2.SetValue(knob23Values[(int)currentState]);
                knob3.SetValue(knob23Values[(int)currentState] + timer*10/timeToNext);
                if (isRedDown) {
                    timer += Time.deltaTime;
                    if (timer >= timeToNext) {
                        currentState = (currentState != States.r_2100) ? currentState+1 : States.r_2020;
                        timer = 0;
                    } 
                } 
                if (redWasReleased) {
                    currentState = (currentState != States.r_2100) ? currentState+1 : States.r_2020;
                    timer = 0;
                    redWasReleased = false;
                }
                if (isOrangeDown) {
                    currentState-=9;
                    timer = 0;
                    setToOrange();
                }
                if (isGreenDown) {
                    setToGreen();
                    currentState-=18;
                    timer = 0;
                }
                break;
        }

    }

    public void HandleKnob(float value) {
        Debug.Log("Handle Knob   " + value);
    }


    public void GreenMouseDownHandle(){
        isGreenDown = true;
    }

    public void GreenMouseUpHandle(){
        isGreenDown = false;
        greenWasReleased = true;
    }  

    public void OrangeMouseDownHandle(){
        isOrangeDown = true;
    }

    public void OrangeMouseUpHandle(){
        isOrangeDown = false;
        orangeWasReleased = true;
    } 
    public void RedMouseDownHandle(){
        isRedDown = true;
    }

    public void RedMouseUpHandle(){
        isRedDown = false;
        redWasReleased = true;
    }

    public void setToGreen(){
        sliderSprite.color = new Color(0.48f, 0.82f, 0.29f, 1f);
        knob1.SetColor( new Color(0.48f, 0.82f, 0.29f, 1f));
        knob2.SetColor( new Color(0.48f, 0.82f, 0.29f, 1f));
        knob3.SetColor( new Color(0.48f, 0.82f, 0.29f, 1f));
    }
    public void setToOrange() {
        sliderSprite.color = new Color(0.85f, 0.57f, 0.04f, 1f);
        knob1.SetColor( new Color(0.85f, 0.57f, 0.04f, 1f));
        knob2.SetColor( new Color(0.85f, 0.57f, 0.04f, 1f));
        knob3.SetColor( new Color(0.85f, 0.57f, 0.04f, 1f));
    }
    public void setToRed() {
        sliderSprite.color = new Color(0.85f, 0.19f, 0.04f, 1f);
        knob1.SetColor( new Color(0.85f, 0.19f, 0.04f, 1f));
        knob2.SetColor( new Color(0.85f, 0.19f, 0.04f, 1f));
        knob3.SetColor( new Color(0.85f, 0.19f, 0.04f, 1f));
    }

}
