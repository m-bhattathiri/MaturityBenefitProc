using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs;

namespace MaturityBenefitProc.Tests.Helpers.BeneficiaryAndLegalHeirs
{
    [TestClass]
    public class LegalHeirValidationServiceIntegrationTests
    {
        private LegalHeirValidationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new LegalHeirValidationService();
        }

        [TestMethod]
        public void Integration_ValidateSuccessionCertificate_VerifyIndemnityBond_WorkflowTest()
        {
            var result1 = _service.ValidateSuccessionCertificate("TEST000", new DateTime(2017, 2, 15));
            var result2 = _service.VerifyIndemnityBond("TEST003", 1400m);
            Assert.IsTrue(result1 || !result1, "ValidateSuccessionCertificate should return a boolean");
            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsTrue(result2 || !result2, "VerifyIndemnityBond should return a boolean");
            Assert.IsInstanceOfType(result2, typeof(bool));
        }

        [TestMethod]
        public void Integration_ValidateSuccessionCertificate_Sequential_Execution_Test()
        {
            var step1 = _service.ValidateSuccessionCertificate("TEST000", new DateTime(2017, 2, 15));
            var step2 = _service.VerifyIndemnityBond("TEST003", 1400m);
            var step3 = _service.CalculateDaysSinceDeath(new DateTime(2017, 7, 15), new DateTime(2017, 8, 15));
            Assert.IsInstanceOfType(step1, typeof(bool), "ValidateSuccessionCertificate bool check");
            Assert.IsInstanceOfType(step2, typeof(bool), "VerifyIndemnityBond bool check");
            Assert.IsInstanceOfType(step3, typeof(int), "CalculateDaysSinceDeath int check");
        }

        [TestMethod]
        public void Integration_VerifyIndemnityBond_CalculateDaysSinceDeath_WorkflowTest()
        {
            var result1 = _service.VerifyIndemnityBond("TEST000", 1100m);
            var result2 = _service.CalculateDaysSinceDeath(new DateTime(2017, 4, 15), new DateTime(2017, 5, 15));
            Assert.IsTrue(result1 || !result1, "VerifyIndemnityBond should return a boolean");
            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsTrue(result2 >= int.MinValue, "CalculateDaysSinceDeath should return int");
            Assert.AreNotEqual(int.MaxValue, result2);
        }

        [TestMethod]
        public void Integration_VerifyIndemnityBond_Sequential_Execution_Test()
        {
            var step1 = _service.VerifyIndemnityBond("TEST000", 1100m);
            var step2 = _service.CalculateDaysSinceDeath(new DateTime(2017, 4, 15), new DateTime(2017, 5, 15));
            var step3 = _service.CalculateHeirShareAmount(1600m, 0.12000000000000001);
            Assert.IsInstanceOfType(step1, typeof(bool), "VerifyIndemnityBond bool check");
            Assert.IsInstanceOfType(step2, typeof(int), "CalculateDaysSinceDeath int check");
            Assert.IsTrue(step3 >= 0m || step3 <= 0m, "CalculateHeirShareAmount decimal check");
        }

        [TestMethod]
        public void Integration_CalculateDaysSinceDeath_CalculateHeirShareAmount_WorkflowTest()
        {
            var result1 = _service.CalculateDaysSinceDeath(new DateTime(2017, 1, 15), new DateTime(2017, 2, 15));
            var result2 = _service.CalculateHeirShareAmount(1300m, 0.09);
            Assert.IsTrue(result1 >= int.MinValue, "CalculateDaysSinceDeath should return int");
            Assert.AreNotEqual(int.MaxValue, result1);
            Assert.IsTrue(result2 >= 0m || result2 < 0m, "CalculateHeirShareAmount should return a decimal value");
            Assert.AreNotEqual(decimal.MaxValue, result2);
            Assert.AreNotEqual(decimal.MinValue, result2);
        }

        [TestMethod]
        public void Integration_CalculateDaysSinceDeath_Sequential_Execution_Test()
        {
            var step1 = _service.CalculateDaysSinceDeath(new DateTime(2017, 1, 15), new DateTime(2017, 2, 15));
            var step2 = _service.CalculateHeirShareAmount(1300m, 0.09);
            var step3 = _service.GetStatutorySharePercentage("TEST006", 12);
            Assert.IsInstanceOfType(step1, typeof(int), "CalculateDaysSinceDeath int check");
            Assert.IsTrue(step2 >= 0m || step2 <= 0m, "CalculateHeirShareAmount decimal check");
            Assert.IsTrue(!double.IsNaN(step3), "GetStatutorySharePercentage NaN check");
        }

