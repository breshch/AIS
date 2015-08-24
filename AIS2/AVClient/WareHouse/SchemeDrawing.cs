using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AVClient.WareHouse
{
	public class SchemeDrawing
	{
		private readonly Canvas _surface;
		private string _fontFamilyDefault = "Verdana";

		public SchemeDrawing(Canvas surfase)
		{
			_surface = surfase;
		}

		public void SetFontFamilyDefault(string fontFamily)
		{
			_fontFamilyDefault = fontFamily;
		}

		public void DrawRectangle(Point point, Size size, Brush strokeBrush, Brush fillBrush, double opacity = 1, double strokeThickness = 1)
		{
			var rectangle = new Rectangle
			{
				Width = size.Width,
				Height = size.Height,
				Fill = fillBrush,
				Stroke = strokeBrush,
				StrokeThickness = strokeThickness,
				Opacity = opacity
			};
			Canvas.SetLeft(rectangle, point.X);
			Canvas.SetTop(rectangle, point.Y);

			_surface.Children.Add(rectangle);
		}

		public void DrawString(Point point, string value, Brush foregroundBrush, double fontSize, double opacity = 1, 
			double angle = 0, bool isBold = false)
		{
			var textBlock = new TextBlock
			{
				Foreground = foregroundBrush,
				Text = value,
				FontFamily = new FontFamily(_fontFamilyDefault),
				FontSize = fontSize,
				Opacity = opacity,
				LayoutTransform = new RotateTransform(angle),
				FontWeight = isBold ? FontWeights.Bold : FontWeights.Regular
			};

			Canvas.SetLeft(textBlock, point.X);
			Canvas.SetTop(textBlock, point.Y);

			_surface.Children.Add(textBlock);
		}

		public Size GetSizeString(string value, double fontSize)
		{
			var ft = new FormattedText(value, new CultureInfo("ru-RU"), FlowDirection.LeftToRight,
				new Typeface(_fontFamilyDefault), fontSize, Brushes.Black);
			return new Size(ft.Width, ft.Height);
		}
	}
}
