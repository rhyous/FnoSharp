namespace FnoSharp.Model
{
    public class SuiteProductCollection : ObservableRangeCollection<productIdentifierWithCountDataType>
    {
        public void Add(string name, string version, int qty = 1)
        {
            Add(new productIdentifierWithCountDataType
            {
                
                productIdentifier = new productIdentifierType
                {                    
                    primaryKeys = new productPKType { name = name, version = version }
                },
                count = qty.ToString()
            });
        }
    }
}
