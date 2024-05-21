using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Blazor.Infrastructure.Entities.Models;

#region Clase para respuesta de endpoints
public class FEResultJson<T>
{
    [JsonProperty("IsValid")]
    public bool IsValid { get; set; }

    [JsonProperty("Warnings")]
    public List<string> Warnings { get; set; }

    [JsonProperty("Errors")]
    public List<FeErrorStatus400> Errors { get; set; }

    [JsonProperty("ResultData")]
    public T ResultData { get; set; }

    [JsonProperty("ResultCode")]
    public int ResultCode { get; set; }

    public List<string> ListaErrores
    {
        get
        {
            if (Errors != null && Errors.Any())
            {
                return Errors.Select(x => $"Codigo:{x.Code}, Campo: {x.Field}").ToList();
            }
            else
            {
                return new List<string>();
            }
        }
    }
}

public class FeErrorStatus400
{
    [JsonProperty("Code")]
    public string Code { get; set; }

    [JsonProperty("Description")]
    public object Description { get; set; }

    [JsonProperty("ExplanationValues")]
    public List<object> ExplanationValues { get; set; }

    [JsonProperty("Field")]
    public string Field { get; set; }
}

public class RespuestaStatus
{
    [JsonProperty("Status")]
    public string Status { get; set; }

    [JsonProperty("ValidationErrors")]
    public List<ValidacionesError> ValidationErrors { get; set; } = new List<ValidacionesError>();
}

public class ValidacionesError
{
    [JsonProperty("Field")]
    public string Field { get; set; }

    [JsonProperty("Code")]
    public string Code { get; set; }

    [JsonProperty("Description")]
    public string Description { get; set; }

    [JsonProperty("ExplanationValues")]
    public List<string> ExplanationValues { get; set; }
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

#region Clases para documento

public class FeFacturaJson
{
    [JsonProperty("Currency")]
    public string Currency { get; set; }

    [JsonProperty("SeriePrefix")]
    public string SeriePrefix { get; set; }

    [JsonProperty("SerieNumber")]
    public string SerieNumber { get; set; }

    [JsonProperty("OperationType")]
    public string OperationType { get; set; }

    [JsonProperty("IssueDate")]
    public DateTime IssueDate { get; set; }

    [JsonProperty("DeliveryDate")]
    public DateTime DeliveryDate { get; set; }

    [JsonProperty("DueDate")]
    public string DueDate { get; set; }

    [JsonProperty("CorrelationDocumentId")]
    public string CorrelationDocumentId { get; set; }

    [JsonProperty("SerieExternalKey")]
    public string SerieExternalKey { get; set; }

    [JsonProperty("IssuerParty")]
    public FeIssuerParty IssuerParty { get; set; } = new FeIssuerParty();

    [JsonProperty("PaymentMeans")]
    public List<FePaymentMean> PaymentMeans { get; set; } = new List<FePaymentMean>();

    [JsonProperty("BillingPeriod")]
    public FeBillingPeriod BillingPeriod { get; set; } = new FeBillingPeriod();

    [JsonProperty("CustomerParty")]
    public FeCustomerParty CustomerParty { get; set; } = new FeCustomerParty();

    [JsonProperty("Lines")]
    public List<FeLine> Lines { get; set; } = new List<FeLine>();

    [JsonProperty("Total")]
    public FeTotal Total { get; set; } = new FeTotal();

    [JsonProperty("PrepaidPayments")]
    public List<FePrepaidPayment> PrepaidPayments { get; set; } = new List<FePrepaidPayment>();

    [JsonProperty("DocumentReferences")]
    public List<FeDocumentReference> DocumentReferences { get; set; } = new List<FeDocumentReference>();

    [JsonProperty("Notes")]
    public List<string> Notes { get; set; } = new List<string>();

    [JsonProperty("HealthcareData")]
    public FeHealthcareData HealthcareData { get; set; } = new FeHealthcareData();
}

public class FeNotaDebitoJson : FeNotaJson
{
    [JsonProperty("ReasonDebit")]
    public string ReasonDebit { get; set; }
}

public class FeNotaCreditoJson : FeNotaJson
{
    [JsonProperty("ReasonCredit")]
    public string ReasonCredit { get; set; }
}

public class FeNotaJson
{
    [JsonProperty("Currency")]
    public string Currency { get; set; }

