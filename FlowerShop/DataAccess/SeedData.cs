// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.DataAccess;
using FlowerShop.Models;

namespace PSI_Food_waste.Data
{
    public static class SeedData
    {
        public static void Initialize(FlowerContext context)
        {
            if (!context.Products.Any())
            {
                Category focals = new Category { Name = "Focals"};
                Category roses = new Category { Name = "Roses" };


                context.Products.AddRange(
                    new Product
                    {
                        Name = "Roses",
                        Price = 10.98M,
                        Image = "https://shorturl.at/ksHZ4",
                        Description = "A beautiful rose flower",
                        Category = roses,
                    },
                    new Product
                    {
                        Name = "Tulips",
                        Price = 9.55M,
                        Image = "https://shorturl.at/rJQV9",
                        Description = "Beautiful tulips",
                        Category = focals,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
