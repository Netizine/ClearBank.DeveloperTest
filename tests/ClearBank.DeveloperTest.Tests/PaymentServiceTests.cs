using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;
using Moq;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types.Enums;
using System.Diagnostics.CodeAnalysis;

namespace ClearBank.DeveloperTest.Tests;

[ExcludeFromCodeCoverage]
public class PaymentServiceTests
{
    private readonly Mock<IDataStore<Account>> _mockDataStore;
    private readonly PaymentService _paymentService;
    private const string TestAccountId = "1234-5678-9123";
    private const decimal OpeningBalance = 100m;
    private const decimal PaymentAmount = 10m;

    public PaymentServiceTests()
    {
        _mockDataStore = new Mock<IDataStore<Account>>();
        _paymentService = new PaymentService(_mockDataStore.Object);

    }


    [Fact]
    public void Test_MakePayment_Fails_When_Account_Is_Invalid()
    {
        // Arrange
        var account = new Account();
        var paymentRequest = new MakePaymentRequest();
        //Act
        var paymentResult = _paymentService.MakePayment(paymentRequest);
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
        var account = CreateAccount(allowedPaymentSchemes, AccountStatus.Live);
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);
        //Act
        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = TestAccountId,
            Amount = PaymentAmount,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);
        // Assert
        Assert.Equal(MakePaymentResult.Succeeded, paymentResult);
        Assert.Equal(OpeningBalance - request.Amount, account.Balance);
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
        var account = CreateAccount(allowedPaymentSchemes, AccountStatus.Live);
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);
        //Act
        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = TestAccountId,
            Amount = PaymentAmount,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);
        // Assert
        Assert.Equal(MakePaymentResult.Failed, paymentResult);
        Assert.Equal(OpeningBalance, account.Balance);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    public void Test_MakePayment_To_Accounts_Fail_If_Disabled(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        // Arrange
        var account = CreateAccount(allowedPaymentSchemes, AccountStatus.Disabled);
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);
        //Act
        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = TestAccountId,
            Amount = PaymentAmount,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);
        // Assert
        switch (paymentScheme)
        {
            case PaymentScheme.Bacs:
            case PaymentScheme.FasterPayments:
                Assert.Equal(MakePaymentResult.Succeeded, paymentResult);
                Assert.Equal(OpeningBalance - request.Amount, account.Balance);
                break;
            case PaymentScheme.Chaps:
                Assert.Equal(MakePaymentResult.Failed, paymentResult);
                Assert.Equal(OpeningBalance, account.Balance);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(paymentScheme), paymentScheme, null);
        }
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    public void Will_Fail_Making_Payment_When_Exception_Occurs(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        // Arrange
        var account = CreateAccount(allowedPaymentSchemes, AccountStatus.Live);
        _mockDataStore.Setup(s => s.TryGet(TestAccountId, out account)).Returns(true);
        _mockDataStore.Setup(s => s.Update(account)).Throws(() => new Exception());
        //Act
        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = TestAccountId,
            Amount = PaymentAmount,
            PaymentScheme = paymentScheme
        };
        var paymentResult = _paymentService.MakePayment(request);
        // Assert
        Assert.Equal(MakePaymentResult.Failed, paymentResult);
        _mockDataStore.Verify(d => d.TryGet(It.IsAny<string>(), out account), Times.Once());
        _mockDataStore.Verify(d => d.Update(account), Times.Once());

    }

    private static Account CreateAccount(AllowedPaymentSchemes allowedPaymentSchemes, AccountStatus accountStatus)
    {
        return new Account
        {
            AccountNumber = TestAccountId,
            AllowedPaymentSchemes = allowedPaymentSchemes,
            Balance = OpeningBalance,
            Status = accountStatus
        };
    }
}