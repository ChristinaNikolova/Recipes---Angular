﻿namespace Recipes.Data.Data.Seeding.CustomSeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Recipes.Data.Common;
    using Recipes.Data.Data.Seeding.Dtos;
    using Recipes.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Categories.Any())
            {
                var categoriesData = JsonConvert
                    .DeserializeObject<List<CategoryDto>>(File.ReadAllText(Constants.CategorySeederPath))
                    .ToList();

                List<Category> categories = new List<Category>();

                foreach (var currentCategoryData in categoriesData)
                {
                    var category = new Category()
                    {
                        Name = currentCategoryData.Name,
                    };

                    categories.Add(category);
                }

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
