using UnityEngine;
using System.Collections.Generic;

public delegate void CEventListenerDelegate(CBaseEvent cBaseEvent);

public class CEventDispacher
{
    public Dictionary<CEventType, CEventListenerDelegate> listeners = new Dictionary<CEventType, CEventListenerDelegate>();

    private CEventDispacher() { }

    protected static CEventDispacher instance;

    public static CEventDispacher GetInstance()
    {
        if (instance == null)
            instance = new CEventDispacher();

        return instance;
    }

    public void AddEventListener(CEventType cEventType, CEventListenerDelegate listener)
    {
        CEventListenerDelegate cEventListenerDelegate;
        this.listeners.TryGetValue(cEventType, out cEventListenerDelegate);

        //Debug.Log(cEventListenerDelegate.ToString());

        if (cEventListenerDelegate != null)
            cEventListenerDelegate += listener;
        else
            cEventListenerDelegate = listener;

        this.listeners[cEventType] = cEventListenerDelegate;
    }

    public void RemoveEventListener(CEventType cEventType, CEventListenerDelegate listener)
    {
        CEventListenerDelegate cEventListenerDelegate = this.listeners[cEventType];
        
        if (cEventListenerDelegate != null)
            cEventListenerDelegate -= listener;

        this.listeners[cEventType] = cEventListenerDelegate;
    }

    public void RemoveAll()
    {
        this.listeners.Clear();
    }

    public void DispatchEvent(CBaseEvent cBaseEvent)
    {
        CEventListenerDelegate cEventListenerDelegate = this.listeners[cBaseEvent.eventType];

        if (cEventListenerDelegate != null)
        {
            try
            {
                cEventListenerDelegate(cBaseEvent);
            }
            catch (System.Exception e)
            {

                Debug.Log("An exception was thrown.The message was:" + e.Message + ".The detail of stack trace was:" + e.StackTrace);
            }
        }
    }
}
