using System;
using System.Runtime.Serialization;

namespace LogicMonitor.Api;

/// <summary>
/// Occurs when an operation is to be performed on a Property but the Property cannot be found
/// </summary>
[Serializable]
public class PropertyNotFoundException : Exception
{
	/// <summary>
	/// PropertyNotFoundException
	/// </summary>
	public PropertyNotFoundException()
	{
	}

	/// <summary>
	/// PropertyNotFoundException
	/// </summary>
	/// <param name="message"></param>
	public PropertyNotFoundException(string message) : base(message)
	{
	}

	/// <summary>
	/// PropertyNotFoundException
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// PropertyNotFoundException
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected PropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
