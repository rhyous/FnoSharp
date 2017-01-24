using System;
using System.Collections.Generic;
using System.Linq;

namespace FnoSharp.Builder
{
    public class SimpleEntitlementBuilder : BuilderBase<createSimpleEntitlementDataType>
    {
        public SimpleEntitlementBuilder(string entitlementIdPrefix = null)
        {
            Object.autoDeploy = true;
            Object.autoDeploySpecified = true;
            Object.entitlementId = new idType
            {
                id = entitlementIdPrefix + Guid.NewGuid()
            };
        }

        public string OrderId { get; set; }

        public string MaintenanceOrderId
        {
            get { return string.IsNullOrWhiteSpace(_MaintenanceOrderId) ? OrderId : _MaintenanceOrderId; }
            set { _MaintenanceOrderId = value; }
        } private string _MaintenanceOrderId;

        public void AddOrganization(organizationDataType organization)
        {
            var channelPartnerBuilder = new ChannelPartnerBuilder();
            channelPartnerBuilder.SetOrganizationDataType(organization);
            var list = (Object.channelPartners == null)
                ? new List<channelPartnerDataType>()
                : new List<channelPartnerDataType>(Object.channelPartners);
            list.Add(channelPartnerBuilder.Object);
            Object.channelPartners = list.ToArray();
        }

        public void AddEntitledProduct(string productName, string productVersion, int quantity, string licenseModel)
        {
            var entitlementLineItemBuilder = new EntitlementLineItemBuilder();
            entitlementLineItemBuilder.AddEntitledProduct(productName, productVersion, quantity, licenseModel);
            AddEntitlementLineItem(entitlementLineItemBuilder.Object);
        }

        public void AddEntitledProduct(string sku, int quantity, string licenseModel)
        {
            var entitlementLineItemBuilder = new EntitlementLineItemBuilder();
            entitlementLineItemBuilder.AddEntitledProduct(sku, quantity, licenseModel);
            AddEntitlementLineItem(entitlementLineItemBuilder.Object);
        }

        public void AddEntitledProducts(List<EntitledProductBuilder> builders, string licenseModel)
        {
            var entitlementLineItemBuilder = new EntitlementLineItemBuilder();
            foreach (var entitledProductBuilder in builders)
            {
                entitlementLineItemBuilder.AddEntitledProduct(entitledProductBuilder, licenseModel);
            }
            AddEntitlementLineItem(entitlementLineItemBuilder.Object);
        }

        public void AddMaintenance(string maintProductName, string maintVersion, DateTime maintStart, DateTime maintEnd)
        {
            var maintLineItemBuilder = new MaintenanceLineItemBuilder();
            maintLineItemBuilder.AddMaintenance(maintProductName, maintVersion, maintStart, maintEnd, MaintenanceOrderId);
            maintLineItemBuilder.Object.parentLineItem = new entitlementLineItemIdentifierType { primaryKeys = new entitlementLineItemPKType { activationId = Object.lineItems.Last().activationId.id } };
            AddMaintenanceLineItem(maintLineItemBuilder.Object);
        }

        public void AddEntitledProductWithMaintenance(string productName, string productVersion, int quantity, string licenseModel, string maintProductName, string maintVersion, DateTime maintStart, DateTime maintEnd)
        {
            var entitlementLineItemBuilder = new EntitledProductBuilder();
            entitlementLineItemBuilder.SetProduct(productName, productVersion, quantity);
            AddEntitledProductsWithMaintenance(new List<EntitledProductBuilder> { entitlementLineItemBuilder }, licenseModel, maintProductName, maintVersion, maintStart, maintEnd);
        }

        public void AddEntitledProductsWithMaintenance(List<EntitledProductBuilder> builders, string licenseModel, string maintProductName, string maintVersion, DateTime maintStartTime, DateTime maintEnd)
        {
            if (builders == null || builders.Count == 0)
                throw new ArgumentException("List<EntitledProductBuilder> cannot be null or empty.", "builders");
            AddEntitledProducts(builders, licenseModel);
            AddMaintenance(maintProductName, maintVersion, maintStartTime, maintEnd);
        }

        public void AddEntitlementLineItem(createEntitlementLineItemDataType entitlementLineItem)
        {
            var list = Object.lineItems == null
                ? new List<createEntitlementLineItemDataType>()
                : new List<createEntitlementLineItemDataType>(Object.lineItems);
            entitlementLineItem.orderLineNumber = list.Count.ToString();
            entitlementLineItem.orderId = OrderId;
            list.Add(entitlementLineItem);
            Object.lineItems = list.ToArray();
        }

        private void AddMaintenanceLineItem(createMaintenanceLineItemDataType maintenanceLineItem)
        {
            var list = Object.maintenanceLineItems == null
                ? new List<createMaintenanceLineItemDataType>()
                : new List<createMaintenanceLineItemDataType>(Object.maintenanceLineItems);
            list.Add(maintenanceLineItem);
            Object.maintenanceLineItems = list.ToArray();
        }
    }
}
