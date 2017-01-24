using System;

namespace FnoSharp.Builder
{
    public class MaintenanceLineItemBuilder : BuilderBase<createMaintenanceLineItemDataType>
    {
        public void AddMaintenance(string productName, string productVersion, DateTime maintStart, DateTime maintEnd, string maintOrderId = null)
        {
            var entitledProductBuilder = new EntitledProductBuilder();
            entitledProductBuilder.SetProduct(productName, productVersion, 0);
            AddMaintenance(entitledProductBuilder, maintStart, maintEnd, maintOrderId);
        }

        public void AddMaintenance(EntitledProductBuilder productBuilder, DateTime maintStart, DateTime maintEnd, string maintOrderId = null)
        {
            Object.isPermanent = false;
            Object.isPermanentSpecified = true;
            Object.startDate = maintStart;
            Object.startDateSpecified = true;
            Object.expirationDate = maintEnd;
            Object.expirationDateSpecified = true;
            Object.activationId = new idType() { id = Guid.NewGuid().ToString() };
            Object.maintenanceProduct = productBuilder.Object.product;
            Object.orderId = maintOrderId;
        }
    }
}
