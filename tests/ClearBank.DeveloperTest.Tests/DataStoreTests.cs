using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Enums;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace ClearBank.DeveloperTest.Tests;

[ExcludeFromCodeCoverage]
public class DataStoreTests
{

    [Fact]
    public void Test_DataStore_Configuration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .Build();

        // Act
        var dataStore = configuration["DataStore:Type"];

        // Assert
        Assert.Equal("Primary",dataStore);
    }

    [Fact]
    public void Test_Primary_Account_DataStore()
    {
        // Arrange
        var provider = new DataStoreProvider(DataStoreType.Primary);

        // Act
        var dataStore = provider.GetAccountDataStore();

        // Assert
        Assert.IsType<AccountDataStore>(dataStore);
    }

    [Fact]
    public void Test_Backup_Account_DataStore()
    {
        // Arrange
        var provider = new DataStoreProvider(DataStoreType.Backup);

        // Act
        var dataStore = provider.GetAccountDataStore();

        // Assert
        Assert.IsType<BackupAccountDataStore>(dataStore);
    }

    [Fact]
    public void Test_Primary_Account_DataStore_From_Config()
    {
        // Arrange
        var provider = new DataStoreProvider("Primary");

        // Act
        var dataStore = provider.GetAccountDataStore();

        // Assert
        Assert.IsType<AccountDataStore>(dataStore);

    }

    [Fact]
    public void Test_Backup_Account_DataStore_From_Config()
    {
        // Arrange
        var provider = new DataStoreProvider("Backup");

        // Act
        var dataStore = provider.GetAccountDataStore();

        // Assert
        Assert.IsType<BackupAccountDataStore>(dataStore);
    }
}