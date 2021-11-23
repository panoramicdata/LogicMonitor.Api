using LogicMonitor.Api.Extensions;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data;

/// <summary>
/// A graph line
/// </summary>
[DataContract]
public class Line
{
	/// <summary>
	/// The line color
	/// </summary>
	[IgnoreDataMember]
	public Color Color
	{
		get
		{
			var color = ColorString.StartsWith("#") ? ColorString.Substring(1) : ColorString;
			return color.Length switch
			{
				3 => Color.FromArgb(HexToInt(color[0]) * 255 / 16, HexToInt(color[1]) * 255 / 16, HexToInt(color[2]) * 255 / 16),
				6 => Color.FromArgb(
						(HexToInt(color[0]) * 16) + HexToInt(color[1]),
						(HexToInt(color[2]) * 16) + HexToInt(color[3]),
						(HexToInt(color[4]) * 16) + HexToInt(color[5])),
				_ => throw new ArgumentOutOfRangeException(),
			};
		}
		set => ColorString = value.ToHtml();
	}

	private static int HexToInt(char h)
	{
		if (h >= '0' && h <= '9')
		{
			return h - '0';
		}
		if (h >= 'a' && h <= 'f')
		{
			return h - 'a' + 10;
		}
		if (h >= 'A' && h <= 'F')
		{
			return h - 'A' + 10;
		}
		throw new ArgumentOutOfRangeException();
	}

	/// <summary>
	/// The line color name
	/// </summary>
	[DataMember(Name = "colorName")]
	public string ColorName { get; set; }

	/// <summary>
	/// The line color string
	/// </summary>
	[DataMember(Name = "color")]
	public string ColorString { get; set; }

	/// <summary>
	/// The line data.
	/// Must be implemented as an object as LogicMonitor have decided that null should be represented as "No Data" (brilliant!)
	/// </summary>
	[DataMember(Name = "data")]
	public object[] DataInternal { get; set; }

	/// <summary>
	/// The line data, accessing the DataInternal
	/// </summary>
	[IgnoreDataMember]
	public double?[] Data
	{
		get => DataInternal.Select(@object => double.TryParse(@object.ToString(), out var result) ? result : (double?)null).ToArray();
		set => DataInternal = value.Select(v => v ?? (object)"No Data").ToArray();
	}

	/// <summary>
	/// The datapoint id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The datapoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	/// <summary>
	/// The line decimal
	/// </summary>
	[DataMember(Name = "decimal")]
	public int Decimal { get; set; }

	/// <summary>
	/// The line description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	/// Whether it is a line for a virtual datapoint
	/// </summary>
	[DataMember(Name = "isVirtualDataPoint")]
	public bool IsVirtualDataPoint { get; set; }

	/// <summary>
	/// The maximum value
	/// </summary>
	[DataMember(Name = "max")]
	public double Maximum { get; set; }

	/// <summary>
	/// The mean
	/// </summary>
	[DataMember(Name = "avg")]
	public double Mean { get; set; }

	/// <summary>
	/// The minimum value
	/// </summary>
	[DataMember(Name = "min")]
	public double Minimum { get; set; }

	/// <summary>
	/// The standard deviation from the mean
	/// </summary>
	[DataMember(Name = "std")]
	public double StandardDeviationFromMean { get; set; }

	/// <summary>
	/// The line legend
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; }

	/// <summary>
	/// The line label
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; }

	/// <summary>
	/// The line type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// Whether to use the YMax
	/// </summary>
	[DataMember(Name = "useYMax")]
	public bool UseYMax { get; set; }

	/// <summary>
	/// Whether the line is visible
	/// </summary>
	[DataMember(Name = "visible")]
	public bool Visible { get; set; }

	/// <summary>
	/// The line weight
	/// </summary>
	[DataMember(Name = "weight")]
	public int Weight { get; set; }

	/// <summary>
	/// String representation
	/// </summary>
	public override string ToString() => $"{Legend} ({ColorName})";
}
