﻿using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;
using Newtonsoft.Json;

namespace FatZebra.Tests
{
    [TestFixture]
    public class Purchases
    {
        [OneTimeSetUp]
        public void Init()
        {
            FatZebra.Gateway.Username = "TEST";
            FatZebra.Gateway.Token = "TEST";
            Gateway.SandboxMode = true;
            Gateway.TestMode = true;
        }

        [Test]
        public void PurchaseShouldBeSuccessful()
        {
            var response = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 120);

            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.20);

            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
            Assert.AreEqual(((Purchase)response.Result).SettlementDate, DateTime.Now.Date);
        }

        [Test]
        public void PurchaseShouldReturnErrors()
        {
            var response = Purchase.Create(120, "M Smith", "", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsFalse(response.Successful);
            Assert.IsFalse(response.Result.Successful);
            Assert.GreaterOrEqual(response.Errors.Count, 1);
        }

        [Test]
        public void PurchaseWithTokenShouldBeSuccessful()
        {
            var card = CreditCard.Create("M SMith", "5123456789012346", DateTime.Now.AddYears(1), "123");
            Assert.IsTrue(card.Successful);
            var response = Purchase.Create(123, card.Result.ID, "123", Guid.NewGuid().ToString(), "123.123.123.1");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 123);

            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.23);

            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
        }

        [Test]
        public void PurchaseWithTokenAndExtraVarsShouldBeSuccessful()
        {
            var extras = new Dictionary<String, Object>();
            extras.Add("ecm", "22");

            var card = CreditCard.Create("M SMith", "5123456789012346", DateTime.Now.AddYears(1), "123");
            Assert.IsTrue(card.Successful);
            var response = Purchase.Create(123, card.Result.ID, "123", Guid.NewGuid().ToString(), "123.123.123.1", "AUD", null, extras);
            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 123);
            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.23);
            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
        }

        [Test]
        public void PurchaseWithFraudCheckShouldBeSuccessful()
        {
            FraudCheck fc = new FraudCheck ();
            fc.DeviceID = "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"; // Will be generated by the deviceID JS library
            fc.Customer.ID = "ABD123";
            fc.Customer.FirstName = "James";
            fc.Customer.LastName = "Smith";
            fc.Customer.Email = "accept@email.com";
            fc.Customer.DOB = null;
            fc.Customer.AddressLine1 = "23 Smith Road";
            fc.Customer.City = "Canberra";
            fc.Customer.PostCode = "2600";
            fc.Customer.Country = "AUS";
            fc.Customer.HomePhone = "0421858999";
            fc.Customer.ExistingCustomer = true;
            fc.Customer.CreatedAt = DateTime.Now;

            fc.ShippingAddress.FirstName = "James";
            fc.ShippingAddress.LastName = "Smith";
            fc.ShippingAddress.Email = "accept@email.com";
            fc.ShippingAddress.AddressLine1 = "23 Smith Road";
            fc.ShippingAddress.City = "Canberra";
            fc.ShippingAddress.PostCode = "2600";
            fc.ShippingAddress.Country = "AUS";
            fc.ShippingAddress.HomePhone = "0421858999";
            fc.ShippingAddress.ShipMethod = ShippingMethod.Express;

            var item = new OrderItem ();
            item.ProductCode = "9999-A";
            item.SKU = "9999";
            item.Description = "Widgets";
            item.ItemCost = 23.30f;
            item.LineTotal = 23.30f;
            item.Quantity = 1;

            fc.OrderItems.Add (item);

            var recip = new Recipient ();
            recip.FirstName = "James";
            recip.LastName = "Smith";
            recip.Email = "james@smith.com";
            recip.AddressLine1 = "1 Fairfield Road";
            recip.City = "Austin";
            recip.State = "TX";
            recip.PostCode = "55555-1234";
            recip.Country = "USA";
            recip.PhoneNumber = "555-555-55555";

            fc.Recipients.Add (recip);
            fc.Website = "http://www.website.com";

            var response = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1", "AUD", fc);
            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(response.Result.Amount, 120);

            Assert.AreEqual(response.Result.DecimalAmount, 1.20);

            Assert.AreEqual(response.Result.CardType, "MasterCard");
            Assert.AreEqual(response.Result.FraudCheckResult, FraudResult.Accept);
        }

        [Test]
        public void PurchaseWithExtraVars() {
            var extras = new Dictionary<String, Object> ();
            extras.Add ("ecm", "22");
            var response = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1", "AUD", null, extras);
            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(response.Result.Amount, 120);

            Assert.AreEqual(response.Result.DecimalAmount, 1.20);

            Assert.AreEqual(response.Result.CardType, "MasterCard");
        }

        [Test]
        public void PurchaseWithDenyFraudCheckShouldNotBeSuccessful()
        {
            FraudCheck fc = new FraudCheck ();
            fc.DeviceID = "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"; // Will be generated by the deviceID JS library
            fc.Customer.ID = "ABD123";
            fc.Customer.FirstName = "James";
            fc.Customer.LastName = "Smith";
            fc.Customer.Email = "deny@email.com";
            fc.Customer.DOB = DateTime.Today.AddYears (-20);
            fc.Customer.AddressLine1 = "23 Smith Road";
            fc.Customer.City = "Canberra";
            fc.Customer.PostCode = "2600";
            fc.Customer.Country = "AUS";
            fc.Customer.HomePhone = "0421858999";
            fc.Customer.ExistingCustomer = true;
            fc.Customer.CreatedAt = DateTime.Now;

            fc.ShippingAddress.FirstName = "James";
            fc.ShippingAddress.LastName = "Smith";
            fc.ShippingAddress.Email = "deny@email.com";
            fc.ShippingAddress.AddressLine1 = "23 Smith Road";
            fc.ShippingAddress.City = "Canberra";
            fc.ShippingAddress.PostCode = "2600";
            fc.ShippingAddress.Country = "AUS";
            fc.ShippingAddress.HomePhone = "0421858999";
            fc.ShippingAddress.ShipMethod = ShippingMethod.Express;

            var item = new OrderItem ();
            item.ProductCode = "9999-A";
            item.SKU = "9999";
            item.Description = "Widgets";
            item.ItemCost = 23.30f;
            item.LineTotal = 23.30f;
            item.Quantity = 1;

            fc.OrderItems.Add (item);

            var recip = new Recipient ();
            recip.FirstName = "James";
            recip.LastName = "Smith";
            recip.Email = "james@smith.com";
            recip.AddressLine1 = "1 Fairfield Road";
            recip.City = "Austin";
            recip.State = "TX";
            recip.PostCode = "55555-1234";
            recip.Country = "USA";
            recip.PhoneNumber = "555-555-55555";

            fc.Recipients.Add (recip);
            fc.Website = "http://www.webite.com";
            fc.Custom.Add (13, "Donation");

            var response = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1", "AUD", fc);
            Assert.IsTrue(response.Successful);
            Assert.IsFalse(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(response.Result.Amount, 120);

            Assert.AreEqual(response.Result.DecimalAmount, 1.20);

            Assert.AreEqual(response.Result.CardType, "MasterCard");
            Assert.AreEqual(response.Result.FraudCheckResult, FraudResult.Deny);
            Assert.AreNotEqual(response.Result.FraudMessages.Count, 0);
        }
    }
}
