﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34209
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FollowMe.Enums
{
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TargetLocation", Namespace="http://schemas.datacontract.org/2004/07/FollowMe.Enums")]
    public enum TargetLocation : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unknown = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TopLeft = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TopCenter = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TopRight = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CenterLeft = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CenterCenter = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CenterRight = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BottomLeft = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BottomCenter = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BottomRight = 9,
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IRemoteControl")]
public interface IRemoteControl
{
    
    //[System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteControl/Start", ReplyAction="http://tempuri.org/IRemoteControl/StartResponse")]
    //void Start();
    
    [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRemoteControl/Start", ReplyAction="http://tempuri.org/IRemoteControl/StartResponse")]
    System.IAsyncResult BeginStart(System.AsyncCallback callback, object asyncState);
    
    void EndStart(System.IAsyncResult result);
    
    //[System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteControl/Stop", ReplyAction="http://tempuri.org/IRemoteControl/StopResponse")]
    //void Stop();
    
    [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRemoteControl/Stop", ReplyAction="http://tempuri.org/IRemoteControl/StopResponse")]
    System.IAsyncResult BeginStop(System.AsyncCallback callback, object asyncState);
    
    void EndStop(System.IAsyncResult result);
    
    //[System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteControl/GetPersonLocation", ReplyAction="http://tempuri.org/IRemoteControl/GetPersonLocationResponse")]
    //FollowMe.Enums.TargetLocation GetPersonLocation();
    
    [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRemoteControl/GetPersonLocation", ReplyAction="http://tempuri.org/IRemoteControl/GetPersonLocationResponse")]
    System.IAsyncResult BeginGetPersonLocation(System.AsyncCallback callback, object asyncState);
    
    FollowMe.Enums.TargetLocation EndGetPersonLocation(System.IAsyncResult result);
    
    //[System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteControl/GetDangerLocation", ReplyAction="http://tempuri.org/IRemoteControl/GetDangerLocationResponse")]
    //FollowMe.Enums.TargetLocation GetDangerLocation();
    
    [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRemoteControl/GetDangerLocation", ReplyAction="http://tempuri.org/IRemoteControl/GetDangerLocationResponse")]
    System.IAsyncResult BeginGetDangerLocation(System.AsyncCallback callback, object asyncState);
    
    FollowMe.Enums.TargetLocation EndGetDangerLocation(System.IAsyncResult result);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IRemoteControlChannel : IRemoteControl, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class GetPersonLocationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
{
    
    private object[] results;
    
    public GetPersonLocationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState)
    {
        this.results = results;
    }
    
    public FollowMe.Enums.TargetLocation Result
    {
        get
        {
            base.RaiseExceptionIfNecessary();
            return ((FollowMe.Enums.TargetLocation)(this.results[0]));
        }
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class GetDangerLocationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
{
    
    private object[] results;
    
    public GetDangerLocationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState)
    {
        this.results = results;
    }
    
    public FollowMe.Enums.TargetLocation Result
    {
        get
        {
            base.RaiseExceptionIfNecessary();
            return ((FollowMe.Enums.TargetLocation)(this.results[0]));
        }
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class RemoteControlClient : System.ServiceModel.ClientBase<IRemoteControl>, IRemoteControl
{
    
    private BeginOperationDelegate onBeginStartDelegate;
    
    private EndOperationDelegate onEndStartDelegate;
    
    private System.Threading.SendOrPostCallback onStartCompletedDelegate;
    
    private BeginOperationDelegate onBeginStopDelegate;
    
    private EndOperationDelegate onEndStopDelegate;
    
    private System.Threading.SendOrPostCallback onStopCompletedDelegate;
    
    private BeginOperationDelegate onBeginGetPersonLocationDelegate;
    
    private EndOperationDelegate onEndGetPersonLocationDelegate;
    
    private System.Threading.SendOrPostCallback onGetPersonLocationCompletedDelegate;
    
    private BeginOperationDelegate onBeginGetDangerLocationDelegate;
    
    private EndOperationDelegate onEndGetDangerLocationDelegate;
    
    private System.Threading.SendOrPostCallback onGetDangerLocationCompletedDelegate;
    
    public RemoteControlClient()
    {
    }
    
    public RemoteControlClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public RemoteControlClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public RemoteControlClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public RemoteControlClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> StartCompleted;
    
    public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> StopCompleted;
    
    public event System.EventHandler<GetPersonLocationCompletedEventArgs> GetPersonLocationCompleted;
    
    public event System.EventHandler<GetDangerLocationCompletedEventArgs> GetDangerLocationCompleted;
    
    //public void Start()
    //{
    //    base.Channel.Start();
    //}
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginStart(System.AsyncCallback callback, object asyncState)
    {
        return base.Channel.BeginStart(callback, asyncState);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public void EndStart(System.IAsyncResult result)
    {
        base.Channel.EndStart(result);
    }
    
    private System.IAsyncResult OnBeginStart(object[] inValues, System.AsyncCallback callback, object asyncState)
    {
        return this.BeginStart(callback, asyncState);
    }
    
    private object[] OnEndStart(System.IAsyncResult result)
    {
        this.EndStart(result);
        return null;
    }
    
    private void OnStartCompleted(object state)
    {
        if ((this.StartCompleted != null))
        {
            InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
            this.StartCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
        }
    }
    
    public void StartAsync()
    {
        this.StartAsync(null);
    }
    
    public void StartAsync(object userState)
    {
        if ((this.onBeginStartDelegate == null))
        {
            this.onBeginStartDelegate = new BeginOperationDelegate(this.OnBeginStart);
        }
        if ((this.onEndStartDelegate == null))
        {
            this.onEndStartDelegate = new EndOperationDelegate(this.OnEndStart);
        }
        if ((this.onStartCompletedDelegate == null))
        {
            this.onStartCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnStartCompleted);
        }
        base.InvokeAsync(this.onBeginStartDelegate, null, this.onEndStartDelegate, this.onStartCompletedDelegate, userState);
    }
    
    //public void Stop()
    //{
    //    base.Channel.Stop();
    //}
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginStop(System.AsyncCallback callback, object asyncState)
    {
        return base.Channel.BeginStop(callback, asyncState);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public void EndStop(System.IAsyncResult result)
    {
        base.Channel.EndStop(result);
    }
    
    private System.IAsyncResult OnBeginStop(object[] inValues, System.AsyncCallback callback, object asyncState)
    {
        return this.BeginStop(callback, asyncState);
    }
    
    private object[] OnEndStop(System.IAsyncResult result)
    {
        this.EndStop(result);
        return null;
    }
    
    private void OnStopCompleted(object state)
    {
        if ((this.StopCompleted != null))
        {
            InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
            this.StopCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
        }
    }
    
    public void StopAsync()
    {
        this.StopAsync(null);
    }
    
    public void StopAsync(object userState)
    {
        if ((this.onBeginStopDelegate == null))
        {
            this.onBeginStopDelegate = new BeginOperationDelegate(this.OnBeginStop);
        }
        if ((this.onEndStopDelegate == null))
        {
            this.onEndStopDelegate = new EndOperationDelegate(this.OnEndStop);
        }
        if ((this.onStopCompletedDelegate == null))
        {
            this.onStopCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnStopCompleted);
        }
        base.InvokeAsync(this.onBeginStopDelegate, null, this.onEndStopDelegate, this.onStopCompletedDelegate, userState);
    }
    
    //public FollowMe.Enums.TargetLocation GetPersonLocation()
    //{
    //    return base.Channel.GetPersonLocation();
    //}
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginGetPersonLocation(System.AsyncCallback callback, object asyncState)
    {
        return base.Channel.BeginGetPersonLocation(callback, asyncState);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public FollowMe.Enums.TargetLocation EndGetPersonLocation(System.IAsyncResult result)
    {
        return base.Channel.EndGetPersonLocation(result);
    }
    
    private System.IAsyncResult OnBeginGetPersonLocation(object[] inValues, System.AsyncCallback callback, object asyncState)
    {
        return this.BeginGetPersonLocation(callback, asyncState);
    }
    
    private object[] OnEndGetPersonLocation(System.IAsyncResult result)
    {
        FollowMe.Enums.TargetLocation retVal = this.EndGetPersonLocation(result);
        return new object[] {
                retVal};
    }
    
    private void OnGetPersonLocationCompleted(object state)
    {
        if ((this.GetPersonLocationCompleted != null))
        {
            InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
            this.GetPersonLocationCompleted(this, new GetPersonLocationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
        }
    }
    
    //public void GetPersonLocationAsync()
    //{
    //    this.GetPersonLocationAsync(null);
    //}
    
    public void GetPersonLocationAsync(object userState)
    {
        if ((this.onBeginGetPersonLocationDelegate == null))
        {
            this.onBeginGetPersonLocationDelegate = new BeginOperationDelegate(this.OnBeginGetPersonLocation);
        }
        if ((this.onEndGetPersonLocationDelegate == null))
        {
            this.onEndGetPersonLocationDelegate = new EndOperationDelegate(this.OnEndGetPersonLocation);
        }
        if ((this.onGetPersonLocationCompletedDelegate == null))
        {
            this.onGetPersonLocationCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetPersonLocationCompleted);
        }
        base.InvokeAsync(this.onBeginGetPersonLocationDelegate, null, this.onEndGetPersonLocationDelegate, this.onGetPersonLocationCompletedDelegate, userState);
    }
    
    //public FollowMe.Enums.TargetLocation GetDangerLocation()
    //{
    //    return base.Channel.GetDangerLocation();
    //}
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginGetDangerLocation(System.AsyncCallback callback, object asyncState)
    {
        return base.Channel.BeginGetDangerLocation(callback, asyncState);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public FollowMe.Enums.TargetLocation EndGetDangerLocation(System.IAsyncResult result)
    {
        return base.Channel.EndGetDangerLocation(result);
    }
    
    private System.IAsyncResult OnBeginGetDangerLocation(object[] inValues, System.AsyncCallback callback, object asyncState)
    {
        return this.BeginGetDangerLocation(callback, asyncState);
    }
    
    private object[] OnEndGetDangerLocation(System.IAsyncResult result)
    {
        FollowMe.Enums.TargetLocation retVal = this.EndGetDangerLocation(result);
        return new object[] {
                retVal};
    }
    
    private void OnGetDangerLocationCompleted(object state)
    {
        if ((this.GetDangerLocationCompleted != null))
        {
            InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
            this.GetDangerLocationCompleted(this, new GetDangerLocationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
        }
    }
    
    public void GetDangerLocationAsync()
    {
        this.GetDangerLocationAsync(null);
    }
    
    public void GetDangerLocationAsync(object userState)
    {
        if ((this.onBeginGetDangerLocationDelegate == null))
        {
            this.onBeginGetDangerLocationDelegate = new BeginOperationDelegate(this.OnBeginGetDangerLocation);
        }
        if ((this.onEndGetDangerLocationDelegate == null))
        {
            this.onEndGetDangerLocationDelegate = new EndOperationDelegate(this.OnEndGetDangerLocation);
        }
        if ((this.onGetDangerLocationCompletedDelegate == null))
        {
            this.onGetDangerLocationCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetDangerLocationCompleted);
        }
        base.InvokeAsync(this.onBeginGetDangerLocationDelegate, null, this.onEndGetDangerLocationDelegate, this.onGetDangerLocationCompletedDelegate, userState);
    }
}
