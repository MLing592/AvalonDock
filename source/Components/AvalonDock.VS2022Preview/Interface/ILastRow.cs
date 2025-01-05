using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AvalonDock.Interface
{
	public interface ILastRow
	{
		/// <summary>
		/// Element in panel's row index
		/// </summary>
		public int PaneRowIndex { get; set; }

		/// <summary>
		/// Element is in panel's last row
		/// </summary>
		public bool IsPaneLastRow { get; set; }

	}
}
