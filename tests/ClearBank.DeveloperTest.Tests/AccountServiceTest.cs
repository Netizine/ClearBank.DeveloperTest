using ClearBank.DeveloperTest.Data.Enums;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Types.Enums;

namespace ClearBank.DeveloperTest.Tests;

public class AccountServiceTest
{
    private const string TestAccountNumber = "1234-5678-9123";
    private const decimal OpeningBalance = 100m;
    private const decimal PaymentAmount = 10m;


    [Fact]
    public void Test_Correct_Account_Exists()
    {
        // Arrange
        var provider = new DataStoreProvider(DataStoreType.Primary);

        //Act
        var dataStore = provider.GetAccountDataStore();
        var accountExists = dataStore.TryGet(TestAccountNumber, out var account);

        // Assert
        Assert.True(accountExists);
        Assert.Equal(TestAccountNumber, account.AccountNumber);
    }

    [Fact]
    public void Account_Can_Process_Bacs_Request()
    {
        // Arrange
        var testAccount = new Account
        {
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.Bacs, PaymentAmount);
        // Assert
        Assert.True(paymentResult);
        Assert.Equal(OpeningBalance - PaymentAmount, testAccount.Balance);
    }

    [Fact]
    public void Account_Can_Process_Chaps_Request()
    {
        // Arrange
        var testAccount = new Account
        {
            AccountNumber = TestAccountNumber,
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.Live
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.Chaps, PaymentAmount);
        // Assert
        Assert.True(paymentResult);
        Assert.Equal(OpeningBalance - PaymentAmount, testAccount.Balance);
    }

    [Fact]
    public void Account_Can_Process_FasterPayments_Request()
    {
        // Arrange
        var testAccount = new Account
        {
            AccountNumber = TestAccountNumber,
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Status = AccountStatus.Live
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.FasterPayments, PaymentAmount);
        // Assert
        Assert.True(paymentResult);
        Assert.Equal(OpeningBalance - PaymentAmount, testAccount.Balance);
    }

    [Fact]
    public void Wont_Process_Bacs_Request_If_Not_Enabled()
    {
        // Arrange
        var testAccount = new Account
        {
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments,
            Status = AccountStatus.Live
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.Bacs, PaymentAmount);
        // Assert
        Assert.False(paymentResult);
        Assert.Equal(OpeningBalance, testAccount.Balance);
    }

    [Fact]
    public void Wont_Process_Chaps_Request_If_Not_Allowed()
    {
        // Arrange
        var testAccount = new Account
        {
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments,
            Status = AccountStatus.Live
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.Chaps, PaymentAmount);
        // Assert
        Assert.False(paymentResult);
        Assert.Equal(OpeningBalance, testAccount.Balance);

    }

    [Fact]
    public void Wont_Process_FasterPayments_Request_If_Not_Allowed()
    {
        // Arrange
        var testAccount = new Account
        {
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.Live
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.FasterPayments, PaymentAmount);
        // Assert
        Assert.False(paymentResult);
        Assert.Equal(OpeningBalance, testAccount.Balance);

    }

    [Fact]
    public void Wont_Process_Chaps_Request_If_Status_Not_Live()
    {
        // Arrange
        var testAccount = new Account
        {
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.Disabled
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.Chaps, PaymentAmount);
        // Assert
        Assert.False(paymentResult);
        Assert.Equal(OpeningBalance, testAccount.Balance);
    }

    [Fact]
    public void Wont_Process_FasterPayments_Request_With_Insufficient_Funds()
    {
        // Arrange
        var testAccount = new Account
        {
            Balance = OpeningBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Status = AccountStatus.Live
        };
        //Act
        var paymentResult = testAccount.ProcessPayment(PaymentScheme.FasterPayments, OpeningBalance + PaymentAmount);
        // Assert
        Assert.False(paymentResult);
        Assert.Equal(OpeningBalance, testAccount.Balance);
    }
}