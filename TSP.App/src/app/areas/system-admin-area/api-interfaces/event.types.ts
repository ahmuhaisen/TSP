export interface EventSimpleRequest {
    id: string;
    eventName: string;
    startDateTime: string;
    locationString: string;
    approvalStatus: string;
    eventDescription: string;
    eventSociety: EventSocietyBasicDto;
    isAdvisorDesignated: boolean;
    isAdvisorInDeanOffice: boolean;
}
export interface EventSocietyBasicDto {
    societyName: string;
    societyDescription: string;
    societyLogoId: string;
}

export interface EventDetailsDTO extends EventSimpleRequest {
    type?: string;
    endDateTime: Date;
    isAdvisorApproved?: boolean;
    isDeanAssistantApproved?: boolean;
    eventRequestDTO: EventRequestDTO;
    advisor: AdvisorBasicDto;
    eventManager: MemberDto;
}

export interface MemberDto {
    studentId: string;
    studentName: string;
    studentEmail: string;
    studentLogoId?: string;
    studentDepartment: string;
    joinYear: number;
    studentRole: string;
    joinedSocietiesNames: string[];
}

export interface AdvisorBasicDto {
    advisorId: string;
    advisorName: string;
    advisorLogoId: string;
}
export interface EventRequestDTO {
    requestTime: Date;
    startTime: Date;
    endTime: Date;
    advisorEmail: string;
    isAttendeesFormEnabled: boolean;
    admins: ApprovalAdministrators[];
}

export interface ApprovalAdministrators {
    facultyMemberName: string;
    facultyMemberEmail: string;
    rank: string;
}
export interface EventRequestDecision {
    eventRequestId: string;
    isAccepted: boolean;
    Remark: string;
}

