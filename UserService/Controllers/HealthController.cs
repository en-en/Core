using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/Health")]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult Get() => Ok("ok");
}