﻿namespace Recipes.Models.Recipes.ViewModels
{
    using System.Collections.Generic;

    using global::Recipes.Data.Models;
    using global::Recipes.Models.Ingredients.InputModels;
    using global::Recipes.Services.Mapping;

    public class RecipeDetailsViewModel : RecipeBaseViewModel, IMapFrom<Recipe>
    {
        public int Portions { get; set; }

        public int PreparationTime { get; set; }

        public int CookingTime { get; set; }

        public string AuthorUserName { get; set; }

        public bool IsFavourite { get; set; }

        public string CategoryId { get; set; }

        public IEnumerable<BaseIngredientViewModel> Ingredients { get; set; }
    }
}
