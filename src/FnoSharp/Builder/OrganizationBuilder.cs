namespace FnoSharp.Builder
{
    public class OrganizationBuilder : BuilderBase<organizationDataType>
    {
        public string Id
        {
            get { return Object.name; }
            set { Object.name = value; }
        }
        
        public string displayName
        {
            get { return Object.displayName; }
            set { Object.displayName = value; }
        }

        public string Description
        {
            get { return Object.description; }
            set { Object.description = value; }
        }
    }
}