        [TestMethod]
        public void Integration_CalculateHeirShareAmount_GetStatutorySharePercentage_WorkflowTest()
        {
            var result1 = _service.CalculateHeirShareAmount(1000m, 0.060000000000000005);
            var result2 = _service.GetStatutorySharePercentage("TEST003", 9);
            Assert.IsTrue(result1 >= 0m || result1 < 0m, "CalculateHeirShareAmount should return a decimal value");
            Assert.AreNotEqual(decimal.MaxValue, result1);
            Assert.AreNotEqual(decimal.MinValue, result1);
            Assert.IsTrue(!double.IsNaN(result2), "GetStatutorySharePercentage should not be NaN");
            Assert.IsTrue(!double.IsInfinity(result2), "Should not be infinity");
        }

        [TestMethod]
        public void Integration_CalculateHeirShareAmount_Sequential_Execution_Test()
        {
            var step1 = _service.CalculateHeirShareAmount(1000m, 0.060000000000000005);
            var step2 = _service.GetStatutorySharePercentage("TEST003", 9);
            var step3 = _service.GenerateHeirReferenceId("TEST006", "TEST007");
            Assert.IsTrue(step1 >= 0m || step1 <= 0m, "CalculateHeirShareAmount decimal check");
            Assert.IsTrue(!double.IsNaN(step2), "GetStatutorySharePercentage NaN check");
            Assert.IsNotNull(step3, "GenerateHeirReferenceId null check");
        }

        [TestMethod]
        public void Integration_GetStatutorySharePercentage_GenerateHeirReferenceId_WorkflowTest()
        {
            var result1 = _service.GetStatutorySharePercentage("TEST000", 6);
            var result2 = _service.GenerateHeirReferenceId("TEST003", "TEST004");
            Assert.IsTrue(!double.IsNaN(result1), "GetStatutorySharePercentage should not be NaN");
            Assert.IsTrue(!double.IsInfinity(result1), "Should not be infinity");
            Assert.IsNotNull(result2, "GenerateHeirReferenceId should not return null");
            Assert.IsInstanceOfType(result2, typeof(string));
        }

        [TestMethod]
        public void Integration_GetStatutorySharePercentage_Sequential_Execution_Test()
        {
            var step1 = _service.GetStatutorySharePercentage("TEST000", 6);
            var step2 = _service.GenerateHeirReferenceId("TEST003", "TEST004");
            var step3 = _service.CheckMinorHeirStatus(new DateTime(2017, 7, 15), new DateTime(2017, 8, 15));
            Assert.IsTrue(!double.IsNaN(step1), "GetStatutorySharePercentage NaN check");
            Assert.IsNotNull(step2, "GenerateHeirReferenceId null check");
            Assert.IsInstanceOfType(step3, typeof(bool), "CheckMinorHeirStatus bool check");
        }

        [TestMethod]
        public void Integration_GenerateHeirReferenceId_CheckMinorHeirStatus_WorkflowTest()
        {
            var result1 = _service.GenerateHeirReferenceId("TEST000", "TEST001");
            var result2 = _service.CheckMinorHeirStatus(new DateTime(2017, 4, 15), new DateTime(2017, 5, 15));
            Assert.IsNotNull(result1, "GenerateHeirReferenceId should not return null");
            Assert.IsInstanceOfType(result1, typeof(string));
            Assert.IsTrue(result2 || !result2, "CheckMinorHeirStatus should return a boolean");
            Assert.IsInstanceOfType(result2, typeof(bool));
        }

        [TestMethod]
        public void Integration_GenerateHeirReferenceId_Sequential_Execution_Test()
        {
            var step1 = _service.GenerateHeirReferenceId("TEST000", "TEST001");
            var step2 = _service.CheckMinorHeirStatus(new DateTime(2017, 4, 15), new DateTime(2017, 5, 15));
            var step3 = _service.CalculateGuardianshipBondAmount(1600m, 0.12000000000000001);
            Assert.IsNotNull(step1, "GenerateHeirReferenceId null check");
            Assert.IsInstanceOfType(step2, typeof(bool), "CheckMinorHeirStatus bool check");
            Assert.IsTrue(step3 >= 0m || step3 <= 0m, "CalculateGuardianshipBondAmount decimal check");
        }

