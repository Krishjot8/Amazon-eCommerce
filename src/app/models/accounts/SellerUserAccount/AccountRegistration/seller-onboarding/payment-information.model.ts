export interface SellerPaymentInformation {


    cardHolderName: string;

    cardNumber: string;

    expirationMonth: number;
    expirationYear: number;
    securityCode: string;

    billingAddressLine1: string;
    billingAddressLine2?: string; // Optional field
    billingCity: string;
    billingState: string;
    billingZipCode: string;
    billingCountry: string;
}