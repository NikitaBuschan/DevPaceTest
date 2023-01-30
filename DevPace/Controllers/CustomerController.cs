using DevPace.DB;
using DevPace.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DevPace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly DevPaceDbContext _context;

        public CustomerController(DevPaceDbContext context)
        {
            _context = context;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(customer => customer.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET api/<CustomerController>/name
        [HttpGet("{name}")]
        public async Task<ActionResult<Customer>> GetName(string name)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(customer => customer.Name == name);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Customer>>> Post(Customer customer)
        {
            var _customer = await _context.FindAsync(typeof(Customer), customer);

            if (_customer != null)
            {
                return StatusCode(409, $"Customer '{customer.Name}' already exists.");
            }

            _context.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(await _context.Customers.ToListAsync());
        }

        // PUT api/<CustomerController>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Customer customer)
        {
            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // DELETE api/<CustomerController>
        [HttpDelete]
        public async Task<ActionResult> Delete(Customer customer)
        {
            try
            {
                _context.Remove(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteId(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(customer => customer.Id == id);

            if (customer == null)
            {
                return NotFound($"Customer with ID: {id} is not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/<CustomerController>/name
        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteName(string name)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(customer => customer.Name == name);

            if (customer == null)
            {
                return NotFound($"Customer with Name: {name} is not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