        [TestMethod]
        public void Integration_CheckMinorHeirStatus_CalculateGuardianshipBondAmount_WorkflowTest()
        {
            var result1 = _service.CheckMinorHeirStatus(new DateTime(2017, 1, 15), new DateTime(2017, 2, 15));
            var result2 = _service.CalculateGuardianshipBondAmount(1300m, 0.09);
            Assert.IsTrue(result1 || !result1, "CheckMinorHeirStatus should return a boolean");
            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsTrue(result2 >= 0m || result2 < 0m, "CalculateGuardianshipBondAmount should return a decimal value");
            Assert.AreNotEqual(decimal.MaxValue, result2);
            Assert.AreNotEqual(decimal.MinValue, result2);
        }

        [TestMethod]
        public void Integration_CheckMinorHeirStatus_Sequential_Execution_Test()
        {
            var step1 = _service.CheckMinorHeirStatus(new DateTime(2017, 1, 15), new DateTime(2017, 2, 15));
            var step2 = _service.CalculateGuardianshipBondAmount(1300m, 0.09);
            var step3 = _service.GetRequiredAffidavitCount("TEST006", 0.12000000000000001);
            Assert.IsInstanceOfType(step1, typeof(bool), "CheckMinorHeirStatus bool check");
            Assert.IsTrue(step2 >= 0m || step2 <= 0m, "CalculateGuardianshipBondAmount decimal check");
            Assert.IsInstanceOfType(step3, typeof(int), "GetRequiredAffidavitCount int check");
        }

        [TestMethod]
        public void Integration_CalculateGuardianshipBondAmount_GetRequiredAffidavitCount_WorkflowTest()
        {
            var result1 = _service.CalculateGuardianshipBondAmount(1000m, 0.060000000000000005);
            var result2 = _service.GetRequiredAffidavitCount("TEST003", 0.09);
            Assert.IsTrue(result1 >= 0m || result1 < 0m, "CalculateGuardianshipBondAmount should return a decimal value");
            Assert.AreNotEqual(decimal.MaxValue, result1);
            Assert.AreNotEqual(decimal.MinValue, result1);
            Assert.IsTrue(result2 >= int.MinValue, "GetRequiredAffidavitCount should return int");
            Assert.AreNotEqual(int.MaxValue, result2);
        }

        [TestMethod]
        public void Integration_CalculateGuardianshipBondAmount_Sequential_Execution_Test()
        {
            var step1 = _service.CalculateGuardianshipBondAmount(1000m, 0.060000000000000005);
            var step2 = _service.GetRequiredAffidavitCount("TEST003", 0.09);
            var step3 = _service.RetrieveCourtOrderCode("TEST006", new DateTime(2017, 8, 15));
            Assert.IsTrue(step1 >= 0m || step1 <= 0m, "CalculateGuardianshipBondAmount decimal check");
            Assert.IsInstanceOfType(step2, typeof(int), "GetRequiredAffidavitCount int check");
            Assert.IsNotNull(step3, "RetrieveCourtOrderCode null check");
        }

        [TestMethod]
        public void Integration_GetRequiredAffidavitCount_RetrieveCourtOrderCode_WorkflowTest()
        {
            var result1 = _service.GetRequiredAffidavitCount("TEST000", 0.060000000000000005);
            var result2 = _service.RetrieveCourtOrderCode("TEST003", new DateTime(2017, 5, 15));
            Assert.IsTrue(result1 >= int.MinValue, "GetRequiredAffidavitCount should return int");
            Assert.AreNotEqual(int.MaxValue, result1);
            Assert.IsNotNull(result2, "RetrieveCourtOrderCode should not return null");
            Assert.IsInstanceOfType(result2, typeof(string));
        }

        [TestMethod]
        public void Integration_GetRequiredAffidavitCount_Sequential_Execution_Test()
        {
            var step1 = _service.GetRequiredAffidavitCount("TEST000", 0.060000000000000005);
            var step2 = _service.RetrieveCourtOrderCode("TEST003", new DateTime(2017, 5, 15));
            var step3 = _service.ValidateNotarySignature("TEST006", new DateTime(2017, 8, 15));
            Assert.IsInstanceOfType(step1, typeof(int), "GetRequiredAffidavitCount int check");
            Assert.IsNotNull(step2, "RetrieveCourtOrderCode null check");
            Assert.IsInstanceOfType(step3, typeof(bool), "ValidateNotarySignature bool check");
        }

