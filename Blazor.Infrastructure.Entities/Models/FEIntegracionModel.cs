using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Blazor.Infrastructure.Entities.Models;

#region Clase para respuesta de endpoints
public class FEResultJson<T>
{
    [JsonProperty("IsValid")]
    public bool IsValid { get; set; }

    [JsonProperty("Warnings")]
    public List<object> Warnings { get; set; }

    [JsonProperty("Errors")]
    public List<object> Errors { get; set; }

    [JsonProperty("ResultData")]
    public T ResultData { get; set; }

    [JsonProperty("ResultCode")]
    public int ResultCode { get; set; }
}

public class RespuestaStatus
{
    [JsonProperty("Status")]
    public string Status { get; set; }

    [JsonProperty("ValidationErrors")]
    public List<Validaciones> ValidationErrors { get; set; } = new List<Validaciones>();
}

public class Validaciones
{
    [JsonProperty("Field")]
    public string Field { get; set; }

    [JsonProperty("Code")]
    public string Code { get; set; }

    [JsonProperty("Description")]
    public object Description { get; set; }

    //[JsonProperty("ExplanationValues")]
    //public List<object> ExplanationValues { get; set; }
}

#endregion

#region Clases para el Token

public class FeTokenJson
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("virtual_operator")]
    public string VirtualOperator { get; set; }
}

public class RespuestaFeTokenJson
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires")]
    public DateTime Expires { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("virtual_operator_alias")]
    public string VirtualOperatorAlias { get; set; }
}

#endregion

#region Clases para Enviar Factura

public class FeRootJson
{
    [JsonProperty("BillingPeriod")]
    public FeBillingPeriod BillingPeriod { get; set; } = new FeBillingPeriod();

    [JsonProperty("CorrelationDocumentId")]
    public string CorrelationDocumentId { get; set; }

    [JsonProperty("Currency")]
    public string Currency { get; set; }

    [JsonProperty("CustomerParty")]
    public FeCustomerParty CustomerParty { get; set; } = new FeCustomerParty();

    [JsonProperty("DeliveryDate")]
    public DateTime DeliveryDate { get; set; }

    [JsonProperty("DocumentReferences")]
    public List<FeDocumentReference> DocumentReferences { get; set; } = new List<FeDocumentReference>();

    [JsonProperty("DueDate")]
    public string DueDate { get; set; }

    [JsonProperty("HealthcareData")]
    public FeHealthcareData HealthcareData { get; set; } = new FeHealthcareData();

    [JsonProperty("IssueDate")]
    public DateTime IssueDate { get; set; }

    [JsonProperty("IssuerParty")]
    public FeIssuerParty IssuerParty { get; set; } = new FeIssuerParty();

    [JsonProperty("Lines")]
    public List<FeLine> Lines { get; set; } = new List<FeLine>();

    [JsonProperty("Notes")]
    public List<string> Notes { get; set; } = new List<string>();

    [JsonProperty("OperationType")]
    public string OperationType { get; set; }

    [JsonProperty("PrepaidPayments")]
    public List<FePrepaidPayment> PrepaidPayments { get; set; } = new List<FePrepaidPayment>();

    [JsonProperty("SerieExternalKey")]
    public string SerieExternalKey { get; set; }

    [JsonProperty("SerieNumber")]
    public string SerieNumber { get; set; }

    [JsonProperty("SeriePrefix")]
    public string SeriePrefix { get; set; }

    [JsonProperty("Total")]
    public FeTotal Total { get; set; } = new FeTotal();

    [JsonProperty("PaymentMeans")]
    public List<FEPaymentMeans> PaymentMeans { get; set; } = new List<FEPaymentMeans>();
}

public class FEPaymentMeans
{
    [JsonProperty("Code")]
    public string Code { get; set; }

    [JsonProperty("Mean")]
    public string Mean { get; set; }

    [JsonProperty("DueDate")]
    public string DueDate { get; set; }
}

public class FeAddress
{
    [JsonProperty("AddressLine")]
    public string AddressLine { get; set; }

    [JsonProperty("CityCode")]
    public string CityCode { get; set; }

    [JsonProperty("Country")]
    public string Country { get; set; }

    [JsonProperty("DepartmentCode")]
    public string DepartmentCode { get; set; }
}

public class FeBillingPeriod
{
    [JsonProperty("From")]
    public string From { get; set; }

    [JsonProperty("To")]
    public string To { get; set; }
}

public class FeCollection
{
    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("NameValues")]
    public List<FeNameValue> NameValues { get; set; } = new List<FeNameValue>();
}

