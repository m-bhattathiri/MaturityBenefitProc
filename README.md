# MaturityBenefitProc

Life insurance maturity benefit processing and disbursement system built on ASP.NET MVC 5 (.NET Framework 4.6.1).

## Overview

MaturityBenefitProc handles end-to-end maturity benefit disbursement for endowment and money-back life insurance policies. The system manages bonus computation, survival benefit payouts, tax certificate generation, and payment processing through NEFT and cheque channels.

## Key Features

- **Endowment Policy Maturity**: Computes sum assured plus accrued reversionary bonus and terminal bonus for endowment plans reaching maturity
- **Money-Back Survival Benefits**: Schedules and disburses periodic survival benefit payouts for money-back policies at predefined intervals
- **Bonus Computation Engine**: Calculates simple reversionary bonus, compound reversionary bonus, terminal bonus, and loyalty additions based on IRDA guidelines
- **Tax Deduction at Source**: Generates Form 10(10D) certificates, computes TDS under Section 194DA, and handles PAN-based exemption thresholds
- **NEFT Disbursement**: Validates beneficiary bank accounts via IFSC lookup, processes NEFT credits, and tracks UTR numbers for payment confirmation
- **Cheque Dispatch**: Manages cheque printing, courier dispatch with AWB tracking, delivery confirmation, and undelivered cheque follow-up
- **Nominee Verification**: Validates nominee identity through KYC documents, handles multiple nominee share allocation, and manages guardian assignments for minor nominees
- **Claim Settlement Workflow**: Orchestrates the full settlement lifecycle from claim initiation through document verification, approval, payment, and discharge voucher generation
- **Premium Reconciliation**: Reconciles premium payment history, identifies lapses, computes paid-up values, and handles policy revival scenarios
- **Audit Trail**: Maintains comprehensive audit logs for regulatory compliance and internal review

## Technology Stack

- ASP.NET MVC 5.2.7
- .NET Framework 4.6.1
- Entity Framework 6.2.0
- MSTest / Moq 4.18.4
- SQL Server (via EF Code First)

## Project Structure

```
MaturityBenefitProc/
    Controllers/       MVC controllers for policy, claim, disbursement views
    Models/            Domain entities (Policy, Policyholder, Nominee, etc.)
    Helpers/           Business logic organized by domain
        BonusComputation/
        MaturityPayout/
        SurvivalBenefit/
        TaxCertificate/
        NeftDisbursement/
        ChequeDispatch/
        PolicyMaturityValidation/
        UndeliveredPayment/
        EndowmentPlan/
        MoneyBackPlan/
        ClaimSettlement/
        PremiumReconciliation/
        NomineeVerification/
        AuditCompliance/
        CustomerCommunication/
        Common/
    Views/             Razor views for dashboard, policy details, claims
    Data/              EF DbContext and migrations
    Repositories/      Generic repository pattern

MaturityBenefitProc.Tests/
    Helpers/           Unit tests mirroring main project helper structure
```

## Building

Open `MaturityBenefitProc.sln` in Visual Studio 2015+ and restore NuGet packages. Build in Debug or Release configuration.

## Running Tests

Use the Visual Studio Test Explorer or run `mstest` from the Developer Command Prompt targeting the test assembly.

## Configuration

Connection strings and application settings are managed through `Web.config`. Update the `MaturityBenefitDb` connection string to point to your SQL Server instance.

## License

Proprietary - Internal use only.
