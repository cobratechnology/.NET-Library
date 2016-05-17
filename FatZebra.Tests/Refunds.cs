﻿using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
    [TestFixture]
    public class Refunds
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
        public void RefundShouldBeSuccessful()
        {
            var purchase = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");

            var refund = Refund.Create(120, purchase.Result.ID, "Refund" + Guid.NewGuid().ToString());

            Assert.IsTrue(refund.Successful);
            Assert.IsTrue(refund.Result.Successful);
            Assert.IsNotNull(refund.Result.ID);
            Assert.AreEqual(((Refund)refund.Result).Amount, 120);
        }

        [Test]
        public void RefundsViaPurchaseShouldBeSuccessful()
        {
            var purchase = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");

            var thePurchase = ((Purchase)purchase.Result);
            var refund = thePurchase.Refund(120, "Refund " + Guid.NewGuid().ToString());

            Assert.IsTrue(refund.Successful);
            Assert.IsTrue(refund.Result.Successful);
            Assert.IsNotNull(refund.Result.ID);
            Assert.AreEqual(((Refund)refund.Result).Amount, 120);
        }
    }
}
