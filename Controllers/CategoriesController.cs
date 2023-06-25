using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models;
using ShopAPI.Wrapper;

namespace ShopAPI.Controllers
{
    [Route("api/categories/")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DatabaseContext db;

        public CategoriesController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<GuitarCategory> categories = db.GuitarCategories.ToList();
            return Ok(new Response<List<GuitarCategory>>(categories));
        }

        [HttpGet("{id}/")]
        public async Task<IActionResult> Get(int id)
        {
            GuitarCategory? guitarCategory = await db.GuitarCategories.FindAsync(id);
            if (guitarCategory == null)
            {
                return BadRequest(new { Error = "Not found category!" });
            }
            return Ok(new Response<GuitarCategory>(guitarCategory));
        }
    }
}
