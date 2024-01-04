﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

public static class DrawUtil
{
    public static void DrawLine(Bitmap bitmap, Point startPoint, Point endPoint, Color? color = null, float width = 1)
    {
        using Pen pen = GetPen(color, width);
        using Graphics graphics = GetGraphics(bitmap);
        graphics.DrawLine(pen, startPoint, endPoint);
    }

    public static void DrawRectangle(Bitmap bitmap, Rectangle rectangle, Color? color = null, float width = 1)
    {
        using Pen pen = GetPen(color, width);
        using Graphics graphics = GetGraphics(bitmap);
        graphics.DrawRectangle(pen, rectangle);
    }

    public static void FillRectangle(Bitmap bitmap, Rectangle rectangle, Color? color = null)
    {
        using Brush brush = GetBrush(color);
        using Graphics graphics = GetGraphics(bitmap);
        graphics.FillRectangle(brush, rectangle);
    }

    public static void DrawCircle(Bitmap bitmap, Point origin, int radius, Color? color = null, float width = 1)
    {
        using Pen pen = GetPen(color, width);
        using Graphics graphics = GetGraphics(bitmap);
        Rectangle rectangle = new Rectangle(origin.X - radius, origin.Y - radius, 2 * radius, 2 * radius);
        graphics.DrawEllipse(pen, rectangle);
    }

    public static void FillCircle(Bitmap bitmap, Point origin, int radius, Color? color = null)
    {
        using Brush brush = GetBrush(color);
        using Graphics graphics = GetGraphics(bitmap);
        Rectangle rectangle = new Rectangle(origin.X - radius, origin.Y - radius, 2 * radius, 2 * radius);
        graphics.FillEllipse(brush, rectangle);
    }

    public static void DrawPoint(Bitmap bitmap, Point origin, Color? color = null, int width = 1)
    {
        FillCircle(bitmap, origin, width, color);
    }

    public static void DrawString(Bitmap bitmap, string text, Rectangle rectangle, Font font, Color? color = null, TextFormatFlags textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter)
    {
        using Graphics graphics = GetGraphics(bitmap);
        TextRenderer.DrawText(graphics, text, font, rectangle, color ?? Color.Black, textFormatFlags);
    }

    public static void DrawBezier(Bitmap bitmap, Point pt1, Point pt2, Point pt3, Point pt4, Color? color = null, float width = 1)
    {
        using Pen pen = GetPen(color, width);
        using Graphics graphics = GetGraphics(bitmap);
        graphics.DrawBezier(pen, pt1, pt2, pt3, pt4);
    }

    private static Graphics GetGraphics(Bitmap bitmap)
    {
        Graphics graphics = Graphics.FromImage(bitmap);
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