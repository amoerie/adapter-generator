namespace Broka.WebApi.Data.GeneratedWebService {
  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.adaptergenerator.com/WS/2006/02/08")]
  public partial class BrokaDepartment {

    private int idField;

    private string abbreviationField;

    private string nameField;

    private short futureBookDateInDaysField;

    private BrokaPhoneNumber phoneField;

    private BrokaPhoneNumber contactPhoneField;

    private string contactNameField;

    private string contactMailField;

    private string externalCodeField;

    private string descriptionField;

    private System.Guid uniqueIdentifierField;

    /// <remarks/>
    public int Id {
      get {
        return this.idField;
      }
      set {
        this.idField = value;
      }
    }

    /// <remarks/>
    public string Abbreviation {
      get {
        return this.abbreviationField;
      }
      set {
        this.abbreviationField = value;
      }
    }

    /// <remarks/>
    public string Name {
      get {
        return this.nameField;
      }
      set {
        this.nameField = value;
      }
    }

    /// <remarks/>
    public short FutureBookDateInDays {
      get {
        return this.futureBookDateInDaysField;
      }
      set {
        this.futureBookDateInDaysField = value;
      }
    }

    /// <remarks/>
    public BrokaPhoneNumber Phone {
      get {
        return this.phoneField;
      }
      set {
        this.phoneField = value;
      }
    }

    /// <remarks/>
    public BrokaPhoneNumber ContactPhone {
      get {
        return this.contactPhoneField;
      }
      set {
        this.contactPhoneField = value;
      }
    }

    /// <remarks/>
    public string ContactName {
      get {
        return this.contactNameField;
      }
      set {
        this.contactNameField = value;
      }
    }

    /// <remarks/>
    public string ContactMail {
      get {
        return this.contactMailField;
      }
      set {
        this.contactMailField = value;
      }
    }

    /// <remarks/>
    public string ExternalCode {
      get {
        return this.externalCodeField;
      }
      set {
        this.externalCodeField = value;
      }
    }

    /// <remarks/>
    public string Description {
      get {
        return this.descriptionField;
      }
      set {
        this.descriptionField = value;
      }
    }

    /// <remarks/>
    public System.Guid UniqueIdentifier {
      get {
        return this.uniqueIdentifierField;
      }
      set {
        this.uniqueIdentifierField = value;
      }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.adaptergenerator.com/WS/2006/02/08")]
  public partial class BrokaPhoneNumber {

    private string numberField;

    private BrokaPhoneNumberType typeField;

    private BrokaPhoneNumberUsage usageField;

    /// <remarks/>
    public string Number {
      get {
        return this.numberField;
      }
      set {
        this.numberField = value;
      }
    }

    /// <remarks/>
    public BrokaPhoneNumberType Type {
      get {
        return this.typeField;
      }
      set {
        this.typeField = value;
      }
    }

    /// <remarks/>
    public BrokaPhoneNumberUsage Usage {
      get {
        return this.usageField;
      }
      set {
        this.usageField = value;
      }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.adaptergenerator.com/WS/2006/02/08")]
  public enum BrokaPhoneNumberType {

    /// <remarks Phone yeah/>
    Phone,

    /// <remarks Fax yeah/>
    Fax,

    /// <remarks/>
    Mobile,

    /// <remarks/>
    Other,
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.adaptergenerator.com/WS/2006/02/08")]
  public enum BrokaPhoneNumberUsage {

    /// <remarks/>
    Private,

    /// <remarks/>
    Business,
  }
}