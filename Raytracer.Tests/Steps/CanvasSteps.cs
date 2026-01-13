using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Reqnroll;
using Shouldly;
using Canvas = Raytracer.Graphics.Canvas.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class CanvasSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [StepArgumentTransformation(@"canvas\((.*), (.*)\)")]
    public static Canvas ToCanvas(int w, int h) => Graphics.Canvas.create(w, h);

    private IEnumerable<string> ppm;

    [Given(@"^(c) ← (canvas.*)$")]
    public void CreateCanvas(string k, Canvas canvas)
    {
        Canvas[k] = canvas;
    }

    [Then(@"^c\.width = (.*)$")]
    public void WidthShouldBe(int expected)
    {
        var c = Canvas["c"];
        c.w.ShouldBe(expected);
    }

    [Then(@"^c\.height = (.*)$")]
    public void HeightShouldBe(int expected)
    {
        var c = Canvas["c"];
        c.h.ShouldBe(expected);
    }

    [Then(@"^every pixel of (c) is (color.*)$")]
    public void EveryPixelShouldBe(string k, Graphics.Color.T expected)
    {
        var c = Canvas[k];
        c.p.ShouldAllBe(a => a.Equals(expected));
    }

    [When(@"^write_pixel\((c), (\d+), (\d+), (.*)\)$")]
    public void WritePixel(string k1, int x, int y, string k2)
    {
        var c = Canvas[k1];
        var red = Color[k2];

        c[x, y] = red;
    }

    [Then(@"^pixel_at\((image), (\d+), (\d+)\) = (color.*)$")]
    public void PixelAtShouldBe(string canvasKey, int x, int y, Raytracer.Graphics.Color.T color)
    {
        var canvas = Canvas[canvasKey];

        canvas[x, y].ShouldBe(color);
    }

    [Then(@"^pixel_at\((c), (2), (3)\) = (red)$")]
    public void PixelAtShouldBe(string canvasKey, int x, int y, string colorKey)
    {
        var canvas = Canvas[canvasKey];
        var color = Color[colorKey];

        canvas[x, y].ShouldBe(color);
    }

    [When(@"^ppm ← canvas_to_ppm\((c)\)$")]
    public void CanvasToPpm(string canvasKey)
    {
        var canvas = Canvas[canvasKey];
        this.ppm = Graphics.Canvas.portablePixmapOf(canvas);
    }

    [Then(@"^lines (\d+)-(\d+) of ppm are$")]
    public void LinesOfPpmShouldBe(int start, int end, string expected)
    {
        start -= 1;
        var actual = string.Join(Environment.NewLine, ppm.Skip(start).Take(end - start));

        actual.ShouldBe(expected);
    }

    [Then(@"^ppm ends with a newline character$")]
    public void PpmEndsWithANewlineCharacter()
    {
        ppm.Last().ShouldBe(Environment.NewLine);
    }

    [When(@"^every pixel of (c) is set to (color.*)$")]
    public void EveryPixelIs(string canvasKey, Graphics.Color.T color)
    {
        var c = Canvas[canvasKey];

        Array.Fill(c.p, color);
    }
}
