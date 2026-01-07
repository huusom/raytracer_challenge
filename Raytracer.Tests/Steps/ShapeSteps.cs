using Reqnroll;
using Shouldly;
using Shape = Raytracer.Geometry.Shape.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class ShapeSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [StepArgumentTransformation(@"sphere\(\)")]
    public static Shape ToSphere() => Geometry.Shape.sphere(System.DateTime.Now.Microsecond, Math.Matrix.M4.identity, MaterialSteps.ToMaterial());

    [Given(@"^(s) ← (sphere.*)$")]
    public void GivenShape(string shapeKey, Shape shape)
    {
        Shape[shapeKey] = shape;
    }

    [When(@"^(xs) ← intersect\((s), (r)\)$")]
    public void WhenCalculatingIntersection(string intersectKey, string shapeKey, string rayKey)
    {
        var s = Shape[shapeKey];
        var r = Ray[rayKey];
        var xs = Geometry.Intersection.intersect(s, r);

        XS[intersectKey] = xs;
    }

    [Then(@"^(xs).count = (\d)$")]
    public void ThenIntersectionCountShouldBe(string intersectKey, int expected)
    {
        var xs = XS[intersectKey];
        var actual = xs.Length;

        actual.ShouldBe(expected);
    }

    [Then(@"^(xs)\[(\d)\] = (.*)")]
    [Then(@"^(xs)\[(\d)\]\.t = (.*)")]
    public void ThenIntersctionValueShouldBe(string intersectKey, int index, double expected)
    {
        var xs = XS[intersectKey];
        var actual = xs[index];
        actual.t.ShouldBe(expected, Library.epsilon);
    }

    [Then(@"^(xs)\[(\d)\].object = (s)$")]
    public void ThenIntersectionObjectShouldBe(string intersectKey, int index, string shapeKey)
    {
        var xs = XS[intersectKey];
        var expected = Shape[shapeKey];
        var actual = xs[index];

        actual.@object.ShouldBe(expected);
    }

    [Then(@"^(s)\.transform = identity_matrix$")]
    public void ThenTransformShouldBeIdentityMatrix(string key)
    {
        var s = Shape[key];
        var actual = Geometry.Shape.getTransform(s);
        actual.ShouldBe(Math.Matrix.M4.identity);
    }

    // When set_transform(s, t)
    [When(@"^set_transform\((s), (t)\)$")]
    public void WhenSetTransform(string shapeKey, string transformKey)
    {
        var s = Shape[shapeKey];
        var t = Transformation[transformKey];

        Geometry.Shape.setTransfrom(s, t);
    }

    // Then s.transform = t
    [Then(@"^(s)\.transform = (t)$")]
    public void ThenTransformShouldBe(string shapeKey, string transformKey)
    {
        var s = Shape[shapeKey];
        var actual = Geometry.Shape.getTransform(s);
        var expected = Transformation[transformKey];

        actual.ShouldBe(expected);
    }

    // When set_transform(s, scaling(2, 2, 2))
    [When(@"^set_transform\((s), (scaling.*)\)$")]
    [When(@"^set_transform\((s), (translation.*)\)$")]
    [Given(@"^set_transform\((s), (translation.*)\)$")]
    public void WhenSetTransform(string shapeKey, Math.Matrix.M4.T transform)
    {
        var s = Shape[shapeKey];
        Geometry.Shape.setTransfrom(s, transform);
    }

    [Given(@"^set_transform\((s), (m)\)$")    ]
    public void GivenSetTransform(string shapeKey, string transformKey )
    {
        var s = Shape[shapeKey];
        var t = Transformation[transformKey];
        Geometry.Shape.setTransfrom(s, t);
    }

    [When(@"^(n) ← normal_at\((s), (point.*)\)$")]
    public void WhenSettingNormal(string key, string shapeKey, Math.Tuple.T point)
    {
        var s = Shape[shapeKey];
        var n = Geometry.Shape.normalAt(s, point);
        Tuple[key] = n;
    }
}