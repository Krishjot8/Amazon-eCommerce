export interface SellerPayoutSetup{

    bankAccountHolderName: string;
    bankName: string;
    accountType: 'Checking' | 'Savings';
    routingNumber: string;
    accountNumber: string;
    swiftBic?: string; // Optional for domestic accounts
    country: string;
    currency: string;
    isDefaultPayoutMethod: boolean;
accountNickname?: string; // Added field for account name

}