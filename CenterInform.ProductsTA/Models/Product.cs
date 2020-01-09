using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Prism.Mvvm;

namespace CenterInform.ProductsTA.Models
{
    [Table("ProductInfo")]
    [Serializable]
    public class Product : BindableBase, IDataErrorInfo
    {
        string _code;
        string _name;
        string _description;
        float _quantity;
        Dictionary<string, string> errorCol = new Dictionary<string, string>();

        public Product()
        {
            Code = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Quantity = 0f;
        }

        public Product(string code, string name, string description, float quantity)
        {
            Code = code;
            Name = name;
            Description = description;
            Quantity = quantity;
        }

        [Key]
        [MaxLength(10)]
        [XmlElement("Code")]
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value);}
        }

        [Required]
        [StringLength(255)]
        [XmlElement("Name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [StringLength(2000)]
        [XmlElement("Description")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        [Required]
        [XmlElement("Quantity")]
        public float Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }

        #region DataError
        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case "Code":
                        {
                            string pattern = "^\\d+$";
                            if (Code.Length == 0)
                            {
                                error = "Строка является обязательной к заполнению";
                            }
                            else if (!Regex.IsMatch(Code, pattern))
                            {
                                error = "Код должен состоять только из цифр";
                            }
                            else if (Code.Length > 10)
                            {
                                error = "Длина строки должна быть до 10 цифр";
                            }

                            break;
                        }
                    case "Name":
                        {
                            if (Name.Length == 0)
                            {
                                error = "Строка является обязательной к заполнению";
                            }
                            else if (Name.Length > 255)
                            {
                                error = "Длина строки должна быть не более 255 символов";
                            }
                            break;
                        }

                    case "Description":
                        {
                            if (Name.Length > 255)
                            {
                                error = "Длина строки должна быть не более 2000 символов";
                            }
                            break;
                        }
                    case "Quantity":
                        {
                            if (Quantity <= 0)
                            {
                                error = "Значение не должно быть меньше или равно 0";
                            }
                            break;
                        }
                }

                if (error != null && !errorCol.ContainsKey(columnName))
                {
                    errorCol.Add(columnName, error);
                }
                else if (error == null && errorCol.ContainsKey(columnName))
                {
                    errorCol.Remove(columnName);
                }

                return error;
            }
        }

        public string Error
        {
            get
            {
                if (errorCol.Count == 0) return null;

                return string.Join(Environment.NewLine, errorCol.Values);
            }
        }

        #endregion DataError
    }


    [XmlRoot("Products")]
    public class Products
    {
        public Products()
        {
            productsList = new List<Product>();
        }

        public Products(List<Product> products)
        {
            productsList = new List<Product>(products);
        }

        [XmlElement("Product")]
        public List<Product> productsList { get; set; }
    }
}
