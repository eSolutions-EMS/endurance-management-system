﻿using System;
using System.Text;

namespace EMS.Judge.Application.Hardware;
public abstract class RfidController
{
    protected TimeSpan throttle;
    protected const int TAG_DATA_START_INDEX = 4;
    protected const int TAG_DATA_LENGTH = 12;

    public RfidController(TimeSpan? throttle = null)
    {
        this.throttle = throttle ?? TimeSpan.FromSeconds(1);
    }

    public bool IsConnected { get; protected set; }
    public bool IsReading { get; protected set; }
    public bool IsWriting { get; protected set; }

    public event EventHandler<string> MessageEvent;
    public void RaiseMessage(string message)
    {
        message = $"{this.Device} {message}";
        Console.WriteLine(message);
        this.MessageEvent?.Invoke(this, message);
    }

    public event EventHandler<string> ErrorEvent;
    public void RaiseError(string error)
    {
        var message = $"{this.Device} ERROR: {error}";
        Console.WriteLine($"{this.Device} ERROR: {error}");
        this.ErrorEvent?.Invoke(this, message);
    }

    public abstract void Connect();
    public abstract void Disconnect();

    protected abstract string Device { get; }
    protected virtual byte[] ConvertToByytes(string data)
    {
        return Encoding.UTF8.GetBytes(data);
    }
    protected virtual string ConvertToString(byte[] data)
    {
        return Encoding.UTF8.GetString(data);
    }
}