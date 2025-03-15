export interface EventSimpleRequest {
    id: string;  
    eventName: string;
    dateTime: Date;  
    locationString: string;
    approvalStatus: string;
    description: string;
    societyName: string;
}