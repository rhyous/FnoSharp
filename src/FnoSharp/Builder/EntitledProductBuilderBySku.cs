namespace FnoSharp.Builder
{
    public class EntitledProductBuilderBySku : BuilderBase<partNumberIdentifierType>
    {
        #region Constructors

        public EntitledProductBuilderBySku()
        {
        }

        public EntitledProductBuilderBySku(string sku, int quantity)
        {
            SetProduct(sku, quantity);
        }

        #endregion

        #region Properties

        public string Sku
        {
            get { return SkuIdentifier.primaryKeys.partId; }
            set { SkuIdentifier.primaryKeys.partId = value; }
        }

        public int Quantity { get; set; }

        private partNumberIdentifierType SkuIdentifier
        {
            get { return Object ?? (Object = new partNumberIdentifierType { primaryKeys = new partNumberPKType { } }); }
        }

        #endregion

        public void SetProduct(string sku, int quantity)
        {
            Sku = sku;
            Quantity = quantity;
        }
    }
}
