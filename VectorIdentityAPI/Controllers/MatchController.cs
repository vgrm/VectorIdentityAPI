﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VectorIdentityAPI.Database;

namespace VectorIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MatchController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Match
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatch()
        {
            return await _context.Match.ToListAsync();
        }

        // GET: api/Match/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _context.Match.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        // PUT: api/Match/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Match match)
        {
            if (id != match.Id)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Match
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {
            _context.Match.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.Id }, match);
        }

        // DELETE: api/Match/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Match.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Match.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatchExists(int id)
        {
            return _context.Match.Any(e => e.Id == id);
        }
    }
}
