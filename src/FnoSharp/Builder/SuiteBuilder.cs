using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using FnoSharp.Model;

namespace FnoSharp.Builder
{
    public class SuiteBuilder : BuilderBase<createUniformSuiteDataType>
    {
        #region Constructors

        public SuiteBuilder()
        {
            Object.licenseTechnology = new licenseTechnologyIdentifierType
            {
                primaryKeys = new licenseTechnologyPKType()
            };
            Object.hostType = new hostTypePKType();
        }

        public SuiteBuilder(string name, string description, string version, string licenseTechnology = null, string hostType = null, IEnumerable<string> licenseModels = null, IEnumerable<string> partNumbers = null)
            : this()
        {
            Name = name;
            Description = description;
            Version = version;
            LicenseTechnology = licenseTechnology;
            HostType = hostType;
            LicenseModels.AddRange(licenseModels);
            Parts.AddRange(partNumbers);
        }

        public SuiteBuilder(string name, string description, string version, string licenseTechnology = null, string hostType = null, string licenseModel = null, string part = null)
            : this(name, description, version, licenseTechnology, hostType, new[] { licenseModel }, new[] { part })
        {
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return Object.suiteName; }
            set { Object.suiteName = value; }
        }
        public string Version
        {
            get { return Object.version; }
            set { Object.version = value; }
        }

        public string Description
        {
            get { return Object.description; }
            set { Object.description = value; }
        }

        public string LicenseTechnology
        {
            get { return Object.licenseTechnology?.primaryKeys.name; }
            set { Object.licenseTechnology.primaryKeys.name = value; }
        }

        public string HostType
        {
            get { return Object.hostType.name; }
            set { Object.hostType.name = value; }
        }

        #region
        public SuiteProductCollection Products
        {
            get
            {
                if (_Products == null)
                {
                    _Products = new SuiteProductCollection();
                    _Products.CollectionChanged += ProductsCollectionChanged;
                }
                return _Products;
            }
        } private SuiteProductCollection _Products;

        private void ProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Object.products = Products.ToArray();
        }
        #endregion

        #region Part / SKU
        public ObservableRangeCollection<string> Parts
        {
            get
            {
                if (_PartNumbers == null)
                {
                    _PartNumbers = new ObservableRangeCollection<string>();
                    _PartNumbers.CollectionChanged += PartsCollectionChanged; ;
                }
                return _PartNumbers;
            }
        }
        private ObservableRangeCollection<string> _PartNumbers;


        private void PartsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (LicenseModels.Count == 0)
            {
                Object.partNumbers = (from string name in Parts
                                      select new partNumberIdentifierWithModelType { primaryKeys = new partNumberPKType { partId = name } }).ToArray();
            }
            else if (LicenseModels.Count < Parts.Count)
            {
                Object.partNumbers = (from string name in Parts
                                      select new partNumberIdentifierWithModelType
                                      {
                                          primaryKeys = new partNumberPKType { partId = name },
                                          licenseModel = new licenseModelIdentifierType
                                          {
                                              primaryKeys = new licenseModelPKType
                                              {
                                                  name = LicenseModels.FirstOrDefault()
                                              }
                                          }
                                      }).ToArray();
            }
            else if (LicenseModels.Count >= Parts.Count)
            {
                Object.partNumbers = Parts.Select((partNumber, i) => new partNumberIdentifierWithModelType
                {
                    primaryKeys = new partNumberPKType { partId = partNumber },
                    licenseModel = new licenseModelIdentifierType
                    {
                        primaryKeys = new licenseModelPKType
                        {
                            name = LicenseModels[i]
                        }
                    }
                }).ToArray();
            }
        }
        #endregion

        public ObservableRangeCollection<string> LicenseModels
        {
            get
            {
                if (_LicenseModels == null)
                {
                    _LicenseModels = new ObservableRangeCollection<string>();
                    _LicenseModels.CollectionChanged += _LicenseModelCollectionChanged;
                }
                return _LicenseModels;
            }
        }
        private ObservableRangeCollection<string> _LicenseModels;

        private void _LicenseModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Object.licenseModels = (from string name in LicenseModels select new licenseModelIdentifierType { primaryKeys = new licenseModelPKType { name = name } }).ToArray();
        }

        #endregion
    }
}
