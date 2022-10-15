﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Enums;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using Microsoft.Extensions.Configuration;

namespace ClearBank.DeveloperTest.Tests;

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