using System;
using System.Collections.Generic;

namespace FnoSharp.Builder
{
    public class EntitlementLineItemBuilder : BuilderBase<createEntitlementLineItemDataType>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName">The product name.</param>
        /// <param name="productVersion">The product version.</param>
        /// <param name="quantity">The quantity of the product.</param>
        /// <param name="licenseModel">The license model.</param>
        /// <param name="lineItemQuantity">The quantity of line product. If the product quantity has 20 and you are selling 20, this should be 1. If this was 20, you would be selling 20 * 20, which is 400.</param>
        public void AddEntitledProduct(string productName, string productVersion, int quantity, string licenseModel, int lineItemQuantity = 1)
        {
            var entitledProductBuilder = new EntitledProductBuilder();
            entitledProductBuilder.SetProduct(productName, productVersion, quantity);
            AddEntitledProduct(entitledProductBuilder, licenseModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sku">The sku version.</param>
        /// <param name="quantity">The quantity of the product.</param>
        /// <param name="licenseModel">The license model.</param>
        /// <param name="lineItemQuantity">The quantity of line product. If the product quantity has 20 and you are selling 20, this should be 1. If this was 20, you would be selling 20 * 20, which is 400.</param>
        public void AddEntitledProduct(string sku, int quantity, string licenseModel, int lineItemQuantity = 1)
        {
            Object.partNumber = new partNumberIdentifierType { primaryKeys = new partNumberPKType { partId = sku } };

        }

        public void AddEntitledProduct(EntitledProductBuilder productBuilder, string licenseModel, int lineItemQuantity = 1)
        {
            SetLicenseModel(licenseModel);
            // One line item by default because the product has quantity too. If you sell a quantity of 50 single licenses, then whichever
            // has the quantity, the other should be one.  1 * 50 = 50. If you put 50 for both product and line item, it would be 50 * 50, or 2500, 
            // which is not intended. 
            // However, if you have a product that is a 10 pack and you are selling 100, you would set quantity to 10 and lineItemQuantity to 10
            // so that 10 * 10 = 100.
            Object.numberOfCopies = lineItemQuantity.ToString(); 
            Object.isPermanent = true;
            Object.isPermanentSpecified = true;
            Object.activationId = new idType() { id = Guid.NewGuid().ToString() };
            var list = Object.entitledProducts == null
                ? new List<entitledProductDataType>()
                : new List<entitledProductDataType>(Object.entitledProducts);
            list.Add(productBuilder.Object);
            Object.entitledProducts = list.ToArray();
        }

        public void SetLicenseModel(string licenseModel)
        {
            var licenseModelBuilder = new LicenseModelBuilder();
            licenseModelBuilder.SetLicenseModel(licenseModel);
            Object.licenseModel = licenseModelBuilder.Object;
        }
    }
}
