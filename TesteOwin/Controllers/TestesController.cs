using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TesteOwin.Models;

namespace TesteOwin.Controllers
{
    [Authorize]
    public class TestesController : ApiController
    {
        private TesteOwinContext db = new TesteOwinContext();

        // GET: api/Testes
        public IQueryable<Teste> GetTestes()
        {
            return db.Testes;
        }

        // GET: api/Testes/5
        [ResponseType(typeof(Teste))]
        public IHttpActionResult GetTeste(Guid id)
        {
            Teste teste = db.Testes.Find(id);
            if (teste == null)
            {
                return NotFound();
            }

            return Ok(teste);
        }

        // PUT: api/Testes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeste(Guid id, Teste teste)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teste.Id)
            {
                return BadRequest();
            }

            db.Entry(teste).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TesteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Testes
        [ResponseType(typeof(Teste))]
        public IHttpActionResult PostTeste(Teste teste)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Testes.Add(teste);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TesteExists(teste.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = teste.Id }, teste);
        }

        // DELETE: api/Testes/5
        [ResponseType(typeof(Teste))]
        public IHttpActionResult DeleteTeste(Guid id)
        {
            Teste teste = db.Testes.Find(id);
            if (teste == null)
            {
                return NotFound();
            }

            db.Testes.Remove(teste);
            db.SaveChanges();

            return Ok(teste);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TesteExists(Guid id)
        {
            return db.Testes.Count(e => e.Id == id) > 0;
        }
    }
}