    [JsonProperty("SeriePrefix")]
    public string SeriePrefix { get; set; }

    [JsonProperty("SerieNumber")]
    public string SerieNumber { get; set; }

    [JsonProperty("OperationType")]
    public string OperationType { get; set; }

    [JsonProperty("IssueDate")]
    public DateTime IssueDate { get; set; }

    [JsonProperty("DueDate")]
    public DateTime DueDate { get; set; }

    [JsonProperty("DeliveryDate")]
    public DateTime DeliveryDate { get; set; }

    [JsonProperty("CorrelationDocumentId")]
    public string CorrelationDocumentId { get; set; }

    [JsonProperty("SerieExternalKey")]
    public string SerieExternalKey { get; set; }

    [JsonProperty("IssuerParty")]
    public FeIssuerParty IssuerParty { get; set; } = new FeIssuerParty();

    [JsonProperty("PaymentMeans")]
    public List<FePaymentMean> PaymentMeans { get; set; } = new List<FePaymentMean>();

    [JsonProperty("CustomerParty")]
    public FeCustomerParty CustomerParty { get; set; } = new FeCustomerParty();

    [JsonProperty("Lines")]
    public List<FeLine> Lines { get; set; } = new List<FeLine>();

    [JsonProperty("Total")]
    public FeTotal Total { get; set; } = new FeTotal();

    [JsonProperty("DocumentReferences")]
    public List<FeDocumentReference> DocumentReferences { get; set; } = new List<FeDocumentReference>();
}

public class FePaymentMean
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
    [JsonProperty("LegalType")]
    public string LegalType { get; set; }

    [JsonProperty("Email")]
    public string Email { get; set; }

    [JsonProperty("TaxScheme")]
    public string TaxScheme { get; set; }

    [JsonProperty("ResponsabilityTypes")]
    public List<string> ResponsabilityTypes { get; set; } = new List<string>();

    [JsonProperty("Identification")]
    public FeIdentification Identification { get; set; } = new FeIdentification();

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Address")]
    public FeAddress Address { get; set; } = new FeAddress();

    [JsonProperty("Person")]
    public FePerson Person { get; set; } = new FePerson();

}

public class FeDocumentReference
{
    [JsonProperty("DocumentReferred")]
    public string DocumentReferred { get; set; }

    [JsonProperty("IssueDate")]
    public string IssueDate { get; set; }

    [JsonProperty("Type")]
    public string Type { get; set; }

    [JsonProperty("DocumentReferredCUFE")]
    public string DocumentReferredCUFE { get; set; }
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

public class FePerson
{
    [JsonProperty("FirstName")]
    public string FirstName { get; set; }

    [JsonProperty("MiddleName")]
    public string MiddleName { get; set; }

    [JsonProperty("FamilyName")]
    public string FamilyName { get; set; }
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
    [JsonProperty("Number")]
    public string Number { get; set; }

    [JsonProperty("Quantity")]
    public string Quantity { get; set; }

    [JsonProperty("QuantityUnitOfMeasure")]
    public string QuantityUnitOfMeasure { get; set; }

    [JsonProperty("ExcludeVat")]
    public string ExcludeVat { get; set; }

    [JsonProperty("AllowanceCharges")]
    public List<FeAllowanceCharges> AllowanceCharges { get; set; } = new List<FeAllowanceCharges>();

    [JsonProperty("UnitPrice")]
    public string UnitPrice { get; set; }

    [JsonProperty("GrossAmount")]
    public string GrossAmount { get; set; }

    [JsonProperty("NetAmount")]
    public string NetAmount { get; set; }

    [JsonProperty("Item")]
    public FeItem Item { get; set; } = new FeItem();
 
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

public class FeAllowanceCharges
{
    [JsonProperty("ChargeIndicator")]
    public string ChargeIndicator { get; set; }

    [JsonProperty("BaseAmount")]
    public string BaseAmount { get; set; }

    [JsonProperty("ReasonCode")]
    public string ReasonCode { get; set; }

    [JsonProperty("Reason")]
    public string Reason { get; set; }

    [JsonProperty("Amount")]
    public string Amount { get; set; }

