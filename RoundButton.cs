using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundButton : Button
{
    protected override void OnPaint(PaintEventArgs e)
    {
        // Define o caminho da forma (elipse/círculo)
        GraphicsPath grPath = new GraphicsPath();
        grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);

        // Aplica o formato ao botão
        this.Region = new System.Drawing.Region(grPath);

        base.OnPaint(e);
    }
}