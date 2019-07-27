using UnityEngine;
using System.Collections;

public class CBaseEvent{

	protected IDictionary arguments;

	protected CEventType cEventType;

	protected Object sender;

    public CEventType eventType
	{
		get
		{
			return this.cEventType;
		}
		set
		{
			this.cEventType = value;
		}
	}

    public IDictionary Params
    {
        get 
        {
            return this.arguments;
        }
        set
        {
            this.arguments = (value as Hashtable);
        }
    }

    public Object GetSender()
    {
        return this.sender;
    }

    public CBaseEvent(CEventType cEventType, Object sender)
    {
        this.cEventType = cEventType;
        this.sender = sender;
        this.arguments = new Hashtable();
    }

    public CBaseEvent(CEventType cEventType,IDictionary arguments,Object sender)
    {
        this.cEventType = cEventType;
        this.sender = sender;
        this.arguments = arguments;

        if (this.arguments == null)
            this.arguments = new Hashtable();
    }

    public string ToString()
    {
        return "Event type is " + this.cEventType + ", sender is " + this.sender == null ? "null" : this.sender.ToString();
    }

    public CBaseEvent Clone()
    {
        return new CBaseEvent(this.cEventType, this.arguments, this.sender);
    }
}
