export interface StudentBasicDTO {
    id: string;
    fullName: string;
    logoId?: string;
}

export interface MembershipRequestDTO {
    id: string;
    section: string;
    reasonForJoining: string;
    status: string;
    requestedOn: string;
    studentBasicDTO: StudentBasicDTO;
}
export interface UpdateMembershipRequest {
    MembershipRequestId: string;
    SocietyId: string;
    isAccepted: boolean;
}


