using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Workflow.Abstractions.Database;
using Workflow.Application.Controllers.Administration.Triggers.Dto;
using Workflow.Application.Controllers.Dto;
using Workflow.Application.Controllers.Triggers.Dto;
using Workflow.Application.Extensions;
using Workflow.Application.Utils;
using Workflow.Core.Models.Triggers;

namespace Workflow.Application.Controllers.Administration
{
    public class TriggersController : AdministrationBaseController
    {
        private readonly IRepository<Trigger> _triggerRepository;


        public TriggersController(IRepository<Trigger> triggerRepository)
        {
            _triggerRepository = triggerRepository;
        }

        [HttpGet]
        public async Task<PagedResultDto<TriggerDto>> GetAll(FullResultInputDto input)
        {
            var triggers = _triggerRepository.Queryable();

            return await triggers.ToPagedResult<Trigger, TriggerDto>(input, p => p.Name.Contains(input.Filter));
        }

        [HttpPost]
        public async Task Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidInputException(@"The name of the trigger is empty.");
            }

            var trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Name == name);

            if (trigger != null)
            {
                throw new InvalidInputException(@"A trigger with this name already exists.");
            }

            trigger = new Trigger
            {
                Name = name
            };

            await _triggerRepository.InsertAsync(trigger);
        }

        //[HttpPut]
        //public async Task Update(TriggerDto input)
        //{
        //    if (string.IsNullOrWhiteSpace(input.Name))
        //    {
        //        throw new InvalidInputException(@"The name of the trigger is empty.");
        //    }

        //    var trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Name == input.Name);

        //    if (trigger != null)
        //    {
        //        throw new InvalidInputException(@"A trigger with this name already exists.");
        //    }

        //    trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Id == input.Id);

        //    if (trigger == null)
        //    {
        //        throw new InvalidInputException(@"Trigger was not found.");
        //    }

        //    trigger.Name = input.Name;

        //    await _triggerRepository.UpdateAsync(trigger);
        //}

        [HttpDelete]
        public  async Task Delete(Guid id)
        {
            var trigger = await _triggerRepository.FirstOrDefaultAsync(p => p.Id == id);

            if (trigger == null)
            {
                throw new InvalidInputException(@"Trigger was not found.");
            }

            await _triggerRepository.RemoveAsync(trigger);
        }
    }
}
