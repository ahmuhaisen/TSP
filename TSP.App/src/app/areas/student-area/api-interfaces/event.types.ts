export interface StudentEvent {
    id: string;
    eventName: string;
    societyName: string;
    startTime: string;
    locationString: string;
    logoId: string;
    isActiveMember: boolean;
}


export interface AddEventRequest {
    societyId: string;
    committeeId: string;
    title: string;
    description: string;
    location: string;
    type: string;
    startDate: string;
    endDate: string;
    isAttendanceFormEnabled: boolean;
}

export interface MemberEventDetailsDTO {
    societyId: string;
    committeeId: string;
    eventId: string;
    title: string;
    description: string;
    location: string;
    type: string;
    startDate: string;
    endDate: string;
    advisorApproval?: boolean;
    deanAssistantApproval?: boolean;
}

export interface EventSimpleDTO {
    id: string;
    name: string;
    societyName: string;
    startTime: string;
    endTime: string;
    description: string;
    location: string;
    isAttendeesFormEnabled: boolean;
}
