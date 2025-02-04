﻿using kurs11135.Models;
using kurs11135.okna;
using kurs11135.Tools;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace kurs11135.VM
{
    public class AddProdVM : BaseVM
    {
        public CommandVM AddProduct { get; set; }
        public CommandVM AddImage { get; set; }
        public CommandVM SaveButton { get; set; }
        public CommandVM EditProduct { get; set; }
        public Product SelectedItem { get; set; }
        public CommandVM DelProduct { get; set; }


        public byte[]? Image { get => image; set { image = value; Signal(); } }
        public string NameProduct { get; set; }
        public decimal CostProduct { get; set; }
        public string ShortName { get; set; }

        public AddProdVM()
        {
            SaveButton = new CommandVM(async () =>
            {
                var json3 = await Api.Post("ProductImages", new ProductImage { Image = Image }, "get");
                var image = Api.Deserialize<ProductImage>(json3);


                var json1 = await Api.Post("Products", new Product
                {
                    CategoryId = ListProductCategory.Id,
                    ProductName = NameProduct,
                    ProductCost = CostProduct,
                    ShortDescription = ShortName,
                    ImageId = image.Id
                }, "SaveProduct");
                Product result1 = Api.Deserialize<Product>(json1);
                MessageBox.Show("молодец");
                if (result1 != null)
                {
                    MessageBox.Show("Товар успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Не удалось добавить товар");
                }
            });

            Task.Run(async () =>
            {
                await che();
            });

            AddProduct = new CommandVM(() =>
            {
                new AddProduct().Show();

            });
            AddImage = new CommandVM(async () =>
            {
                OpenFileDialog ofd = new();
                if (ofd.ShowDialog() == true)
                {
                    var bytes = File.ReadAllBytes(ofd.FileName);
                    Image = bytes;
                }
            });
            DelProduct = new CommandVM(async () =>
            {
                var json1 = await Api.Post("Products", SelectedItem.Id, "delete");
                
                    await che();
                });
            

            EditProduct = new CommandVM(async () =>
            {
                product = SelectedItem;
                new EditProduct(product).Show();
            });

        }
        public List<ProductCategory> productCategories { get; set; }
        private ProductCategory listProductCategory;
        private byte[]? image;
        public ProductCategory ListProductCategory
        {
            get => listProductCategory;
            set
            {
                listProductCategory = value;
                Signal();
            }
        }

        public async Task che()
        {
            string json1 = await Api.Post("Products", null, "get");
            var result1 = Api.Deserialize<List<Product>>(json1);
            products = result1;
            Signal(nameof(products));

            string json = await Api.Post("ProductCategories", null, "get");
            var result = Api.Deserialize<List<ProductCategory>>(json);
            productCategories = result;
            Signal(nameof(productCategories));




        }
        public List<Product> products { get; set; }
        public Product product { get; set; }




    }
}
