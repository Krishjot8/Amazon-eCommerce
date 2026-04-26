export interface SellerVerificationOverview{

sellerUserId: number;
currentState: SellerVerificationState;
identityDocsApproved: boolean;
businessAddressPostcardVerified: boolean;
videoMeetingCompleted: boolean;
taxInterviewValidated: boolean;
payoutMethodCleared: boolean;
finalApprovalDate?: Date;

}






export enum SellerVerificationState{
    Incomplete = 0,
    UnderReview = 1,
    ActionRequired = 2,
    Verified = 3,
    Rejected = 4,
    Suspended = 5 


}