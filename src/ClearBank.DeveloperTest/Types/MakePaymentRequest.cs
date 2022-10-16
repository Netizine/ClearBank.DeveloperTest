﻿using ClearBank.DeveloperTest.Types.Enums;

namespace ClearBank.DeveloperTest.Types;
public class MakePaymentRequest
{
    public string CreditorAccountNumber { get; set; } = null!;

    public string DebtorAccountNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public PaymentScheme PaymentScheme { get; set; }
}