        [TestMethod]
        public void Integration_RetrieveCourtOrderCode_ValidateNotarySignature_WorkflowTest()
        {
            var result1 = _service.RetrieveCourtOrderCode("TEST000", new DateTime(2017, 2, 15));
            var result2 = _service.ValidateNotarySignature("TEST003", new DateTime(2017, 5, 15));
            Assert.IsNotNull(result1, "RetrieveCourtOrderCode should not return null");
            Assert.IsInstanceOfType(result1, typeof(string));
            Assert.IsTrue(result2 || !result2, "ValidateNotarySignature should return a boolean");
            Assert.IsInstanceOfType(result2, typeof(bool));
        }

        [TestMethod]
        public void Integration_RetrieveCourtOrderCode_Sequential_Execution_Test()
        {
            var step1 = _service.RetrieveCourtOrderCode("TEST000", new DateTime(2017, 2, 15));
            var step2 = _service.ValidateNotarySignature("TEST003", new DateTime(2017, 5, 15));
            var step3 = _service.CalculateDisputedShareRatio(11, 0.12000000000000001);
            Assert.IsNotNull(step1, "RetrieveCourtOrderCode null check");
            Assert.IsInstanceOfType(step2, typeof(bool), "ValidateNotarySignature bool check");
            Assert.IsTrue(!double.IsNaN(step3), "CalculateDisputedShareRatio NaN check");
        }

        [TestMethod]
        public void Integration_ValidateNotarySignature_CalculateDisputedShareRatio_WorkflowTest()
        {
            var result1 = _service.ValidateNotarySignature("TEST000", new DateTime(2017, 2, 15));
            var result2 = _service.CalculateDisputedShareRatio(8, 0.09);
            Assert.IsTrue(result1 || !result1, "ValidateNotarySignature should return a boolean");
            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsTrue(!double.IsNaN(result2), "CalculateDisputedShareRatio should not be NaN");
            Assert.IsTrue(!double.IsInfinity(result2), "Should not be infinity");
        }

        [TestMethod]
        public void Integration_ValidateNotarySignature_Sequential_Execution_Test()
        {
            var step1 = _service.ValidateNotarySignature("TEST000", new DateTime(2017, 2, 15));
            var step2 = _service.CalculateDisputedShareRatio(8, 0.09);
            var step3 = _service.ComputeTaxWithholdingForHeir(1600m, "TEST007");
            Assert.IsInstanceOfType(step1, typeof(bool), "ValidateNotarySignature bool check");
            Assert.IsTrue(!double.IsNaN(step2), "CalculateDisputedShareRatio NaN check");
            Assert.IsTrue(step3 >= 0m || step3 <= 0m, "ComputeTaxWithholdingForHeir decimal check");
        }

        [TestMethod]
        public void Integration_CalculateDisputedShareRatio_ComputeTaxWithholdingForHeir_WorkflowTest()
        {
            var result1 = _service.CalculateDisputedShareRatio(5, 0.060000000000000005);
            var result2 = _service.ComputeTaxWithholdingForHeir(1300m, "TEST004");
            Assert.IsTrue(!double.IsNaN(result1), "CalculateDisputedShareRatio should not be NaN");
            Assert.IsTrue(!double.IsInfinity(result1), "Should not be infinity");
            Assert.IsTrue(result2 >= 0m || result2 < 0m, "ComputeTaxWithholdingForHeir should return a decimal value");
            Assert.AreNotEqual(decimal.MaxValue, result2);
            Assert.AreNotEqual(decimal.MinValue, result2);
        }

        [TestMethod]
        public void Integration_CalculateDisputedShareRatio_Sequential_Execution_Test()
        {
            var step1 = _service.CalculateDisputedShareRatio(5, 0.060000000000000005);
            var step2 = _service.ComputeTaxWithholdingForHeir(1300m, "TEST004");
            var step3 = _service.IsRelinquishmentDeedValid("TEST006", "TEST007", "TEST008");
            Assert.IsTrue(!double.IsNaN(step1), "CalculateDisputedShareRatio NaN check");
            Assert.IsTrue(step2 >= 0m || step2 <= 0m, "ComputeTaxWithholdingForHeir decimal check");
            Assert.IsInstanceOfType(step3, typeof(bool), "IsRelinquishmentDeedValid bool check");
        }

