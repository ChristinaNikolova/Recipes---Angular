﻿namespace Recipes.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Recipes.Areas.Admin.Models.Ingredients.InputModels;
    using Recipes.Areas.Admin.Models.Ingredients.ViewModels;
    using Recipes.Common;
    using Recipes.Controllers;
    using Recipes.Models.Common;
    using Recipes.Models.Ingredients.InputModels;
    using Recipes.Services.Data.Ingredients;

    [Route("api/admin/[controller]/[action]")]
    public class IngredientsController : ApiController
    {
        private readonly IIngredientsService ingredientsService;

        public IngredientsController(IIngredientsService ingredientsService)
        {
            this.ingredientsService = ingredientsService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Create([FromBody] IngredientCreateAdminInputModel input)
        {
            if (this.User.IsInRole(GlobalConstants.AdminName))
            {
                var isIngredientAlreadyExisting = await this.ingredientsService.IsAlreadyAddedAsync(input.Name);

                if (isIngredientAlreadyExisting)
                {
                    return this.BadRequest(new BadRequestViewModel
                    {
                        Message = Messages.AlreadyExistsIngredient,
                    });
                }

                try
                {
                    await this.ingredientsService.CreateAsync(input.Name);

                    return this.Ok(new
                    {
                        Message = Messages.SuccessfullyAdded,
                    });
                }
                catch (Exception)
                {
                    return this.BadRequest(new BadRequestViewModel
                    {
                        Message = Messages.UnknownError,
                    });
                }
            }

            return this.Unauthorized();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<IngredientAdminViewModel>>> All()
        {
            if (this.User.IsInRole(GlobalConstants.AdminName))
            {
                try
                {
                    var ingredients = await this.ingredientsService.GetAllAsync<IngredientAdminViewModel>();

                    return new List<IngredientAdminViewModel>(ingredients);
                }
                catch (Exception)
                {
                    return this.BadRequest(new BadRequestViewModel
                    {
                        Message = Messages.UnknownError,
                    });
                }
            }

            return this.Unauthorized();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IngredientUpdateInputModel>> IngredientForUpdate(string id)
        {
            if (this.User.IsInRole(GlobalConstants.AdminName))
            {
                try
                {
                    var ingredient = await this.ingredientsService.GetDetailsAsync<IngredientUpdateInputModel>(id);

                    return ingredient;
                }
                catch (Exception)
                {
                    return this.BadRequest(new BadRequestViewModel
                    {
                        Message = Messages.UnknownError,
                    });
                }
            }

            return this.Unauthorized();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(string id)
        {
            if (this.User.IsInRole(GlobalConstants.AdminName))
            {
                try
                {
                    await this.ingredientsService.DeleteAsync(id);

                    return this.Ok(new
                    {
                        Message = Messages.SuccessfullyDeleted,
                    });
                }
                catch (Exception)
                {
                    return this.BadRequest(new BadRequestViewModel
                    {
                        Message = Messages.UnknownError,
                    });
                }
            }

            return this.Unauthorized();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(IngredientUpdateInputModel input)
        {
            if (this.User.IsInRole(GlobalConstants.AdminName))
            {
                try
                {
                    await this.ingredientsService.UpdateAsync(input.Id, input.Name);

                    return this.Ok(new
                    {
                        Message = Messages.SuccessfullyUpdated,
                    });
                }
                catch (Exception)
                {
                    return this.BadRequest(new BadRequestViewModel
                    {
                        Message = Messages.UnknownError,
                    });
                }
            }

            return this.Unauthorized();
        }
    }
}
