export enum ProofOfAddressType
{
   // Financial Statements (0-9)
   BankAccountStatement = 0,
   CreditCardStatement = 1,
   BuildingSocietyStatement = 2,
   MortgageStatement = 3,
   LoanStatement = 4,
   PayoneerStatement = 5,
   HyperwalletStatement = 6,

   // Utility Bills (10-19)
   ElectricityBill = 10,
   WaterBill = 11,
   GasBill = 12,
   InternetBill = 13,
   TvBill = 14,
   TelephoneBill = 15,
   MobilePhoneBill = 16,

   // Housing/Rent (20-29)
   RentBill = 20,
   LeaseAgreement = 21,
   PropertyTaxReceipt = 22,
   
   // Government/Other (30+)
   TaxIdentityDocument = 30,
   CouncilTaxBill = 31,
   Other = 99

}