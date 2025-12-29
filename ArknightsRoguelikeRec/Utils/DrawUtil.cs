using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

public static class DrawUtil
{
    public static void DrawLine(Graphics graphics, Point startPoint, Point endPoint, Color? color = null, float width = 1)
    {
        if (graphics == null) throw new ArgumentNullException($"DrawLine: {nameof(graphics)}");
        using Pen pen = GetPen(color, width);
        graphics.DrawLine(pen, startPoint, endPoint);
    }

    public static void DrawRectangle(Graphics graphics, Rectangle rectangle, Color? color = null, float width = 1)
    {
        if (graphics == null) throw new ArgumentNullException($"DrawRectangle: {nameof(graphics)}");
        using Pen pen = GetPen(color, width);
        graphics.DrawRectangle(pen, rectangle);
    }

    public static void FillRectangle(Graphics graphics, Rectangle rectangle, Color? color = null)
    {
        if (graphics == null) throw new ArgumentNullException($"FillRectangle: {nameof(graphics)}");
        using Brush brush = GetBrush(color);
        graphics.FillRectangle(brush, rectangle);
    }

    public static void DrawCircle(Graphics graphics, Point origin, int radius, Color? color = null, float width = 1)
    {
        if (graphics == null) throw new ArgumentNullException($"DrawCircle: {nameof(graphics)}");
        using Pen pen = GetPen(color, width);
        Rectangle rectangle = new Rectangle(origin.X - radius, origin.Y - radius, 2 * radius, 2 * radius);
        graphics.DrawEllipse(pen, rectangle);
    }

    public static void FillCircle(Graphics graphics, Point origin, int radius, Color? color = null)
    {
        if (graphics == null) throw new ArgumentNullException($"FillCircle: {nameof(graphics)}");
        using Brush brush = GetBrush(color);
        Rectangle rectangle = new Rectangle(origin.X - radius, origin.Y - radius, 2 * radius, 2 * radius);
        graphics.FillEllipse(brush, rectangle);
    }

    public static void DrawPoint(Graphics graphics, Point origin, Color? color = null, int width = 1)
    {
        if (graphics == null) throw new ArgumentNullException($"DrawPoint: {nameof(graphics)}");
        FillCircle(graphics, origin, width, color);
    }

    public static void DrawString(Graphics graphics, string text, Rectangle rectangle, Font font, Color? color = null, TextFormatFlags textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter)
    {
        if (graphics == null) throw new ArgumentNullException($"DrawString: {nameof(graphics)}");
        TextRenderer.DrawText(graphics, text, font, rectangle, color ?? Color.Black, textFormatFlags);
    }

    public static void DrawBezier(Graphics graphics, Point pt1, Point pt2, Point pt3, Point pt4, Color? color = null, float width = 1)
    {
        if (graphics == null) throw new ArgumentNullException($"DrawBezier: {nameof(graphics)}");
        using Pen pen = GetPen(color, width);
        graphics.DrawBezier(pen, pt1, pt2, pt3, pt4);
    }

    public static void DrawLine(Image image, Point startPoint, Point endPoint, Color? color = null, float width = 1f)
    {
        if (image == null) throw new ArgumentNullException($"DrawLine: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        DrawLine(graphics, startPoint, endPoint, color, width);
    }

    public static void DrawRectangle(Image image, Rectangle rectangle, Color? color = null, float width = 1f)
    {
        if (image == null) throw new ArgumentNullException($"DrawRectangle: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        DrawRectangle(graphics, rectangle, color, width);
    }

    public static void FillRectangle(Image image, Rectangle rectangle, Color? color = null)
    {
        if (image == null) throw new ArgumentNullException($"FillRectangle: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        FillRectangle(graphics, rectangle, color);
    }

    public static void DrawCircle(Image image, Point origin, int radius, Color? color = null, float width = 1f)
    {
        if (image == null) throw new ArgumentNullException($"DrawCircle: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        DrawCircle(graphics, origin, radius, color, width);
    }

    public static void FillCircle(Image image, Point origin, int radius, Color? color = null)
    {
        if (image == null) throw new ArgumentNullException($"FillCircle: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        FillCircle(graphics, origin, radius, color);
    }

    public static void DrawPoint(Image image, Point origin, Color? color = null, int width = 1)
    {
        if (image == null) throw new ArgumentNullException($"DrawPoint: {nameof(image)}");
        FillCircle(image, origin, width, color);
    }

    public static void DrawString(Image image, string text, Rectangle rectangle, Font font, Color? color = null, TextFormatFlags textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter)
    {
        if (image == null) throw new ArgumentNullException($"DrawString: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        DrawString(graphics, text, rectangle, font, color, textFormatFlags);
    }

    public static void DrawBezier(Image image, Point pt1, Point pt2, Point pt3, Point pt4, Color? color = null, float width = 1f)
    {
        if (image == null) throw new ArgumentNullException($"DrawBezier: {nameof(image)}");
        using Graphics graphics = GetGraphics(image);
        DrawBezier(graphics, pt1, pt2, pt3, pt4, color, width);
    }

    private static Graphics GetGraphics(Image image)
    {
        if (image == null) throw new ArgumentNullException($"GetGraphics: {nameof(image)}");
        Graphics graphics = Graphics.FromImage(image);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        return graphics;
    }

    private static Pen GetPen(Color? color, float width)
    {
        Pen pen = new Pen(new SolidBrush(color ?? Color.Black), width);
        pen.DashStyle = DashStyle.Solid;
        return pen;
    }

    private static Brush GetBrush(Color? color)
    {
        Brush brush = new SolidBrush(color ?? Color.Black);
        return brush;
    }
}