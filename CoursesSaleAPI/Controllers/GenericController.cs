using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Entity;
using Domain.Contracts.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    public class GenericController<T, TRequest, TResponse> : ControllerBase where T : class, IEntity where TRequest : class where TResponse : class
    {
        protected readonly Dictionary<string, string> errorDescriptions = ConstantsErrors.ERROR_DESCRIPTIONS;
        protected readonly IServiceGeneric<T> _service;
        protected readonly IMapper _mapper;
        public GenericController(IServiceGeneric<T> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TResponse>>> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<TResponse>>(await _service.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TResponse>> GetAsync(Guid id)
        {
            return Ok(_mapper.Map<TResponse>(await _service.GetAsync(id)));
        }

        [HttpPost]
        public virtual async Task<ActionResult<TResponse>> PostAsync([FromBody] TRequest entityRequest)
        {
            return Created("", _mapper.Map<TResponse>(await _service.AddAsync(_mapper.Map<T>(entityRequest))));
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TResponse>> PutAsync(Guid id, [FromBody] TRequest entityRequest)
        {
            return Ok(_mapper.Map<TResponse>(await _service.UpdateAsync(id, _mapper.Map<T>(entityRequest))));
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
