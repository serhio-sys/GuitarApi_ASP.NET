using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models;
using ShopAPI.Tools;
using ShopAPI.Wrapper;
using System.Reflection;

namespace ShopAPI.Controllers
{
    [Route("api/guitars/")]
    [ApiController]
    public class GuitarsController : ControllerBase
    {
        private readonly DatabaseContext db;
        private const int MAX_GUITAR_IN_PAGE = 20;
        private readonly ILogger logger;

        public GuitarsController(DatabaseContext db, ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger("GuitarsController Logger");
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetAll(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var validFilter = new PaginationFilter((int)page, MAX_GUITAR_IN_PAGE);
            var pagedData = db.Guitars.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize).ToList();
            var totalRecords = db.Guitars.Count();
            return Ok(new PageResponse<List<Guitar>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalRecords));

        }

        [HttpGet("{id}/")]
        public async Task<IActionResult> GetById(int id)
        {

            var guitar = await db.Guitars.FindAsync(id);
            if (guitar == null)
            {
                return BadRequest(new { Error=$"Not found guitar with id - {id}" });
            }
            return Ok(new Response<Guitar>(guitar));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuitar(Guitar guitar)
        {
            GuitarCategory? guitarCategory = await db.GuitarCategories.FindAsync(guitar.CategoryId);
            if (guitarCategory == null || guitar == null)
            {
                return BadRequest( new { Error = "Can`t save null object!" } );
            }
            await db.Guitars.AddAsync(guitar);
            await db.SaveChangesAsync();
            return Ok(new { Message = "Successfull" });
        }
        [HttpDelete("{id}/delete/")]
        public async Task<IActionResult> DeleteGuitar(int id)
        {
            Guitar? guitar = await db.Guitars.FindAsync(id);
            if(guitar == null) { return BadRequest(new { Error = "Can`t delete null object!" }); }
            db.Guitars.Remove(guitar);
            await db.SaveChangesAsync();
            return Ok(new { Message = $"{guitar.Id} - Has been deleted!" });
        }

        [HttpPatch("{id}/patch/")]
        public async Task<IActionResult> PatchGuitar(int id,Guitar patchDoc)
        {
            if (patchDoc != null)
            {
                Guitar? guitar = await db.Guitars.FindAsync(id);
                if (guitar != null)
                {
                    foreach (PropertyInfo field in patchDoc.GetType().GetProperties())
                    {
                        if (field.PropertyType == typeof(GuitarCategory)) { continue; }
                        if (field.Name == "Id") { continue; } 
                        try
                        {
                            int? value = (int?)field.GetValue(patchDoc);
                            if (value == null || value == 0)
                            {
                                continue;
                            }
                            else
                            {
                                guitar.GetType()?.GetProperty(field.Name)?.SetValue(guitar, field.GetValue(patchDoc));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex.Message);
                            if (!string.IsNullOrEmpty((string?)field.GetValue(patchDoc)))
                            {
                                Console.WriteLine(field.GetValue(patchDoc));
                                guitar.GetType()?.GetProperty(field.Name)?.SetValue(guitar, field.GetValue(patchDoc));
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    return Ok( new { UpdatedGuitar = guitar });
                }
                
            }
            return BadRequest(new { Message = "Patching Error!" });
        }
    }
}
