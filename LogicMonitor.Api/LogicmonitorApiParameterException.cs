using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace LogicMonitor.Api
{
	/// <summary>
	/// A LogicMonitor API parameter exception
	/// </summary>
	[Serializable]
	public class LogicMonitorApiParameterException : Exception
	{
		/// <summary>
		/// The parameter
		/// </summary>
		public string Parameter { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameter"></param>
		public LogicMonitorApiParameterException(string parameter)
		{
			Parameter = parameter;
		}

		/// <summary>
		///  Constructor
		/// </summary>
		public LogicMonitorApiParameterException()
		{
		}

		/// <summary>
		///  Constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected LogicMonitorApiParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		///  Constructor
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public LogicMonitorApiParameterException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <inheritdoc />
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("Parameter", Parameter);
		}
	}
}