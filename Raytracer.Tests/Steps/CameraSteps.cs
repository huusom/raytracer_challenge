using Shouldly;
using Reqnroll;
namespace Raytracer.Tests.Steps;

[Binding]
public class CameraSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    private int hsize;
    private int vsize;
    private double fov;

    [Given(@"^hsize ← (\d+)$")]
    public void GivenHSize(int hsize)
    {
        this.hsize = hsize;
    }

    [Given(@"^vsize ← (\d+)$")]
    public void GivenVSize(int vsize)
    {
        this.vsize = vsize;
    }

    [Given(@"^field_of_view ← π/(\d+)$")]
    public void GivenFieldOfView(double fov)
    {
        this.fov = System.Math.PI / fov;
    }

    [Given(@"^(c) ← camera\((\d+), (\d+), π/(\d+)\)$")]
    public void GivenCamera(string key, int hsize, int vsize, double fov)
    {
        GivenHSize(hsize);
        GivenVSize(vsize);
        GivenFieldOfView(fov);
        GivenCamera(key);
    }

    [When(@"^(c) ← camera\(hsize, vsize, field_of_view\)$")]
    public void GivenCamera(string key)
    {
        Camera[key] = Raytracer.Scene.Camera.cameraOf(hsize, vsize, fov);
    }

    [Then(@"^(c)\.hsize = (\d+)$")]
    public void ThenHSizeShouldBe(string key, int expected)
    {
        var c = Camera[key];
        c.hsize.ShouldBe(expected);
    }

    [Then(@"^(c)\.vsize = (\d+)$")]
    public void ThenVSizeShouldBe(string key, int expected)
    {
        var c = Camera[key];
        c.vsize.ShouldBe(expected);
    }

    [Then(@"^(c)\.field_of_view = π/(\d+)$")]
    public void ThenFovShouldBe(string key, double expected)
    {
        var c = Camera[key];
        c.fov.ShouldBe(System.Math.PI / expected);
    }

    [Then(@"^(c)\.transform = identity_matrix$")]
    public void ThenTransformShouldBeIdentityMatrix(string key)
    {
        var c = Camera[key];
        c.transform.ShouldBe(Raytracer.Geometry.Transformation.identity);
    }

    [Then(@"^(c)\.pixel_size = (.*)$")]
    public void ThenPixelSizeShouldBe(string key, double expected)
    {
        var c = Camera[key];
        c.pixel_size.ShouldBe(expected);
    }

    [When(@"^(r) ← ray_for_pixel\((c), (\d+), (\d+)\)$")] 
    public void CreateRayForPixel(string key, string cameraKey, int x, int y)
    {
        var c = Camera[cameraKey];
        var r = Scene.Camera.rayFor(c, x, y);

        Ray[key] = r; 
    }

    [When(@"^(c)\.transform ← (rotation_y.*) \* (translation.*)$")]
    public void WhenSettingTransform(string key, Math.Matrix.M4.T rotation, Math.Matrix.M4.T translation)
    {
        var c = Camera[key];
        c.transform = rotation * translation;
    }

    [Given(@"^(c).transform ← view_transform\(from, to, up\)$")]
    public void GivenViewTransform(string key)
    {
        var t = Geometry.Transformation.viewOf(Tuple["from"], Tuple["to"], Tuple["up"]);
        var c = Camera[key];
        c.transform = t; 
    }

    [When(@"^(image) ← render\((c), (w)\)")]
    public void WhenRenderCamera(string key, string cameraKey, string worldKey)
    {
        var c = Camera[cameraKey];
        var w = World[worldKey];

        var i = Scene.Camera.render(c, w);

        Canvas[key] = i;
    }

}
