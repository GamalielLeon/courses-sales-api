using Domain.Contracts.Entity;
using Domain.Contracts.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    public class GenericController<T, TRequest, TResponse> : ControllerBase where T : class, IEntity where TRequest : class where TResponse : class
    {
        protected readonly IServiceGeneric<T> _service;
        public GenericController(IServiceGeneric<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<ICollection<TResponse>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TResponse>> GetAsync(Guid id)
        {
            return Ok(await _service.GetAsync(id));
        }

        [HttpPost]
        public virtual async Task<ActionResult<TResponse>> PostAsync([FromBody] TRequest entityRequest)
        {
            return Created("", await _service.AddAsync(entityRequest));
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TResponse>> PutAsync(Guid id, [FromBody] TRequest entityRequest)
        {
            return Ok(await _service.UpdateAsync(id, entityRequest));
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
