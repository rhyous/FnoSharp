namespace FnoSharp.Builder
{
    public class EntitledProductBuilder : BuilderBase<entitledProductDataType>
    {
        #region Constructors

        public EntitledProductBuilder()
        {
        }

        public EntitledProductBuilder(string productName, string productVersion, int quantity)
        {
            SetProduct(productName, productVersion, quantity);
        }

        #endregion

        #region Properties

        public string ProductName
        {
            get { return Product.primaryKeys.name; }
            set { Product.primaryKeys.name = value; }
        }
        public string Version
        {
            get { return Product.primaryKeys.version; }
            set { Product.primaryKeys.version = value; }
        }

        public int Quantity
        {
            get { return int.Parse(Object.quantity); }
            set { Object.quantity = value.ToString(); }
        }

        private productIdentifierType Product
        {
            get { return Object.product ?? (Object.product = new productIdentifierType { primaryKeys = new productPKType() }); }
        }

        #endregion

        public void SetProduct(string productName, string productVersion, int quantity)
        {
            ProductName = productName;
            Version = productVersion;
            Quantity = quantity;
        }
    }
}
