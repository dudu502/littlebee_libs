using Engine.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnityLogger : Engine.Common.Log.ILogger
{
    private string _tag;
    private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";

    public bool IsDebugEnabled { get; set; } = true;

    public UnityLogger(string tag)
    {
        _tag = tag;
    }
    public void Error(string message)
    {
        if (IsDebugEnabled)
            Debug.LogError($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_tag}]\t{message}");
    }

    public void Info(string message)
    {
        if (IsDebugEnabled)
            Debug.Log($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_tag}]\t{message}");
    }

    public void Warn(string message)
    {
        if (IsDebugEnabled)
            Debug.LogWarning($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_tag}]\t{message}");
    }

    public void Info(object message)
    {
        Info(message.ToString());
    }

    public void Warn(object message)
    {
        Warn(message.ToString());
    }

    public void Error(object message)
    {
        Error(message.ToString());
    }
}
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Context context = new Context(Context.EDITOR,new UnityLogger("Editor"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
