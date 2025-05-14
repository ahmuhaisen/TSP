export interface Society {
    id: string;
    name: string;
    description: string;
    logoId: string;
    creationDate: string;
    isCommittee: boolean;
}
export interface MemberAssociatedSociety extends Society {
    position: string;
}

export interface SocietyJoinRequest {
    id: string;
    societyId: string;
    societyName: string;
    societyLogo: string;
    section: string;
    status: 'pending' | 'approved' | 'rejected';
    submissionDate: string;
    motivation: string;
}

export interface JoinSocietyRequest {
    studentId: string;
    societyId: string;
    section: string;
    motivation: string;
}
