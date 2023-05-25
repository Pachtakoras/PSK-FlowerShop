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
                Category Flowers = new Category { Name = "Flowers"};
                Category Boquets = new Category { Name = "Bouquets" };
                Category FlowerBoxes = new Category { Name = "Flower Boxes" };
                Category Gifts = new Category { Name = "Gifts" };
                Category Cards = new Category { Name = "Cards" };


                context.Products.AddRange(
                    new Product
                    {
                        Name = "Roses",
                        Price = 10.98M,
                        Image = "https://shorturl.at/ksHZ4",
                        Description = "A beautiful rose flower",
                        Category = Flowers,
                    },
                    new Product
                    {
                        Name = "Tulips",
                        Price = 5.00M,
                        Image = "https://shorturl.at/rJQV9",
                        Description = "Beautiful tulips",
                        Category = Flowers,
                    },
                    new Product
                    {
                        Name = "White Roses",
                        Price = 9.99M,
                        Image = "https://shorturl.at/kOTY9",
                        Description = "White roses, very beautiful, would buy 10/10",
                        Category = Flowers,
                    },
                    new Product
                    {
                        Name = "Sunflowers",
                        Price = 12.55M,
                        Image = "https://shorturl.at/sPV01",
                        Description = "You can make oil out of that",
                        Category = Flowers,
                    },
                    new Product
                    {
                        Name = "Flowers in a jar",
                        Price = 19.99M,
                        Image = "https://shorturl.at/zCIT4",
                        Description = "Who put this here",
                        Category = Flowers,
                    },
                    new Product
                    {
                        Name = "Rose Boquet",
                        Price = 21.99M,
                        Image = "https://shorturl.at/gHIZ8",
                        Description = "Thats a lot of roses",
                        Category = Boquets,
                    },
                    new Product
                    {
                        Name = "Bespoke Boquet",
                        Price = 20.99M,
                        Image = "https://shorturl.at/bmqQY",
                        Description = "Nice",
                        Category = Boquets,
                    },
                    new Product
                    {
                        Name = "Hand Boquet",
                        Price = 25.99M,
                        Image = "https://shorturl.at/fgsI3",
                        Description = "Gourgeos hands",
                        Category = Boquets,
                    },
                    new Product
                    {
                        Name = "Seasoning Boquet",
                        Price = 30.99M,
                        Image = "https://shorturl.at/sxVYZ",
                        Description = "Some nice seasoning in there",
                        Category = Boquets,
                    },
                    new Product
                    {
                        Name = "Flowers in a white box",
                        Price = 43.99M,
                        Image = "https://shorturl.at/bwzO9",
                        Description = "Some nice seasoning in there",
                        Category = FlowerBoxes,
                    },
                    new Product
                    {
                        Name = "Cute butterfly flower box",
                        Price = 43.99M,
                        Image = "https://shorturl.at/pxKP1",
                        Description = "Some nice seasoning in there",
                        Category = FlowerBoxes,
                    },
                    new Product
                    {
                        Name = "Garden flower box",
                        Price = 49.99M,
                        Image = "https://shorturl.at/vwyFJ",
                        Description = "Nice house",
                        Category = FlowerBoxes,
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
