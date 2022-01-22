using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    private double endingTime;
    private float fade;
    private float intencity;
    private double lastTime;

    public Sound(float duration, float intencity, float fade){
        this.fade = fade;
        this.intencity = intencity;
        this.endingTime = (double)(Time.time * 1000f) + duration;
        this.lastTime = (double)(Time.time * 1000f);
    }

    public float GetCurrentNoiseLevel(){
        return this.intencity;
    }

    public bool UpdateStatus(){
        double currentTime = (double)(Time.time * 1000f);
        if(currentTime >= endingTime) return true;
        intencity /= fade * (float) (lastTime - currentTime);
        lastTime = currentTime;
        return false;
    }
}
