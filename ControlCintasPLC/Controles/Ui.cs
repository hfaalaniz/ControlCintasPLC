using System.Drawing.Drawing2D;

namespace ControlCintasPLC.Controles
{
    /// <summary>Ejecuta una acción sobre el PLC con gestión centralizada de errores.</summary>
    public delegate Task Ejecutor(Func<Task> accion, string descripcion);

    /// <summary>Indicador LED circular con texto.</summary>
    public sealed class Led : Control
    {
        private bool _on;
        public Color ColorOn { get; set; } = Color.LimeGreen;
        public Color ColorOff { get; set; } = Color.DimGray;

        public bool On
        {
            get => _on;
            set { if (_on != value) { _on = value; Invalidate(); } }
        }

        public Led(string texto, Color colorOn)
        {
            Text = texto;
            ColorOn = colorOn;
            Size = new Size(90, 18);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int d = Height - 4;
            using var br = new SolidBrush(_on ? ColorOn : ColorOff);
            g.FillEllipse(br, 2, 2, d, d);
            g.DrawEllipse(Pens.Black, 2, 2, d, d);
            TextRenderer.DrawText(g, Text, Font, new Point(d + 6, (Height - Font.Height) / 2), ForeColor);
        }
    }

    internal static class Ui
    {
        public static Label Etiqueta(string texto, int x, int y, int w = 110, float tam = 9f, bool negrita = false) => new()
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(w, (int)(tam * 2.2f)),
            Font = new Font("Segoe UI", tam, negrita ? FontStyle.Bold : FontStyle.Regular),
            AutoEllipsis = true
        };

        public static Button Boton(string texto, int x, int y, int w = 70, int h = 26) => new()
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(w, h),
            UseVisualStyleBackColor = true
        };

        public static Led NuevoLed(string texto, Color on, int x, int y) => new(texto, on) { Location = new Point(x, y) };

        public static NumericUpDown Numero(int x, int y, decimal min, decimal max, int decimales = 1, int w = 70) => new()
        {
            Location = new Point(x, y),
            Size = new Size(w, 23),
            Minimum = min,
            Maximum = max,
            DecimalPlaces = decimales,
            Increment = decimales > 0 ? 0.5m : 1m
        };

        public static decimal Clamp(NumericUpDown n, double v) =>
            Math.Min(n.Maximum, Math.Max(n.Minimum, (decimal)(double.IsFinite(v) ? v : 0)));
    }
}
