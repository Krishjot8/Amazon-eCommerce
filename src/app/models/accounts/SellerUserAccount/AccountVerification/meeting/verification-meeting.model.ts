import { MeetingStatus, VerificationMeetingType } from "./enums/verification-meeting-enum";

export interface SellerVerificationMeeting{

    id: number;
    meetingType: VerificationMeetingType
    scheduledDateTime : Date;
    preferredLanguage: string;
    notes?: string; // Optional field for additional notes or comments
    meetingStatus: MeetingStatus;
    meetingLink?: string; // Optional field for virtual meeting link

}