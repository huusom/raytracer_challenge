using Raytracer.Graphics;
using Raytracer.Math;
using Raytracer.Geometry;
using Reqnroll;
using Shouldly;
namespace Raytracer.Tests.Steps;

public class Item<T>(ScenarioContext ctx)
{
    public T this[string key]
    {
        get => ctx.Get<T>(key);
        set => ctx.Set(value, key);
    }
}

[Binding]
public class StepsBase(ScenarioContext ctx)
{
    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        ShouldlyConfiguration.DefaultFloatingPointTolerance = Library.epsilon;
    }

    public readonly ScenarioContext ctx = ctx;
    public readonly Item<Color.T> Color = new(ctx);
    public readonly Item<Tuple.T> Tuple = new(ctx);
    public readonly Item<Canvas.T> Canvas = new(ctx);
    public readonly Item<Matrix.T> Matrix = new(ctx);
    public readonly Item<Matrix.M4.T> Transformation = new(ctx);
    public readonly Item<Ray.T> Ray = new(ctx);
    public readonly Item<Shape.T> Shape = new(ctx);
    public readonly Item<Intersection.T[]> XS = new(ctx);
    public readonly Item<Intersection.T> I = new(ctx);
    public readonly Item<Light.T> Light = new(ctx);
    public readonly Item<Material.T> Material = new(ctx);
    public readonly Item<Scene.World.T> World = new(ctx);

    public readonly Item<Intersection.Comps.T> Comps = new (ctx);

    public readonly Item<Scene.Camera.T> Camera = new (ctx);
}


public static class DefaultsBuilder
{
    public static Scene.World.T World(Shape.T[] objects = null, Light.T[] lights = null) =>
        Scene.World.create(
            objects ?? [Sphere(material: Material(Color.create(0.8, 1, 0.6), diffuse: 0.7, specular: 0.2)),
                        Sphere(transform: Transformation.scaling(0.5, 0.5, 0.5))],
            lights ?? [PointLight()]);

    public static Shape.T Sphere(Matrix.M4.T transform = null, Material.T material = null) => Shape.sphere(transform ?? Transformation.identity, material ?? Material());

    public static Material.T Material(Color.T? color = null, double ambient = 0.1, double diffuse = 0.9, double specular = 0.9, double shininess = 200) => Graphics.Material.create(color ?? Color.white, ambient, diffuse, specular, shininess);

    public static Light.T PointLight(Tuple.T? position = null, Color.T? intensity = null) => Light.pointLight(position ?? Tuple.point(-10, 10, -10), intensity ?? Color.white);
}