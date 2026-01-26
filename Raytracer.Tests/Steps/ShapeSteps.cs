using System.Linq;
using Reqnroll;
using Shouldly;
using Raytracer.Geometry;
using Raytracer.Math;

namespace Raytracer.Tests.Steps;

[Binding]
public class ShapeSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    private Ray.T saved_ray;

    [StepArgumentTransformation(@"sphere\(\)")]
    public static Shape.T CreateSphere() => DefaultsBuilder.Sphere();

    [StepArgumentTransformation(@"plane\(\)")]
    public static Shape.T CreatePlane() => DefaultsBuilder.Plane();

    [StepArgumentTransformation(@"test_shape\(\)")]
    public Shape.T CreateTest() => DefaultsBuilder.TestShape(SaveRay);

    private void SaveRay(Ray.T t)
    {
        this.saved_ray = t;
    }

    [Given(@"^(s) ← (test_shape\(\))$")]
    [Given(@"^(p) ← (plane\(\))$")]
    [Given(@"^(s|s1|shape|object) ← (sphere\(\))$")]
    public void GivenShape(string shapeKey, Shape.T shape)
    {
        Shape[shapeKey] = shape;
    }

    [Then(@"^(s)\.transform = identity_matrix$")]
    public void ThenTransformShouldBeIdentityMatrix(string key)
    {
        var s = Shape[key];
        var actual = s.transform;
        actual.ShouldBe(Math.Transformation.identity);
    }

    [Given(@"^set_transform\((s), (m)\)$")]
    [When(@"^set_transform\((s), (t|m)\)$")]
    public void WhenSetTransform(string shapeKey, string transformKey)
    {
        var s = Shape[shapeKey];
        var t = Transformation[transformKey];
        s.transform = t;
    }

    // Then s.transform = t
    [Then(@"^(s)\.transform = (t)$")]
    public void ThenTransformShouldBe(string shapeKey, string transformKey)
    {
        var s = Shape[shapeKey];
        var actual = s.transform;
        var expected = Transformation[transformKey];

        actual.ShouldBe(expected);
    }

    // When set_transform(s, scaling(2, 2, 2))
    [When(@"^set_transform\((s), (scaling.*)\)$")]
    [When(@"^set_transform\((s), (translation.*)\)$")]
    [Given(@"^set_transform\((s), (translation.*)\)$")]
    [Given(@"^set_transform\((object|shape), (scaling.*)\)$")]
    public void WhenSetTransform(string shapeKey, Transformation.T transform)
    {
        var s = Shape[shapeKey];
        s.transform = transform;
    }

    [Then(@"^(s)\.transform = (translation.*)$")]
    public void ThenTransformShouldBe(string shapeKey, Transformation.T expected)
    {
        var s = Shape[shapeKey];
        var actual = s.transform;

        actual.ShouldBe(expected);
    }

    [When(@"^(n) ← normal_at\((s), (point.*)\)$")]
    public void WhenSettingNormal(string key, string shapeKey, Math.Tuple.T point)
    {
        var s = Shape[shapeKey];
        var n = Geometry.Shape.normalFrom(s, point);
        Tuple[key] = n;
    }

    [Given(@"^(s\d|shape) ← (sphere\(\)) with:$")]
    public void GivenSphereWith(string key, Shape.T shape, DataTable dataTable)
    {
        dataTable
            .Rows.Select(r => string.Join(" ", r.Values))
            .Append(string.Join(" ", dataTable.Header)).
            Aggregate(shape, Raytracer.IO.Parser.updateShapeFrom);

        Shape[key] = shape;
    }

    [Given(@"^(outer|inner)\.material\.ambient ← (.*)$")]
    public void GivenMaterialAmbient(string key, double value)
    {
        var shape = Shape[key];
        shape.material.ambient = value;
    }

    [Then(@"(c) = (inner)\.material\.color")]
    public void ThenMaterialColorShouldBe(string key, string expectedKey)
    {
        var shape = Shape[expectedKey];
        var expected = shape.material;
        var actual = Color[key];

        actual.ShouldBe(expected.color);

    }

    [When(@"^(m) ← (s).material")]
    public void WhenGettingMaterial(string key, string shapeKey)
    {
        var s = Shape[shapeKey];
        Material[key] = s.material;
    }

    //   Then s.material = m

    [When(@"^(s)\.material ← (m)")]
    public void WhenSettingMaterialTo(string key, string materialKey)
    {
        var s = Shape[key];
        var m = Material[materialKey];

        s.material = m;
    }

    [Then(@"^(s)\.material = (m)$")]
    public void ThenMaterialShouldBe(string key, string materialKey)
    {
        var s = Shape[key];
        var expected = Material[materialKey];
        var actual = s.material;
        actual.ShouldBe(expected);
    }

    [Then(@"^s\.saved_ray\.origin = (point.*)$")]
    public void ThenSavedRayOriginShouldBe(Tuple.T expected)
    {
        saved_ray.origin.ShouldBe(expected);
    }

    [Then(@"^s\.saved_ray\.direction = (vector.*)$")]
    public void ThenSavedRayDirectionShouldBe(Tuple.T expected)
    {
        saved_ray.direction.ShouldBe(expected);
    }

    [When(@"^(n\d) ← local_normal_at\((p), (point.*)\)$")]
    public void WhenGettingLocalNormal(string key, string planeKey, Tuple.T point)
    {
        var p = Shape[planeKey];
        Tuple[key] = p.normal.Invoke(point);
    }

    [When(@"^(xs) ← local_intersect\((p), (r)\)$")]
    public void WhenLocalIntersects(string key, string planeKey, string rayKey)
    {
        var p = Shape[planeKey];
        var r = Ray[rayKey];
        XS[key] = [.. Intersection.intersectionsOf(p, r).OrderBy( i => i.t)];
    }
}

