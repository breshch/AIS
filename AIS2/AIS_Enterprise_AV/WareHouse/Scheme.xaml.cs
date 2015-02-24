using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AIS_Enterprise_AV.WareHouse
{
	/// <summary>
	/// Логика взаимодействия для Scheme.xaml
	/// </summary>
	public partial class Scheme : Window
	{
		private readonly SchemeDrawing _schemeDrawing;
		private SchemeData _schemeData;

		public Scheme()
		{
			InitializeComponent();

			InitializeWarehouse();
			_schemeDrawing = new SchemeDrawing(Surface, _schemeData);
			_schemeDrawing.DrawWarehouse();
		}

		private void InitializeWarehouse()
		{
			_schemeData = new SchemeData(22, 7, 4, 3);
			_schemeData.SetDisableCells(1, 1);
			_schemeData.SetDisableCells(2, 1);

			for (int row = 3; row <= _schemeData.CountRows; row++)
			{
				_schemeData.SetDisableCells(row, 4, 1);
				_schemeData.SetDisableCells(row, 4, 2);
			}

			for (int row = 2; row <= _schemeData.CountRows; row += 2)
			{
				_schemeData.SetRoad(new SchemeRoad
				{
					Start = row,
					Finish = row + 1,
					Type = RoadType.Row
				});
			}
		}
	}
}