public class FeCustomerParty
{
    [JsonProperty("Address")]
    public FeAddress Address { get; set; } = new FeAddress();

    [JsonProperty("Email")]
    public string Email { get; set; }

    [JsonProperty("Identification")]
    public FeIdentification Identification { get; set; } = new FeIdentification();

    [JsonProperty("LegalType")]
    public string LegalType { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("ResponsabilityTypes")]
    public List<string> ResponsabilityTypes { get; set; } = new List<string>();

    [JsonProperty("TaxScheme")]
    public string TaxScheme { get; set; }
}

public class FeDocumentReference
{
    [JsonProperty("DocumentReferred")]
    public string DocumentReferred { get; set; }

    [JsonProperty("IssueDate")]
    public string IssueDate { get; set; }

    [JsonProperty("Type")]
    public string Type { get; set; }
}

public class FeHealthcareData
{
    [JsonProperty("Collections")]
    public List<FeCollection> Collections { get; set; } = new List<FeCollection>();
}

public class FeIdentification
{
    [JsonProperty("CheckDigit")]
    public string CheckDigit { get; set; }

    [JsonProperty("CountryCode")]
    public string CountryCode { get; set; }

    [JsonProperty("DocumentNumber")]
    public string DocumentNumber { get; set; }

    [JsonProperty("DocumentType")]
    public string DocumentType { get; set; }
}

public class FeIssuerParty
{
    [JsonProperty("Identification")]
    public FeIdentification Identification { get; set; } = new FeIdentification();
}

public class FeItem
{
    [JsonProperty("Description")]
    public string Description { get; set; }

    [JsonProperty("Gtin")]
    public string Gtin { get; set; }
}

public class FeLine
{
    [JsonProperty("ExcludeVat")]
    public string ExcludeVat { get; set; }

    [JsonProperty("GrossAmount")]
    public string GrossAmount { get; set; }

    [JsonProperty("Item")]
    public FeItem Item { get; set; } = new FeItem();

    [JsonProperty("NetAmount")]
    public string NetAmount { get; set; }

    [JsonProperty("Number")]
    public string Number { get; set; }

    [JsonProperty("Quantity")]
    public string Quantity { get; set; }

    [JsonProperty("QuantityUnitOfMeasure")]
    public string QuantityUnitOfMeasure { get; set; }

    [JsonProperty("UnitPrice")]
    public string UnitPrice { get; set; }

    [JsonProperty("WithholdingTaxSubTotals")]
    public List<FeWithholdingTaxSubTotal> WithholdingTaxSubTotals { get; set; } = new List<FeWithholdingTaxSubTotal>();

    [JsonProperty("WithholdingTaxTotals")]
    public List<FeWithholdingTaxTotal> WithholdingTaxTotals { get; set; } = new List<FeWithholdingTaxTotal>();
}

public class FeNameValue
{
    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Value")]
    public string Value { get; set; }

    [JsonProperty("CodeListCode")]
    public string CodeListCode { get; set; }

    [JsonProperty("CodeListName")]
    public string CodeListName { get; set; }
}

public class FePrepaidPayment
{
    [JsonProperty("PaidAmount")]
    public string PaidAmount { get; set; }

    [JsonProperty("PaidDate")]
    public DateTime PaidDate { get; set; }
}

public class FeTotal
{
    [JsonProperty("GrossAmount")]
    public string GrossAmount { get; set; }

    [JsonProperty("PayableAmount")]
    public string PayableAmount { get; set; }

    [JsonProperty("PrePaidTotalAmount")]
    public string PrePaidTotalAmount { get; set; }

    [JsonProperty("TaxableAmount")]
    public string TaxableAmount { get; set; }

    [JsonProperty("TotalBillableAmount")]
    public string TotalBillableAmount { get; set; }
}

public class FeWithholdingTaxSubTotal
{
    [JsonProperty("TaxableAmount")]
    public string TaxableAmount { get; set; }

    [JsonProperty("TaxAmount")]
    public string TaxAmount { get; set; }

    [JsonProperty("TaxPercentage")]
    public string TaxPercentage { get; set; }

    [JsonProperty("WithholdingTaxCategory")]
    public string WithholdingTaxCategory { get; set; }
}

public class FeWithholdingTaxTotal
{
    [JsonProperty("TaxAmount")]
    public string TaxAmount { get; set; }

    [JsonProperty("WithholdingTaxCategory")]
    public string WithholdingTaxCategory { get; set; }
}

#endregion
