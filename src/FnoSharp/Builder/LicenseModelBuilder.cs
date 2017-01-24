namespace FnoSharp.Builder
{
    class LicenseModelBuilder : BuilderBase<licenseModelIdentifierType>
    {
        public void SetLicenseModel(string licenseModel)
        {
            Object.primaryKeys = new licenseModelPKType
            {
                name = licenseModel
            };
        }
    }
}
