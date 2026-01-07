using Reqnroll;
namespace Raytracer.Tests.Steps;

public class Item<T>(ScenarioContext ctx)
{
    public T this[string key]
    {
        get => ctx.Get<T>(key);
        set => ctx.Set<T>(value, key);
    }
}

[Binding]
public class StepsBase(ScenarioContext ctx)
{
    public readonly ScenarioContext ctx = ctx;
    public readonly Item<Graphics.Color.T> Color = new(ctx);
    public readonly Item<Math.Tuple.T> Tuple = new(ctx);
    public readonly Item<Graphics.Canvas.T> Canvas = new(ctx);
    public readonly Item<Math.Matrix.T> Matrix = new(ctx);
    public readonly Item<Math.Matrix.M4.T> Transformation = new(ctx);
    public readonly Item<Geometry.Ray.T> Ray = new(ctx);
    public readonly Item<Geometry.Shape.T> Shape = new(ctx);
    public readonly Item<Geometry.Intersection.T[]> XS = new(ctx);
    public readonly Item<Geometry.Intersection.T> I = new(ctx);
    public readonly Item<Graphics.Light.T> Light = new(ctx);
    public readonly Item<Graphics.Material.T> Material = new (ctx);
}