    [JsonProperty("Percentage")]
    public string Percentage { get; set; }

    [JsonProperty("SequenceIndicator")]
    public string SequenceIndicator { get; set; }

}

public class FeRespuestaConsultaDocumento
{
    [JsonProperty("DocumentStatusReason")]
    public object DocumentStatusReason { get; set; }

    [JsonProperty("DocumentStatusReasonCode")]
    public object DocumentStatusReasonCode { get; set; }

    [JsonProperty("PaymentStatus")]
    public string PaymentStatus { get; set; }

    [JsonProperty("PaymentCorrelationId")]
    public object PaymentCorrelationId { get; set; }

    [JsonProperty("IsPdfGenerated")]
    public bool IsPdfGenerated { get; set; }

    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("DocumentType")]
    public string DocumentType { get; set; }

    [JsonProperty("DocumentSubType")]
    public string DocumentSubType { get; set; }

    [JsonProperty("DocumentNumber")]
    public string DocumentNumber { get; set; }

    [JsonProperty("OriginId")]
    public string OriginId { get; set; }

    [JsonProperty("OriginName")]
    public string OriginName { get; set; }

    [JsonProperty("OriginCode")]
    public string OriginCode { get; set; }

    [JsonProperty("DestinationId")]
    public string DestinationId { get; set; }

    [JsonProperty("DestinationName")]
    public string DestinationName { get; set; }

    [JsonProperty("DestinationCode")]
    public string DestinationCode { get; set; }

    [JsonProperty("DocumentDate")]
    public DateTime DocumentDate { get; set; }

    [JsonProperty("CreationDate")]
    public DateTime CreationDate { get; set; }

    [JsonProperty("DocumentStatus")]
    public string DocumentStatus { get; set; }

    [JsonProperty("DocumentStatusDate")]
    public DateTime DocumentStatusDate { get; set; }

    [JsonProperty("DocumentStatusApplicationResponseInvalidSignature")]
    public bool DocumentStatusApplicationResponseInvalidSignature { get; set; }

    [JsonProperty("CommunicationStatus")]
    public object CommunicationStatus { get; set; }

    [JsonProperty("CommunicationStatusComments")]
    public object CommunicationStatusComments { get; set; }

    [JsonProperty("MainEmailNotification")]
    public string MainEmailNotification { get; set; }

    [JsonProperty("MainEmailNotificationStatus")]
    public string MainEmailNotificationStatus { get; set; }

    [JsonProperty("MainEmailNotificationStatusReason")]
    public string MainEmailNotificationStatusReason { get; set; }

    [JsonProperty("Currency")]
    public string Currency { get; set; }

    [JsonProperty("TotalAmount")]
    public double TotalAmount { get; set; }

    [JsonProperty("Cufe")]
    public string Cufe { get; set; }
}

public class FeResutoGetXml
{
    [JsonProperty("FileName")]
    public string FileName { get; set; }

    [JsonProperty("Content")]
    public string Content { get; set; }
}

#endregion

#region Clases para respuesta de Series

public class FEResultadoSeries
{
    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("CompanyId")]
    public string CompanyId { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("AuthorizationNumber")]
    public string AuthorizationNumber { get; set; }

    [JsonProperty("Prefix")]
    public string Prefix { get; set; }

    [JsonProperty("ValidFrom")]
    public DateTime ValidFrom { get; set; }

    [JsonProperty("ValidTo")]
    public DateTime ValidTo { get; set; }

    [JsonProperty("StartValue")]
    public int StartValue { get; set; }

    [JsonProperty("EndValue")]
    public int EndValue { get; set; }

    [JsonProperty("EfectiveValue")]
    public object EfectiveValue { get; set; }

    [JsonProperty("DocumentType")]
    public string DocumentType { get; set; }

    [JsonProperty("SerieType")]
    public string SerieType { get; set; }

    [JsonProperty("TechnicalKey")]
    public string TechnicalKey { get; set; }

    [JsonProperty("Status")]
    public string Status { get; set; }

    [JsonProperty("AutoIncrement")]
    public bool AutoIncrement { get; set; }

    [JsonProperty("ExternalKey")]
    public string ExternalKey { get; set; }
}

#endregion
