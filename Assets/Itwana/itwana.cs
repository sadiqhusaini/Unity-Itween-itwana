﻿using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class itwana : MonoBehaviour {

    #region  Enums

    public enum Axis{
        None,X,Y,Z,XY,XZ,YZ,XYZ,TargetLocation
    }
    public enum Type{
        None,Scale,Move,Rotate,Stab,FollowPath,Audio
    }
    public enum Method{
        None,To,From,Add,Update,By,Punch,Shake
    }

    #endregion    
   
    
    
    #region Variables
	
    public Type type;
    public Method  method;
    public Axis axis;
    public iTween.EaseType easeType=iTween.EaseType.linear;
    public iTween.LoopType loopType;

    public GameObject targetGameObject;

    public AudioClip clip;
    public float x, y, z;
    public float time=1, delay;
	
    public float pitch=1;
    public float volume=1;
	
    
    public bool repeat=false;
    public bool ignoreTimeScale=false;
    public bool onClick = false;
    public string pathName="Path0";


    public GameObject otherObject;
    
	
    #endregion
    private   Hashtable hash=new Hashtable();

    private Vector3 pos,rot,scal;

    private GameObject objectForTween;

    private void Awake()
    {
        SetObjectForTween();
    }

    void SetObjectForTween()
    {
        if (otherObject)
        {
            objectForTween = otherObject;
        }
        else
        {
            objectForTween = this.gameObject;
        }
    }


    void OnEnable()
    {
        SetObjectForTween();
        pos = objectForTween.transform.localPosition;
        scal = objectForTween.transform.localScale;
        rot = objectForTween.transform.localEulerAngles;
        
        if(repeat)
            if(!onClick)
                StartTweens();
    }
    void Start(){
        if (!repeat)
            if(!onClick)
                StartTweens();
    }
    
    private void OnDisable()
    {
        if (repeat) {
            objectForTween. transform.localPosition = pos;
            objectForTween.     transform.localScale = scal;
            objectForTween.  transform.localEulerAngles = rot;
        }
    }


    void StartTweens(){
        
        hash=new Hashtable();
        
        if (method != Method.Update)
        {
            hash.Add("EaseType", "" + easeType);
            hash.Add("LoopType", "" + loopType);
        }

        hash.Add("time", time);
        hash.Add("delay", delay);

        if (ignoreTimeScale)
        {
            hash.Add("ignoretimescale", ignoreTimeScale);
        }
        
        switch(type)
        {
            case Type.Move:
            {
                MoveMethods ();
                break;
            }
            case Type.Rotate:
            {
                RotateMethods();
                break;
            }
            case Type.Scale:
            {
                ScaleMethods ();
                break;
            }
            case Type.Stab:
            {
                StabMethod ();
                break;
            }
            case Type.FollowPath:
            {
                MovePathMethod ();
                break;
            }
            case Type.Audio:
            {
                AudioMethods ();
                break;
            }
            default: break;
                Debug.LogError ("Select Correct type of itween");
        }
        

    }
    void Update(){
        
        // iTween.ScaleUpdate (objectForTween, hash); 
        //
        // iTween.MoveUpdate (objectForTween, hash); 
        // iTween.RotateUpdate (objectForTween, hash); 
        // iTween.AudioUpdate (objectForTween, hash); 
        //
    }	

    public void OnClickTwana()
    {
        StartTweens();
    }

    void MovePathMethod (){
        hash.Add ("path", iTweenPath.GetPath (pathName));
        iTween.MoveTo (objectForTween,hash);
    }

    void AudioMethods(){
        ////////////////////////////Method To////////////////////////////////////
        if (Method.To == method) {
      
                iTween.AudioTo (objectForTween,hash);
        }
        else if (Method.From== method) {

          
                iTween.AudioFrom (objectForTween,hash);
        }
        else if (Method.Update== method) {

                iTween.AudioFrom (objectForTween,hash);
        }
        else
        {
            Debug.Log("there is no method in Type " + type);
        }
    }
    
    private void StabMethod (){
        hash.Add ("Audioclip", clip);
        hash.Add ("pitch",pitch);
        hash.Add ("volume", volume);
            iTween.Stab (objectForTween, hash );
    }

    private void SetAxisValues(ref Vector3 temp)
    {
        switch (axis)
        {
            case Axis.X:
            {
                temp.x = x;
                if (method == Method.Punch || method == Method.Shake)
                {
                    temp.y = 0;
                    temp.z = 0;
                }
                break;
            }
            case Axis.Y:
            {
                temp.y = y;
                if (method == Method.Punch || method == Method.Shake)
                {
                    temp.x = 0;
                    temp.z = 0;
                }
                break;
            }
            case Axis.Z:
            {
                temp.z = z;
                if (method == Method.Punch || method == Method.Shake)
                {
                    temp.x = 0;
                    temp.y = 0;
                }
                break;
            }
            case Axis.XY:
            {
                temp.x = x;
                temp.y = y;
                
                if (method == Method.Punch || method == Method.Shake)
                {
                    temp.z = 0;
                }
                break;
            }
            case Axis.YZ:
            {
                temp.y = y;
                temp.z = z;
                
                if (method == Method.Punch || method == Method.Shake)
                {
                    temp.x = 0;
                }
                break;
            }
            case Axis.XZ:
            {
                temp.x = x;
                temp.z = z;

                if (method == Method.Punch || method == Method.Shake)
                {
                    temp.y = 0;
                }
                break;
            }
            case Axis.XYZ:
            {
                temp.x = x;
                temp.y = y;
                temp.z = z;
                break;
            }
            case Axis.TargetLocation:
            {
                if (type == Type.Move)
                {
                    temp = targetGameObject.transform.position;
                }
                else if (type == Type.Rotate)
                {
                    temp = targetGameObject.transform.localEulerAngles;
                }
                else if (type == Type.Scale)
                {
                    temp = targetGameObject.transform.localScale;
                }

                break;
            }
        }

    
    }
    
    private  void ScaleMethods(){

        
        Vector3 tempScale = objectForTween.transform.localScale;
        SetAxisValues(ref tempScale);
        hash.Add ("scale", tempScale);
        
        switch (method)
        {
            case Method.To:
            {
                iTween.ScaleTo (objectForTween,hash);
                break;
            }
            case Method.From:
            {
                iTween.ScaleFrom (objectForTween,hash);
                break;
            }
            case Method.Add:
            {
                iTween.ScaleAdd(objectForTween,hash);
                break;
            }
            case Method.Update:
            {
                
                break;
            }
            case Method.By:
            {
                iTween.ScaleBy(objectForTween,hash);
                break;
            }
            case Method.Punch:
            {
                iTween.PunchScale (objectForTween,hash);
                break;
            }
            case Method.Shake:
            {
                iTween.ShakeScale (objectForTween,hash);
                break;
            }
        }


    }

    private void MoveMethods(){
        Vector3 tempPos = objectForTween.transform.localPosition;
        SetAxisValues(ref tempPos);
        hash.Add ("position", tempPos);
        
        switch (method)
        {
            case Method.To:
            {
                iTween.MoveTo (objectForTween,hash);
                break;
            }
            case Method.From:
            {
                iTween.MoveFrom (objectForTween,hash);
                break;
            }
            case Method.Add:
            {
                iTween.MoveAdd(objectForTween,hash);
                break;
            }
            case Method.Update:
            {
                
                break;
            }
            case Method.By:
            {
                iTween.MoveBy(objectForTween,hash);
                break;
            }
            case Method.Punch:
            {
                iTween.PunchPosition (objectForTween,hash);
                break;
            }
            case Method.Shake:
            {
                iTween.ShakePosition (objectForTween,hash);
                break;
            }
        }
    }

    private void RotateMethods()
    {

        Vector3 tempRot = objectForTween.transform.localEulerAngles;
        SetAxisValues(ref tempRot);
        hash.Add ("rotation", tempRot);
        
        switch (method)
        {
            case Method.To:
            {
                iTween.RotateTo(objectForTween,hash);
                break;
            }
            case Method.From:
            {
                iTween.RotateFrom(objectForTween,hash);
                break;
            }
            case Method.Add:
            {
                iTween.RotateAdd(objectForTween,hash);
                break;
            }
            case Method.Update:
            {
                
                break;
            }
            case Method.By:
            {
                iTween.RotateBy(objectForTween,hash);
                break;
            }
            case Method.Punch:
            {
                iTween.PunchRotation (objectForTween,hash);
                break;
            }
            case Method.Shake:
            {
                iTween.ShakeRotation (objectForTween,hash);
                break;
            }
        }
    }
}