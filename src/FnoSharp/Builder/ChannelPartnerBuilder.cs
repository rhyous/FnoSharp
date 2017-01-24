namespace FnoSharp.Builder
{
    public class ChannelPartnerBuilder : BuilderBase<channelPartnerDataType>
    {
        public const string EndCustomer = "bo.constants.partnertiernames.endcustomer";
        public const string Distributor = "bo.constants.partnertiernames.tier1";

        public void SetOrganizationDataType(organizationDataType organization)
        {
            SetOrganizationDataType(organization.name);
        }

        public void SetOrganizationDataType(string orgName)
        {
            Object.organizationUnit = new organizationIdentifierType
            {
                primaryKeys = new organizationPKType
                {
                    name = orgName
                }
            };
            Object.tierName = EndCustomer;
        }
    }
}