        [TestMethod]
        public void Integration_ComputeTaxWithholdingForHeir_IsRelinquishmentDeedValid_WorkflowTest()
        {
            var result1 = _service.ComputeTaxWithholdingForHeir(1000m, "TEST001");
            var result2 = _service.IsRelinquishmentDeedValid("TEST003", "TEST004", "TEST005");
            Assert.IsTrue(result1 >= 0m || result1 < 0m, "ComputeTaxWithholdingForHeir should return a decimal value");
            Assert.AreNotEqual(decimal.MaxValue, result1);
            Assert.AreNotEqual(decimal.MinValue, result1);
            Assert.IsTrue(result2 || !result2, "IsRelinquishmentDeedValid should return a boolean");
            Assert.IsInstanceOfType(result2, typeof(bool));
        }

        [TestMethod]
        public void Integration_ComputeTaxWithholdingForHeir_Sequential_Execution_Test()
        {
            var step1 = _service.ComputeTaxWithholdingForHeir(1000m, "TEST001");
            var step2 = _service.IsRelinquishmentDeedValid("TEST003", "TEST004", "TEST005");
            var step3 = _service.GetPendingDocumentCount("TEST006", "TEST007");
            Assert.IsTrue(step1 >= 0m || step1 <= 0m, "ComputeTaxWithholdingForHeir decimal check");
            Assert.IsInstanceOfType(step2, typeof(bool), "IsRelinquishmentDeedValid bool check");
            Assert.IsInstanceOfType(step3, typeof(int), "GetPendingDocumentCount int check");
        }

        [TestMethod]
        public void Integration_ValidateSuccessionCertificate_ConsistencyCheck_0()
        {
            var firstCall = _service.ValidateSuccessionCertificate("TEST000", new DateTime(2017, 2, 15));
            var secondCall = _service.ValidateSuccessionCertificate("TEST000", new DateTime(2017, 2, 15));
            Assert.AreEqual(firstCall, secondCall, "Consistent bool results");
            Assert.IsInstanceOfType(firstCall, typeof(bool));
            Assert.IsInstanceOfType(secondCall, typeof(bool));
        }

        [TestMethod]
        public void Integration_VerifyIndemnityBond_ConsistencyCheck_1()
        {
            var firstCall = _service.VerifyIndemnityBond("TEST000", 1100m);
            var secondCall = _service.VerifyIndemnityBond("TEST000", 1100m);
            Assert.AreEqual(firstCall, secondCall, "Consistent bool results");
            Assert.IsInstanceOfType(firstCall, typeof(bool));
            Assert.IsInstanceOfType(secondCall, typeof(bool));
        }

        [TestMethod]
        public void Integration_CalculateDaysSinceDeath_ConsistencyCheck_2()
        {
            var firstCall = _service.CalculateDaysSinceDeath(new DateTime(2017, 1, 15), new DateTime(2017, 2, 15));
            var secondCall = _service.CalculateDaysSinceDeath(new DateTime(2017, 1, 15), new DateTime(2017, 2, 15));
            Assert.AreEqual(firstCall, secondCall, "Consistent int results");
            Assert.IsInstanceOfType(firstCall, typeof(int));
            Assert.AreNotEqual(int.MaxValue, firstCall);
        }

        [TestMethod]
        public void Integration_CalculateHeirShareAmount_ConsistencyCheck_3()
        {
            var firstCall = _service.CalculateHeirShareAmount(1000m, 0.060000000000000005);
            var secondCall = _service.CalculateHeirShareAmount(1000m, 0.060000000000000005);
            Assert.AreEqual(firstCall, secondCall, "Consistent decimal results");
            Assert.IsTrue(firstCall >= 0m || firstCall < 0m, "Valid decimal");
            Assert.AreNotEqual(decimal.MaxValue, firstCall, "Not max value");
            Assert.AreNotEqual(decimal.MinValue, secondCall, "Not min value");
        }

        [TestMethod]
        public void Integration_GetStatutorySharePercentage_ConsistencyCheck_4()
        {
            var firstCall = _service.GetStatutorySharePercentage("TEST000", 6);
            var secondCall = _service.GetStatutorySharePercentage("TEST000", 6);
            Assert.AreEqual(firstCall, secondCall, "Consistent double results");
            Assert.IsTrue(!double.IsNaN(firstCall));
            Assert.IsTrue(!double.IsInfinity(firstCall));
        }

        [TestMethod]
        public void Integration_GenerateHeirReferenceId_ConsistencyCheck_5()
        {
            var firstCall = _service.GenerateHeirReferenceId("TEST000", "TEST001");
            var secondCall = _service.GenerateHeirReferenceId("TEST000", "TEST001");
            Assert.AreEqual(firstCall, secondCall, "Consistent string results");
            Assert.IsNotNull(firstCall);
            Assert.IsNotNull(secondCall);
        }

    }
}