using ClearBank.DeveloperTest.Data.Enums;
using ClearBank.DeveloperTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;
using Moq;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types.Enums;

namespace ClearBank.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly Mock<IDataStore<Account>> _mockDataStore;
    private readonly PaymentService _paymentService;
    private const string TestAccountId = "1234-5678-9123";

    public PaymentServiceTests()
    {
        _mockDataStore = new();
        _paymentService = new(_mockDataStore.Object);
    }


    [Fact]
    public void Test_MakePayment_Fails_When_Account_Doesnt_Exists()
    {
        // Arrange
        var account = new Account();

        //Act
        var paymentResult = _paymentService.MakePayment(new() { });

        // Assert
        Assert.Equal(MakePaymentResult.Failed, paymentResult);
        _mockDataStore.Verify(d => d.TryGet(It.IsAny<string>(), out account), Times.Once());
        _mockDataStore.Verify(d => d.Update(account), Times.Never());
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    public void Test_MakePayment_To_Accounts(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        // Arrange
        var account = new Account()
        {
            AllowedPaymentSchemes = allowedPaymentSchemes,
            Balance = 100,
            Status = AccountStatus.Live
        };
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);

        //Act
        var request = new MakePaymentRequest()
        {
            DebtorAccountNumber = TestAccountId,
            Amount = 10,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);

        // Assert
        Assert.Equal(MakePaymentResult.Succeeded, paymentResult);
        Assert.Equal(100 - request.Amount, account.Balance);
        _mockDataStore.Verify(d => d.TryGet(It.IsAny<string>(), out account), Times.Once());
        _mockDataStore.Verify(d => d.Update(account), Times.Once());
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
    public void Test_MakePayment_To_Accounts_Fail_If_Not_Allowed(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        // Arrange
        var account = new Account()
        {
            AllowedPaymentSchemes = allowedPaymentSchemes,
            Balance = 100,
            Status = AccountStatus.Live
        };
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);

        //Act
        var request = new MakePaymentRequest()
        {
            DebtorAccountNumber = TestAccountId,
            Amount = 10,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);

        // Assert
        Assert.Equal(MakePaymentResult.Failed, paymentResult);
        Assert.Equal(100, account.Balance);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    public void Test_MakePayment_To_Accounts_Fail_If_Disabled(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        // Arrange
        var account = new Account()
        {
            AllowedPaymentSchemes = allowedPaymentSchemes,
            Balance = 100,
            Status = AccountStatus.Disabled
        };
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);

        //Act
        var request = new MakePaymentRequest()
        {
            DebtorAccountNumber = TestAccountId,
            Amount = 10,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);

        // Assert
        switch (paymentScheme)
        {
            case PaymentScheme.Bacs:
                Assert.Equal(MakePaymentResult.Succeeded, paymentResult);
                Assert.Equal(100 - request.Amount, account.Balance);
                break;
            case PaymentScheme.Chaps:
                Assert.Equal(MakePaymentResult.Failed, paymentResult);
                Assert.Equal(100, account.Balance);
                break;
            case PaymentScheme.FasterPayments:
                Assert.Equal(MakePaymentResult.Succeeded, paymentResult);
                Assert.Equal(100 - request.Amount, account.Balance);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(paymentScheme), paymentScheme, null);
        }
    }
}